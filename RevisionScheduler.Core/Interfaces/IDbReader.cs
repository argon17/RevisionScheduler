using RevisionScheduler.Core.Models;

public interface IDbReader
{
    public TopicSet Read(string filePath);
    public TopicSet Read(FileStream fileStream);
}