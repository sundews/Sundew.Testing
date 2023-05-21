namespace Sundew.Testing.IO;

using System;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

public sealed record Paths(params string[] FileSystemPaths)
{
    public static bool TryFindPathUpwards(string path, [NotNullWhen(true)] out string? foundPath)
    {
        return TryFindPathUpwardsFrom(Directory.GetCurrentDirectory(), path, out foundPath);
    }

    public static bool TryFindPathUpwardsFromEntryAssembly(string path, [NotNullWhen(true)] out string? foundPath)
    {
        var assembly = Assembly.GetEntryAssembly();
        if (assembly == null)
        {
            foundPath = null;
            return false;
        }

        var basePath = Directory.GetParent(assembly.Location);
        if (basePath == null)
        {
            foundPath = null;
            return false;
        }

        return TryFindPathUpwardsFrom(basePath.FullName, path, out foundPath);
    }

    public static bool TryFindPathUpwardsFrom(string basePath, string path, [NotNullWhen(true)] out string? foundPath)
    {
        if (Path.IsPathRooted(path))
        {
            foundPath = null;
            return false;
        }

        var currentPath = basePath;
        foundPath = Path.Combine(currentPath, path);
        while (foundPath != null && !Exists(foundPath))
        {
            if (currentPath == null)
            {
                return false;
            }

            currentPath = Directory.GetParent(currentPath)?.FullName;
            foundPath = currentPath != null ? Path.Combine(currentPath, path) : null;
        }

        return !string.IsNullOrEmpty(foundPath);
    }

    public static string FindPathUpwardsFrom(string basePath, string path)
    {
        if (TryFindPathUpwardsFrom(basePath, path, out var foundPath))
        {
            return foundPath;
        }

        throw CreatePathNotFoundException(path);
    }

    public static string FindPathUpwardsFromEntryAssembly(string path)
    {
        if (TryFindPathUpwardsFromEntryAssembly(path, out var foundPath))
        {
            return foundPath;
        }

        throw CreatePathNotFoundException(path);
    }

    public static string FindPathUpwards(string path)
    {
        if (TryFindPathUpwards(path, out var foundPath))
        {
            return foundPath;
        }

        throw CreatePathNotFoundException(path);
    }

    public static bool Exists(string path)
    {
        return Directory.Exists(path) || File.Exists(path);
    }

    private static Exception CreatePathNotFoundException(string path)
    {
        return new PathNotFoundException($"The path: {path} was not found", path);
    }
}
