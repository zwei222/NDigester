using System;
using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace NDigester.Services.Implementations;

public sealed class HashService : IHashService
{
    private const int DefaultBufferSize = 4096;

    public async ValueTask<string> GetHashAsync(
        string filePath,
        Commands.HashAlgorithm algorithm,
        CancellationToken cancellationToken = default)
    {
        var fileStream = new FileStream(
            filePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            DefaultBufferSize,
            true);

        await using (fileStream.ConfigureAwait(false))
        {
            return await this.GetHashAsync(fileStream, algorithm, cancellationToken).ConfigureAwait(false);
        }
    }

    private async ValueTask<string> GetHashAsync(
        Stream stream,
        Commands.HashAlgorithm algorithm,
        CancellationToken cancellationToken)
    {
        var bufferSize = this.GetBufferSize(algorithm);
        using var memoryOwner = MemoryPool<byte>.Shared.Rent(bufferSize);
        var buffer = memoryOwner.Memory[..bufferSize];
        var written = algorithm switch
        {
            Commands.HashAlgorithm.MD5 => await this.WriteMd5HashAsync(stream, buffer, cancellationToken)
                .ConfigureAwait(false),
            Commands.HashAlgorithm.SHA1 => await this.WriteSha1HashAsync(stream, buffer, cancellationToken)
                .ConfigureAwait(false),
            Commands.HashAlgorithm.SHA256 => await this.WriteSha256HashAsync(stream, buffer, cancellationToken)
                .ConfigureAwait(false),
            Commands.HashAlgorithm.SHA384 => await this.WriteSha384HashAsync(stream, buffer, cancellationToken)
                .ConfigureAwait(false),
            Commands.HashAlgorithm.SHA512 => await this.WriteSha512HashAsync(stream, buffer, cancellationToken)
                .ConfigureAwait(false),
            _ => throw new NotSupportedException(),
        };

        using var hexBufferOwner = MemoryPool<char>.Shared.Rent(written * 2);
        using var charBufferOwner = MemoryPool<char>.Shared.Rent(2);
        var hexBuffer = hexBufferOwner.Memory;
        var charBuffer = charBufferOwner.Memory[..2];
        var hexWritten = 0;

        for (var index = 0; index < buffer.Length; index++)
        {
            buffer.Span[index].TryFormat(charBuffer.Span, out var charWritten, "x2");

            for (var charIndex = 0; charIndex < charWritten; charIndex++)
            {
                hexBuffer.Span[hexWritten] = charBuffer.Span[charIndex];
                hexWritten++;
            }
        }

        return hexBuffer[..hexWritten].ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ValueTask<int> WriteMd5HashAsync(
        Stream stream,
        Memory<byte> buffer,
        CancellationToken cancellationToken)
    {
        return MD5.HashDataAsync(stream, buffer, cancellationToken);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ValueTask<int> WriteSha1HashAsync(
        Stream stream,
        Memory<byte> buffer,
        CancellationToken cancellationToken)
    {
        return SHA1.HashDataAsync(stream, buffer, cancellationToken);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ValueTask<int> WriteSha256HashAsync(
        Stream stream,
        Memory<byte> buffer,
        CancellationToken cancellationToken)
    {
        return SHA256.HashDataAsync(stream, buffer, cancellationToken);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ValueTask<int> WriteSha384HashAsync(
        Stream stream,
        Memory<byte> buffer,
        CancellationToken cancellationToken)
    {
        return SHA384.HashDataAsync(stream, buffer, cancellationToken);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ValueTask<int> WriteSha512HashAsync(
        Stream stream,
        Memory<byte> buffer,
        CancellationToken cancellationToken)
    {
        return SHA512.HashDataAsync(stream, buffer, cancellationToken);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int GetBufferSize(Commands.HashAlgorithm algorithm)
    {
        return algorithm switch
        {
            Commands.HashAlgorithm.MD5 => 16,
            Commands.HashAlgorithm.SHA1 => 20,
            Commands.HashAlgorithm.SHA256 => 32,
            Commands.HashAlgorithm.SHA384 => 48,
            Commands.HashAlgorithm.SHA512 => 64,
            _ => throw new ArgumentException(nameof(algorithm)),
        };
    }
}