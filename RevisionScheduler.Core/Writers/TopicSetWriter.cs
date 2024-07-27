using System.Xml.Serialization;
using RevisionScheduler.Core.Models;

public class TopicSetWriter : IDbWriter
{
    public TopicSetWriter()
    {
    }

    public void Write(TopicSet topicSet, FileStream fileStream)
    {
        StreamWriter streamWriter = new StreamWriter(fileStream);
        XmlSerializer serializer = new XmlSerializer(typeof(TopicSet));
        serializer.Serialize(streamWriter, topicSet);
    }

    public void Write(TopicSet topicSet, string filePath)
    {
        FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        Write(topicSet, fileStream);
        fileStream.Close();
    }
}