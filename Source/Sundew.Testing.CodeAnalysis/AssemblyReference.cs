// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyReference.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Testing.CodeAnalysis;

using Microsoft.CodeAnalysis;

public class AssemblyReference : IReference
{
    private readonly MetadataReference metadataReference;

    public AssemblyReference(string path)
    {
        this.metadataReference = MetadataReference.CreateFromFile(path, MetadataReferenceProperties.Assembly);
    }

    public MetadataReference GetMetadataReference()
    {
        return this.metadataReference;
    }
}