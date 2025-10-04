// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectReference.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Testing.CodeAnalysis;

using System;
using Microsoft.CodeAnalysis;

/// <summary>
/// Represents a reference to a project that can be used to obtain a metadata reference for compilation purposes.
/// </summary>
/// <remarks>A ProjectReference encapsulates the logic required to retrieve a MetadataReference from a project, enabling scenarios such as dynamic compilation or code analysis that depend on project-level metadata. The metadata reference is created lazily and cached for subsequent calls.</remarks>
public class ProjectReference : IReference
{
    private readonly Lazy<MetadataReference> metadataReference;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectReference"/> class for the specified project.
    /// </summary>
    /// <param name="project">The project to be referenced. Cannot be null.</param>
    public ProjectReference(IProject project)
    {
        this.metadataReference = new Lazy<MetadataReference>(() => project.Compile().ToMetadataReference());
    }

    /// <summary>
    /// Gets the metadata reference associated with this instance.
    /// </summary>
    /// <returns>A <see cref="MetadataReference"/> representing the metadata for the current context.</returns>
    public MetadataReference GetMetadataReference()
    {
        return this.metadataReference.Value;
    }
}