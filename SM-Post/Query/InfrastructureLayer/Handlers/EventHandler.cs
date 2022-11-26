using DomainLayer.Entities;
using InfrastructureLayer.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PostCommon.Events;

namespace InfrastructureLayer.Handlers;

public class EventHandler : IEventHandler
{
    private readonly IServiceCollection _services;

    public EventHandler(IServiceCollection services)
    {
        _services = services;
    }

    public async Task On(PostCreatedEvent @event)
    {
        using var dbContext = _services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
        var post = new PostEntity
        {
            Id = @event.Id,
            Author = @event.Author,
            Message = @event.Message,
            DatePosted = @event.DatePosted
        };

        dbContext.Posts.Add(post);
        await dbContext.SaveChangesAsync();
    }

    public async Task On(MessageUpdatedEvent @event)
    {
        using var dbContext = _services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
        var post = await dbContext.Posts.Where(x => x.Id == @event.Id).FirstOrDefaultAsync();

        if (post == null)
        {
            return;
        }

        post.Message = @event.Message;

        dbContext.Posts.Update(post);
        await dbContext.SaveChangesAsync();
    }

    public async Task On(PostLikedEvent @event)
    {
        using var dbContext = _services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
        var post = await dbContext.Posts.Where(x => x.Id == @event.Id).FirstOrDefaultAsync();
        post.Likes++;

        dbContext.Posts.Update(post);
        await dbContext.SaveChangesAsync();
    }

    public async Task On(CommentAddedEvent @event)
    {
        using var dbContext = _services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
        var comment = new CommentEntity
        {
            PostId = @event.Id,
            Comment = @event.Comment,
            CommentDate = @event.CommentDate,
            Id = @event.CommentId,
            UserName = @event.UserName,
            Edited = false
        };

        dbContext.Comments.Add(comment);
        await dbContext.SaveChangesAsync();
    }

    public async Task On(CommentUpdatedEvent @event)
    {
        using var dbContext = _services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
        var comment = await dbContext.Comments.Where(x => x.Id == @event.CommentId).FirstOrDefaultAsync();

        if (comment == null)
        {
            return;
        }

        comment.Comment = @event.Comment;
        comment.Edited = true;
        comment.CommentDate = @event.EditDate;

        dbContext.Comments.Update(comment);
        await dbContext.SaveChangesAsync();
    }

    public async Task On(CommentRemovedEvent @event)
    {
        using var dbContext = _services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
        var comment = await dbContext.Comments.Where(x => x.Id == @event.CommentId).FirstOrDefaultAsync();

        if (comment == null)
        {
            return;
        }

        dbContext.Comments.Remove(comment);
        await dbContext.SaveChangesAsync();
    }

    public async Task On(PostRemovedEvent @event)
    {
        using var dbContext = _services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
        var post = await dbContext.Posts.Where(x => x.Id == @event.Id).FirstOrDefaultAsync();

        if (post == null)
        {
            return;
        }

        dbContext.Posts.Remove(post);
        await dbContext.SaveChangesAsync();
    }
}

