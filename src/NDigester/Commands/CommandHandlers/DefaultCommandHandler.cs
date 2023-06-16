using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.Threading.Tasks;
using NDigester.Services;

namespace NDigester.Commands.CommandHandlers;

public sealed class DefaultCommandHandler : ICommandHandler
{
    private readonly IConsole console;

    private readonly IHashService hashService;

    public DefaultCommandHandler(IConsole console, IHashService hashService)
    {
        this.console = console;
        this.hashService = hashService;
    }

    public string Target { get; set; } = string.Empty;

    public HashAlgorithm Algorithm { get; set; }

    public string? Compare { get; set; }

    public int Invoke(InvocationContext context)
    {
        throw new NotImplementedException();
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(this.Target))
            {
                this.console.Error.WriteLine("No target path specified.");
                return 1;
            }

            var cancellationToken = context.GetCancellationToken();
            var hash = await this.hashService.GetHashAsync(
                this.Target,
                this.Algorithm,
                cancellationToken).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(this.Compare))
            {
                this.console.WriteLine(hash);
                return 0;
            }

            if (hash.Equals(this.Compare, StringComparison.OrdinalIgnoreCase))
            {
                this.console.WriteLine($"{true}");
                return 0;
            }

            this.console.WriteLine($"{false}");
            return -1;
        }
        catch (Exception exception)
        {
            this.console.Error.WriteLine(exception.ToString());
            return -1;
        }
    }
}