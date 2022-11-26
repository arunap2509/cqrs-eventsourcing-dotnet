using CQRS.Core.Commads;

namespace Api.Commands;

public class NewPostCommand: BaseCommand
{
    public string Author { get; set; } = "";
    public string Message { get; set; } = "";
}

