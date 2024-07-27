using RevisionScheduler.Core.Models;

public interface IDbWriter
{
    public void Write(TopicSet topicSet, FileStream fileStream);
    public void Write(TopicSet topicSet, string filePath);
}