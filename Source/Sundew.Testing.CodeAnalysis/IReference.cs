// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReference.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Testing.CodeAnalysis;

using Microsoft.CodeAnalysis;

/// <summary>
/// Represents a reference to external metadata, such as an assembly or module, that can be used for compilation or
/// analysis.
/// </summary>
/// <remarks>Implementations of this interface provide access to a <see cref="MetadataReference"/> instance, which
/// can be supplied to compilation APIs or code analysis tools. This interface abstracts the source of the reference,
/// allowing for different implementations that may load metadata from files, streams, or other sources.</remarks>
public interface IReference
{
    /// <summary>
    /// Gets a metadata reference that can be used for compilation or analysis purposes.
    /// </summary>
    /// <returns>A <see cref="MetadataReference"/> representing the metadata for the associated assembly or resource.</returns>
    MetadataReference GetMetadataReference();
}