using System.Xml.Serialization;
using RevisionScheduler.Core.Models;

public class TaskReader
{
    public FileStream _fileStream;
    public TaskReader(FileStream fileStream)
    {
        _fileStream = fileStream; 
    }

    public TopicSet Read()
    {
        StreamReader streamReader = new StreamReader(_fileStream);
        XmlSerializer serializer = new XmlSerializer(typeof(TopicSet));
        TopicSet? topicSet = (TopicSet?)serializer.Deserialize(streamReader);
        if(topicSet is not null) return topicSet;
        throw new InvalidDataException($"XmlData in Invalid. Failed to read the FileStream.");
    }
}