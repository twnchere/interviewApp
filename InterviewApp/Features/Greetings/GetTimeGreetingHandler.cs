using System.Threading;
using System.Threading.Tasks;
using InterviewApp.Services;
using MediatR;

namespace InterviewApp.Features.Greetings;

public sealed class GetTimeGreetingHandler(ITimeGreetingService timeGreetingService)
    : IRequestHandler<GetTimeGreetingQuery, string>
{
    public Task<string> Handle(GetTimeGreetingQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(timeGreetingService.GetGreeting(request.Language));
    }
}
