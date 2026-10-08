using System;
using InterviewApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InterviewApp.Services
{
    public class GreetingService(
        IOptions<GreetingOptions> options,
        ILogger<GreetingService> logger) : IGreetingService
    {
        private readonly GreetingOptions _options = options.Value;

        public string GetGreeting(string timeGreeting)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(timeGreeting);
            return ComposeGreeting(timeGreeting);
        }

        private string ComposeGreeting(string timeGreeting)
        {
            logger.LogInformation("Generating greeting.");
            string message = $"{timeGreeting}! {_options.Message}";
            logger.LogInformation("Displaying greeting: {Greeting}", message);
            return message;
         }
    }
}