using CQRS.Core.Domain;
using PostCommon.Events;

namespace Domain.Aggregates;

public class PostAggregate : AggregateRoot
{
    private string _author;

    public bool Active { get; private set; }

    private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();

    public PostAggregate()
    {

    }

    public PostAggregate(Guid id, string author, string message)
    {
        RaiseEvent(new PostCreatedEvent
        {
            Id = id,
            Author = author,
            Message = message,
            DatePosted = DateTime.Now
        });
    }

    public void Apply(PostCreatedEvent @event)
    {
        Id = @event.Id;
        Active = true;
        _author = @event.Author;
    }

    public void EditMessage(string message)
    {
        if (!Active)
        {
            throw new InvalidOperationException("you cannot edit and inactive post");
        }

        if (string.IsNullOrEmpty(message))
        {
            throw new InvalidOperationException($"the value of {nameof(message)} cannot be null or empty");
        }

        RaiseEvent(new MessageUpdatedEvent
        {
            Id = Id,
            Message = message
        });
    }

    public void Apply(MessageUpdatedEvent @event)
    {
        Id = @event.Id;
    }

    public void LikePost()
    {
        if (!Active)
        {
            throw new InvalidOperationException("you cannot edit and inactive post");
        }

        RaiseEvent(new PostLikedEvent
        {
            Id = Id
        });
    }

    public void Apply(PostLikedEvent @event)
    {
        Id = @event.Id;
    }

    public void AddComment(string comment, string username)
    {
        if (!Active)
        {
            throw new InvalidOperationException("you cannot edit and inactive post");
        }

        if (string.IsNullOrEmpty(comment))
        {
            throw new InvalidOperationException($"the value of {nameof(comment)} cannot be null or empty");
        }

        RaiseEvent(new CommentAddedEvent
        {
            Id = Id,
            CommentId = Guid.NewGuid(),
            Comment = comment,
            UserName = username,
            CommentDate = DateTime.Now
        });
    }

    public void Apply(CommentAddedEvent @event)
    {
        Id = @event.Id;
        _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.UserName));
    }

    public void EditComment(Guid commentId, string comment, string username)
    {
        if (!Active)
        {
            throw new InvalidOperationException("you cannot edit and inactive post");
        }

        if (string.IsNullOrEmpty(comment))
        {
            throw new InvalidOperationException($"the value of {nameof(comment)} cannot be null or empty");
        }

        if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidOperationException("you cannot edit comment by another user");
        }

        RaiseEvent(new CommentUpdatedEvent
        {
            Id = Id,
            CommentId = commentId,
            Comment = comment,
            UserName = username,
            EditDate = DateTime.Now
        });
    }

    public void Apply(CommentUpdatedEvent @event)
    {
        Id = @event.Id;
        _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.UserName);
    }

    public void RemoveComment(Guid commentId, string username)
    {
        if (!Active)
        {
            throw new InvalidOperationException("you cannot edit and inactive post");
        }

        if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidOperationException("you cannot delete comment by another user");
        }

        RaiseEvent(new CommentRemovedEvent
        {
            Id = Id,
            CommentId = commentId
        });
    }

    public void Apply(CommentRemovedEvent @event)
    {
        Id = @event.Id;
        _comments.Remove(@event.CommentId);
    }

    public void DeletePost(string username)
    {
        if (!Active)
        {
            throw new InvalidOperationException("you cannot edit and inactive post");
        }

        if (!_author.Equals(username, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidOperationException("you cannot delete post by another user");
        }

        RaiseEvent(new PostRemovedEvent
        {
            Id = Id
        });
    }

    public void Apply(PostRemovedEvent @event)
    {
        Id = @event.Id;
        Active = false;
    }
}

