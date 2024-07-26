using System.Xml.Serialization;
using RevisionScheduler.Core.Models;

public class TaskWriter
{
    public TopicSet _topicSet;
    public TaskWriter(TopicSet topicSet)
    {
        _topicSet = topicSet; 
    }

    public void Write(FileStream fileStream)
    {
        StreamWriter streamWriter = new StreamWriter(fileStream);
        XmlSerializer serializer = new XmlSerializer(typeof(TopicSet));
        serializer.Serialize(streamWriter, _topicSet);
    }

    public void Write(string filePath)
    {
        FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        Write(fileStream);
        fileStream.Close();
    }
}