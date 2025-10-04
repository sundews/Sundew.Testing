// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProject.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Testing.CodeAnalysis;

using System.Collections.Generic;
using Microsoft.CodeAnalysis;

/// <summary>
/// Represents a project that can be compiled and queried for its source files.
/// </summary>
/// <remarks>Implementations of this interface provide functionality to compile a project and enumerate its
/// associated files. The specific behavior of compilation and file retrieval may vary depending on the
/// implementation.</remarks>
public interface IProject
{
    /// <summary>
    /// Creates a compilation representing the current state of the project.
    /// </summary>
    /// <remarks>The returned <see cref="Compilation"/> is immutable and can be used for further analysis, code generation, or emitting binaries. Repeated calls may return different results if the project state has changed.</remarks>
    /// <returns>A <see cref="Compilation"/> object that contains the compiled code and associated metadata for the project.</returns>
    Compilation Compile();

    /// <summary>
    /// Retrieves a collection of file paths available in the current context.
    /// </summary>
    /// <returns>An enumerable collection of strings, each representing the full path of a file. The collection is empty if no files are found.</returns>
    IEnumerable<string> GetFiles();
}