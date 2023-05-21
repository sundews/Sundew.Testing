namespace Sundew.Testing.IO;

using System.IO;

public class PathNotFoundException : IOException
{
    public string Path { get; }

    public PathNotFoundException(string path)
    {
        this.Path = path;
    }

    public PathNotFoundException(string message, string path)
        : base(message)
    {
        this.Path = path;
    }
}