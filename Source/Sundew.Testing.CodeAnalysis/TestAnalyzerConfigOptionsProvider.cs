namespace Sundew.Testing.CodeAnalysis;

using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

/// <summary>
/// Provides analyzer configuration options for testing purposes.
/// </summary>
public class TestAnalyzerConfigOptionsProvider : AnalyzerConfigOptionsProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestAnalyzerConfigOptionsProvider"/> class.
    /// </summary>
    /// <param name="globalOptions">The global analyzer configuration options to be used by the provider. Cannot be null.</param>
    public TestAnalyzerConfigOptionsProvider(AnalyzerConfigOptions globalOptions)
    {
        this.GlobalOptions = globalOptions;
    }

    /// <summary>
    /// Gets the global analyzer configuration options that apply to all files in the project or solution.
    /// </summary>
    public override AnalyzerConfigOptions GlobalOptions { get; }

    /// <summary>
    /// Retrieves the configuration options associated with the specified syntax tree.
    /// </summary>
    /// <remarks>This implementation always returns an empty set of options, indicating that no per-file
    /// configuration is provided for syntax trees.</remarks>
    /// <param name="tree">The syntax tree for which to obtain configuration options. Cannot be null.</param>
    /// <returns>An <see cref="AnalyzerConfigOptions"/> instance containing the options for the given syntax tree. Returns an
    /// empty set of options if no configuration is available.</returns>
    public override AnalyzerConfigOptions GetOptions(SyntaxTree tree)
    {
        // Return empty options for syntax trees, or implement per-file config
        return new DictionaryAnalyzerConfigOptions(new Dictionary<string, string>());
    }

    /// <summary>
    /// Retrieves the analyzer configuration options associated with the specified additional text file.
    /// </summary>
    /// <param name="textFile">The additional text file for which to obtain configuration options. Cannot be null.</param>
    /// <returns>An <see cref="AnalyzerConfigOptions"/> instance containing the configuration options for the specified file. If
    /// no options are available, returns an empty set.</returns>
    public override AnalyzerConfigOptions GetOptions(AdditionalText textFile)
    {
        return new DictionaryAnalyzerConfigOptions(new Dictionary<string, string>());
    }
}