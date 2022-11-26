using CQRS.Core.Commads;

namespace Api.Commands;

public class RemoveCommentCommand : BaseCommand
{
    public Guid CommentId { get; set; }
    public string UserName { get; set; } = "";
}

