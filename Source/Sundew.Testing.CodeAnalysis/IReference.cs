namespace Sundew.Testing.CodeAnalysis;

using Microsoft.CodeAnalysis;

public interface IReference
{
    MetadataReference GetMetadataReference();
}