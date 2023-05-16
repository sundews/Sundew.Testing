namespace Sundew.Testing.CodeAnalysis;

using System.Collections.Generic;
using Microsoft.CodeAnalysis;

public interface IProject
{
    Compilation Compile();

    IEnumerable<string> GetFiles();
}