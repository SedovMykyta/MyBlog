namespace MyBlog.Infrastructure.Entities;

public class Article
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; }

    public string Desctiprion { get; set; }

    public string FullText { get; set; }

    //Для хранения картинки к статье, я не уверен что именно так выглядит поле для нее, но допустим
    public byte[]? Image { get; set; }
}