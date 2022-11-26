namespace DomainLayer.Entities;

public class PostEntity
{
    public Guid Id { get; set; }
    public string Author { get; set; }
    public DateTime DatePosted { get; set; }
    public string Message { get; set; }
    public int Likes { get; set; }
    public List<CommentEntity> Comments { get; set; }
}

