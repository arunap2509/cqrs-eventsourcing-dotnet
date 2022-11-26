namespace DomainLayer.Entities;

public class CommentEntity
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public DateTime CommentDate { get; set; }
    public string Comment { get; set; }
    public bool Edited { get; set; }
    public Guid PostId { get; set; }
    public PostEntity Post { get; set; }
}

