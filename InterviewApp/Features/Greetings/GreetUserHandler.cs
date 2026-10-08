using System.Threading;
using System.Threading.Tasks;
using InterviewApp.Models;
using InterviewApp.Services;
using MediatR;
using Microsoft.Extensions.Options;

namespace InterviewApp.Features.Greetings;

public sealed class GreetUserHandler(
    IOptions<GreetingOptions> options,
    ISender sender,
    IGreetingService greetingService) : IRequestHandler<GreetUserCommand, string>
{
    public async Task<string> Handle(GreetUserCommand request, CancellationToken cancellationToken)
    {
        string timeGreeting = await sender.Send(
            new GetTimeGreetingQuery(options.Value.Language),
            cancellationToken);

        return greetingService.GetGreeting(timeGreeting);
    }
}
