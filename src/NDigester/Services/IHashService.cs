namespace NDigester.Services;

public interface IHashService
{
    ValueTask<string> GetHashAsync(
        string filePath,
        Commands.HashAlgorithm algorithm,
        CancellationToken cancellationToken = default);
}
