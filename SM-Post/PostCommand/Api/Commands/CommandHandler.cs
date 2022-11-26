using CQRS.Core.Handlers;
using Domain.Aggregates;

namespace Api.Commands;

public class CommandHandler : ICommandHandler
{
    private readonly IEventSourceHandler<PostAggregate> _eventSourcingHanlder;

    public CommandHandler(IEventSourceHandler<PostAggregate> eventSourcingHanlder)
    {
        _eventSourcingHanlder = eventSourcingHanlder;
    }

    public async Task HandleAsync(NewPostCommand command)
    {
        var aggregate = new PostAggregate(command.Id, command.Author, command.Message);
        await _eventSourcingHanlder.SaveAsync(aggregate);
    }

    public async Task HandleAsync(EditPostCommand command)
    {
        var aggregate = await _eventSourcingHanlder.GetByIdAsync(command.Id);
        aggregate.EditMessage(command.Message);

        await _eventSourcingHanlder.SaveAsync(aggregate);
    }

    public async Task HandleAsync(LikePostCommand command)
    {
        var aggregate = await _eventSourcingHanlder.GetByIdAsync(command.Id);
        aggregate.LikePost();

        await _eventSourcingHanlder.SaveAsync(aggregate);
    }

    public async Task HandleAsync(DeletePostCommand command)
    {
        var aggregate = await _eventSourcingHanlder.GetByIdAsync(command.Id);
        aggregate.DeletePost(command.UserName);

        await _eventSourcingHanlder.SaveAsync(aggregate);
    }

    public async Task HandleAsync(AddCommentCommand command)
    {
        var aggregate = await _eventSourcingHanlder.GetByIdAsync(command.Id);
        aggregate.AddComment(command.Comment, command.UserName);

        await _eventSourcingHanlder.SaveAsync(aggregate);
    }

    public async Task HandleAsync(EditCommentCommand command)
    {
        var aggregate = await _eventSourcingHanlder.GetByIdAsync(command.Id);
        aggregate.EditComment(command.CommentId, command.Comment, command.UserName);

        await _eventSourcingHanlder.SaveAsync(aggregate);
    }

    public async Task HandleAsync(RemoveCommentCommand command)
    {
        var aggregate = await _eventSourcingHanlder.GetByIdAsync(command.Id);
        aggregate.RemoveComment(command.CommentId, command.UserName);

        await _eventSourcingHanlder.SaveAsync(aggregate);
    }

    public async Task HandleAsync(RestoreReadDbCommand command)
    {
        await _eventSourcingHanlder.RepublishEventsAsync();
    }
}

