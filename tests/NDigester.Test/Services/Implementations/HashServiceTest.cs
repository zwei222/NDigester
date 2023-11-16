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
    [InlineData(HashAlgorithm.MD5, "92a864c81117d9d44e8e425beec7fb2b")]
    [InlineData(HashAlgorithm.SHA1, "709397c236215a7bb8667546249eef0c58081399")]
    [InlineData(HashAlgorithm.SHA256, "248cd683e6dfaaf8aace50cbe4c673111820bb26545e60eea1290056c9557eb1")]
    [InlineData(HashAlgorithm.SHA384, "b98e991c2e0df3023dd2b7717cde53ba277a31092d17d175d02120ac76ebd891c4467346420def00a08aa700441a305d")]
    [InlineData(HashAlgorithm.SHA512, "f7e611082472e9695ca4135814e45c82bbd5035fb6ceb244ed4a5bc199f47cae42c73b5edfa7b1fe7bf72f0462dbf406ea8918b2af8270ede54663738c41297a")]
    [InlineData(HashAlgorithm.XxHash32, "c11cb0ab")]
    [InlineData(HashAlgorithm.XxHash64, "d7f9ed92086efd36")]
    [InlineData(HashAlgorithm.XxHash3, "78c460dcba1226cc")]
    [InlineData(HashAlgorithm.XxHash128, "5581408ded4f7e3578c460dcba1226cc")]
    public async Task HashTest(HashAlgorithm algorithm, string expected)
    {
        var actual = await this.hashService.GetHashAsync(SampleFilePath, algorithm);

        Assert.Equal(expected, actual);
    }
}
