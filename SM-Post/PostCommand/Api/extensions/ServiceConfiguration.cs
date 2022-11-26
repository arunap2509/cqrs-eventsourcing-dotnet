using Api.Commands;
using CQRS.Core.Infrastructure;
using Infrastructure.Dispatchers;

namespace Api.extensions;

public static class ServiceConfiguration
{
    public static void RegisterDispatchHandler(this IServiceCollection services)
    {
        var commandHandler = services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
        var dispatcher = new CommandDispatcher();
        dispatcher.RegisterHandler<NewPostCommand>(commandHandler.HandleAsync);
        dispatcher.RegisterHandler<EditPostCommand>(commandHandler.HandleAsync);
        dispatcher.RegisterHandler<LikePostCommand>(commandHandler.HandleAsync);
        dispatcher.RegisterHandler<AddCommentCommand>(commandHandler.HandleAsync);
        dispatcher.RegisterHandler<EditCommentCommand>(commandHandler.HandleAsync);
        dispatcher.RegisterHandler<RemoveCommentCommand>(commandHandler.HandleAsync);
        dispatcher.RegisterHandler<DeletePostCommand>(commandHandler.HandleAsync);
        dispatcher.RegisterHandler<RestoreReadDbCommand>(commandHandler.HandleAsync);

        services.AddSingleton<ICommandDispatcher>(_ => dispatcher);
    }
}

