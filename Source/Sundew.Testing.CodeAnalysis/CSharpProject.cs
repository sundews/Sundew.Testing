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

public sealed class CSharpProject : IProject
{
    private const string SearchPattern = "*.cs";

    public CSharpProject(string path, Paths? additionalPaths = null, Paths? excludePaths = null, References? references = null)
    {
        this.References = references ?? new References();
        this.BasePath = File.Exists(path) ? Directory.GetParent(Path.GetFullPath(path)).FullName : Path.GetFullPath(path);
        this.AdditionalPaths = additionalPaths ?? new Paths();
        this.ProjectName = Path.GetFileNameWithoutExtension(path);
        var actualExcludedPaths = excludePaths ?? new Paths();
        this.ExcludePaths = new Paths(Array.ConvertAll(actualExcludedPaths.FileSystemPaths, input => Path.GetFullPath(Path.Combine(path, input))).Concat(this.AdditionalPaths.FileSystemPaths.SelectMany(s => actualExcludedPaths.FileSystemPaths, (s, s1) => Path.GetFullPath(Path.Combine(s, s1)))).ToArray());
    }


    public string BasePath { get; }

    public string ProjectName { get; }

    public Paths AdditionalPaths { get; }

    public References References { get; }
    
    public Paths ExcludePaths { get; }

    public Compilation Compile()
    {
        var appDomainReferences = AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic)
            .Select(assembly => MetadataReference.CreateFromFile(assembly.Location)).ToArray();
        var references = appDomainReferences.ToList<MetadataReference>();
        foreach (var metadataReference in References.ReferenceList.Select(x => x.GetMetadataReference()))
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

    public IEnumerable<string> GetFiles()
    {
        return System.IO.Directory.EnumerateFiles(this.BasePath, SearchPattern, SearchOption.AllDirectories).Concat(this.AdditionalPaths.FileSystemPaths.SelectMany(x => System.IO.Directory.EnumerateFiles(x, SearchPattern, SearchOption.AllDirectories))).Where(this.IsNotExcluded);
    }

    private bool IsNotExcluded(string path)
    {
        return !this.ExcludePaths.FileSystemPaths.Any(path.StartsWith);
    }
}