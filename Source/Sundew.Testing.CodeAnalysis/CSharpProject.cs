// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CSharpProject.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Testing.CodeAnalysis;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Sundew.Base.IO;

/// <summary>
/// Represents a C# project that provides access to source files, references, and compilation functionality.
/// </summary>
/// <remarks>Use this class to manage and compile C# projects by specifying the project path, additional source directories, references, and excluded paths.
/// The class supports enumerating all C# source files in the project, including those in additional paths, while excluding specified directories or files
/// Compilation is performed using the current application domain's assemblies and any additional references provided.</remarks>
public sealed class CSharpProject : IProject
{
    private const string SearchPattern = "*.cs";

    /// <summary>
    /// Initializes a new instance of the <see cref="CSharpProject"/> class.
    /// </summary>
    /// <param name="path">The file path to the C# project file. If the file exists, its directory is used as the base path; otherwise, the
    /// provided path is treated as the base path.</param>
    /// <param name="additionalPaths">An optional collection of additional file system paths to include in the project. If null, an empty collection
    /// is used.</param>
    /// <param name="excludePaths">An optional collection of file system paths to exclude from the project. If null, an empty collection is used.</param>
    /// <param name="references">An optional collection of project references. If null, an empty collection is used.</param>
    public CSharpProject(string path, Paths? additionalPaths = null, Paths? excludePaths = null, References? references = null)
    {
        this.References = references ?? new References();
        this.BasePath = File.Exists(path) ? Directory.GetParent(Path.GetFullPath(path)).FullName : Path.GetFullPath(path);
        this.AdditionalPaths = additionalPaths ?? new Paths();
        this.ProjectName = Path.GetFileNameWithoutExtension(path);
        var actualExcludedPaths = excludePaths ?? new Paths();
        this.ExcludePaths = new Paths(Array.ConvertAll(actualExcludedPaths.FileSystemPaths, input => Path.GetFullPath(Path.Combine(path, input))).Concat(this.AdditionalPaths.FileSystemPaths.SelectMany(s => actualExcludedPaths.FileSystemPaths, (s, s1) => Path.GetFullPath(Path.Combine(s, s1)))).ToArray());
    }

    /// <summary>
    /// Gets the base directory path used for resolving relative file or resource locations.
    /// </summary>
    public string BasePath { get; }

    /// <summary>
    /// Gets the name of the project associated with this instance.
    /// </summary>
    public string ProjectName { get; }

    /// <summary>
    /// Gets the collection of additional file system paths associated with this instance.
    /// </summary>
    public Paths AdditionalPaths { get; }

    /// <summary>
    /// Gets the collection of references associated with the current object.
    /// </summary>
    public References References { get; }

    /// <summary>
    /// Gets the collection of file system paths to exclude from processing.
    /// </summary>
    public Paths ExcludePaths { get; }

    /// <summary>
    /// Creates a C# compilation for the current project using the specified source files and assembly references.
    /// </summary>
    /// <remarks>The returned compilation includes all assemblies loaded in the current application domain, as well as any additional references specified in the project.
    /// The compilation is configured to produce a dynamically linked library (DLL) with nullable reference types enabled.</remarks>
    /// <returns>A <see cref="Compilation"/> object representing the compiled project, including all source files and references.</returns>
    public Compilation Compile()
    {
        var appDomainReferences = AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic)
            .Select(assembly => MetadataReference.CreateFromFile(assembly.Location)).ToArray();
        var references = appDomainReferences.ToList<MetadataReference>();
        foreach (var metadataReference in this.References.ReferenceList.Select(x => x.GetMetadataReference()))
        {
            if (appDomainReferences.All(x => x.Display != metadataReference.Display))
            {
                references.Add(metadataReference);
            }
        }

        return CSharpCompilation.Create(
            this.ProjectName,
            this.GetFiles().Select(x => CSharpSyntaxTree.ParseText(SourceText.From(File.ReadAllText(x)), null, x)),
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, nullableContextOptions: NullableContextOptions.Enable));
    }

    /// <summary>
    /// Returns an enumerable collection of file paths that match the search pattern within the base path and any additional file system paths, including all subdirectories.
    /// </summary>
    /// <remarks>The returned collection includes files from both the base path and any additional file system paths, searching recursively through all subdirectories.
    /// Files that meet exclusion criteria, as determined by the current exclusion logic, are omitted from the results.</remarks>
    /// <returns>An enumerable collection of strings containing the full paths of files that match the search pattern and are not excluded. The collection is empty if no matching files are found.</returns>
    public IEnumerable<string> GetFiles()
    {
        return System.IO.Directory.EnumerateFiles(this.BasePath, SearchPattern, SearchOption.AllDirectories).Concat(this.AdditionalPaths.FileSystemPaths.SelectMany(x => System.IO.Directory.EnumerateFiles(x, SearchPattern, SearchOption.AllDirectories))).Where(this.IsNotExcluded);
    }

    private bool IsNotExcluded(string path)
    {
        return !this.ExcludePaths.FileSystemPaths.Any(path.StartsWith);
    }
}