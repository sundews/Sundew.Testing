// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyReference.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Testing.CodeAnalysis;

using Microsoft.CodeAnalysis;

/// <summary>
/// Represents a reference to a compiled assembly for use in code analysis or compilation tasks.
/// </summary>
/// <remarks>Use this class to encapsulate an assembly file as a metadata reference, typically when working with Roslyn APIs or other code analysis tools that require assembly references. This class provides a convenient way to create and manage assembly references from file paths.</remarks>
public class AssemblyReference : IReference
{
    private readonly MetadataReference metadataReference;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyReference"/> class for the specified assembly file path.
    /// </summary>
    /// <remarks>The specified path must point to a valid assembly file. This constructor creates a metadata
    /// reference to the assembly, which can be used for compilation or analysis purposes.</remarks>
    /// <param name="path">The file system path to the assembly file to reference. Cannot be null or empty.</param>
    public AssemblyReference(string path)
    {
        this.metadataReference = MetadataReference.CreateFromFile(path, MetadataReferenceProperties.Assembly);
    }

    /// <summary>
    /// Gets the metadata reference associated with this instance.
    /// </summary>
    /// <returns>The <see cref="MetadataReference"/> representing the metadata for this object.</returns>
    public MetadataReference GetMetadataReference()
    {
        return this.metadataReference;
    }
}