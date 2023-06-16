using System.CommandLine;

namespace NDigester.Commands;

public sealed class DefaultCommand : RootCommand
{
    public DefaultCommand()
        : base("Obtains a hash value of a specified file or compares files by hash value.")
    {
        this.SetArguments();
        this.SetOptions();
    }

    private void SetArguments()
    {
        var targetPathArgument = new Argument<string>(
            name: "target",
            description: "Specifies the target file path from which hash values are to be obtained.");

        this.AddArgument(targetPathArgument);
    }

    private void SetOptions()
    {
        var algorithmOption = new Option<HashAlgorithm>(
            aliases: new[]
            {
                "--algorithm",
                "-a",
            },
            description: "Specifies the algorithm of the hash value.",
            getDefaultValue: () => HashAlgorithm.MD5);
        var compareOption = new Option<string>(
            aliases: new[]
            {
                "--compare",
                "-c"
            },
            description: "Specifies a hash value to compare with the target file.");

        this.AddOption(algorithmOption);
        this.AddOption(compareOption);
    }
}