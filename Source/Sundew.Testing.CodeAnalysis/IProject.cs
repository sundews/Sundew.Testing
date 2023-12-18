// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProject.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Testing.CodeAnalysis;

using System.Collections.Generic;
using Microsoft.CodeAnalysis;

public interface IProject
{
    Compilation Compile();

    IEnumerable<string> GetFiles();
}