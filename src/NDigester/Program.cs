using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NDigester.Commands;
using NDigester.Commands.CommandHandlers;
using NDigester.Services;
using NDigester.Services.Implementations;

var builder = new CommandLineBuilder(new DefaultCommand());

builder.UseDefaults();
builder.UseHost(
    _ => Host.CreateDefaultBuilder(),
    host =>
    {
        host.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.SetMinimumLevel(LogLevel.Warning);
            logging.AddSimpleConsole();
        });
        host.ConfigureServices(services =>
        {
            services.AddSingleton<IHashService, HashService>();
        });
        host.UseCommandHandler<DefaultCommand, DefaultCommandHandler>();
    });

var parser = builder.Build();

await parser.InvokeAsync(args).ConfigureAwait(false);