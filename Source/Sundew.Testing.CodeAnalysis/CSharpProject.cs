// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CSharpProject.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
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
using Sundew.Testing.IO;

public sealed class CSharpProject : IProject
{
    private const string SearchPattern = "*.cs";
    private readonly string projectName;
    private readonly string basePath;
    private readonly Paths additionalPaths;
    private readonly string[] excludePaths;

    public CSharpProject(string basePath, Paths? additionalPaths, Paths? excludePaths, References? references)
    {
        this.References = references ?? new References();
        this.basePath = Path.GetFullPath(basePath);
        this.additionalPaths = additionalPaths ?? new Paths();
        this.projectName = Path.GetFileName(basePath);
        excludePaths ??= new Paths();
        this.excludePaths = Array.ConvertAll(excludePaths.FileSystemPaths, input => Path.GetFullPath(Path.Combine(basePath, input))).Concat(this.additionalPaths.FileSystemPaths.SelectMany(s => excludePaths.FileSystemPaths, (s, s1) => Path.GetFullPath(Path.Combine(s, s1)))).ToArray();
    }

    public References References { get; }

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
            this.projectName,
            this.GetFiles().Select(x => CSharpSyntaxTree.ParseText(SourceText.From(File.ReadAllText(x)), null, x)),
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, nullableContextOptions: NullableContextOptions.Enable));
    }

    public IEnumerable<string> GetFiles()
    {
        return Directory.EnumerateFiles(this.basePath, SearchPattern, SearchOption.AllDirectories).Concat(this.additionalPaths.FileSystemPaths.SelectMany(x => Directory.EnumerateFiles(x, SearchPattern, SearchOption.AllDirectories))).Where(this.IsNotExcluded);
    }

    private bool IsNotExcluded(string path)
    {
        return !this.excludePaths.Any(path.StartsWith);
    }
}