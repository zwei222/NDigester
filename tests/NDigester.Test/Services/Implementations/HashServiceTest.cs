using NDigester.Commands;
using NDigester.Services;
using NDigester.Services.Implementations;
using Xunit;

namespace NDigester.Test.Services.Implementations;

public class HashServiceTest
{
    private const string SampleFilePath = @"./Resources/sample.txt";

    private readonly IHashService hashService;

    public HashServiceTest()
    {
        this.hashService = new HashService();
    }

    [Theory(DisplayName = "Hash test")]
    [InlineData(HashAlgorithm.MD5, "b5adc7195f55640db1a983130705f0e2")]
    [InlineData(HashAlgorithm.SHA1, "7df0e377c46e2f18cb0b1ee3083d3de5d4fe265c")]
    [InlineData(HashAlgorithm.SHA256, "c07491defe6f75fa3a0abcd454607768a1e3176a22f47624a23d61ac425e359f")]
    [InlineData(HashAlgorithm.SHA384, "c27d06484d21a7842b057166939ba18d44ba84f12e15684f022877d94a45cfd8d5fef71c38e145bf264ada4c725339ec")]
    [InlineData(HashAlgorithm.SHA512, "3fbdd45ce9d5aaf652072c39e23e2eaf35b0fe5cd8ee97a2b60717023d2ba8fdd52c6640a404f8114edf8bbadc2da7e6ddb3b3346ee824b840434e9bdb7e3d95")]
    [InlineData(HashAlgorithm.XxHash32, "58906e01")]
    [InlineData(HashAlgorithm.XxHash64, "a597d5073c8d02a5")]
    [InlineData(HashAlgorithm.XxHash3, "264b10b4067f3274")]
    [InlineData(HashAlgorithm.XxHash128, "7d1249808b3f7cc4264b10b4067f3274")]
    public async Task HashTest(HashAlgorithm algorithm, string expected)
    {
        var actual = await this.hashService.GetHashAsync(SampleFilePath, algorithm);

        Assert.Equal(expected, actual);
    }
}
