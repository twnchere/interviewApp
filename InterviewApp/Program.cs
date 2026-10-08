using System;
using System.Threading.Tasks;
using InterviewApp.Features.Greetings;
using InterviewApp.Models;
using InterviewApp.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((_, configuration) =>
                configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true))
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton<IValidateOptions<GreetingOptions>, GreetingOptionsValidator>();
                services.AddOptions<GreetingOptions>()
                    .BindConfiguration("Greeting")
                    .ValidateOnStart();

                services.AddSingleton(TimeProvider.System);
                services.AddSingleton<ITimeGreetingService, TimeGreetingService>();
                services.AddTransient<IGreetingService, GreetingService>();
                services.AddMediatR(configuration =>
                    configuration.RegisterServicesFromAssembly(typeof(Program).Assembly));
            })
            .Build();

        ILogger<Program> logger = host.Services.GetRequiredService<ILogger<Program>>();

        try
        {
            await host.StartAsync();

            string greeting = await host.Services
                .GetRequiredService<ISender>()
                .Send(new GreetUserCommand());

            Console.WriteLine(greeting);
            if (!Console.IsInputRedirected)
            {
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey(intercept: true);
            }

            await host.StopAsync();
            return 0;
        }
        catch (OptionsValidationException exception)
        {
            logger.LogError("Invalid greeting configuration: {Errors}", string.Join("; ", exception.Failures));
            return 1;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to generate the greeting.");
            return 1;
        }
    }
}
