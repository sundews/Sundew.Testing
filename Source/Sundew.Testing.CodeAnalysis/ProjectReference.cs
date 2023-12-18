// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectReference.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Testing.CodeAnalysis;

using System;
using Microsoft.CodeAnalysis;

public class ProjectReference : IReference
{
    private readonly Lazy<MetadataReference> metadataReference;

    public ProjectReference(IProject project)
    {
        this.metadataReference = new Lazy<MetadataReference>(() => project.Compile().ToMetadataReference());
    }


    public MetadataReference GetMetadataReference()
    {
        return this.metadataReference.Value;
    }
}