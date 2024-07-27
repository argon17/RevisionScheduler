using System.Xml.Serialization;
using RevisionScheduler.Core.Models;

public class TopicSetReader : IDbReader
{
    public TopicSetReader()
    { 
    }

    public TopicSet Read(string filePath)
    {
        if(!Path.Exists(filePath)){
            throw new FileNotFoundException("File not found at DbPath");
        }
        FileStream fileStream = File.OpenRead(filePath);
        return Read(fileStream);
    }

    public TopicSet Read(FileStream fileStream)
    {
        StreamReader streamReader = new StreamReader(fileStream);
        XmlSerializer serializer = new XmlSerializer(typeof(TopicSet));
        TopicSet? topicSet = (TopicSet?)serializer.Deserialize(streamReader);
        if(topicSet is not null) return topicSet;
        throw new InvalidDataException($"XmlData in Invalid. Failed to read the FileStream.");
    }
}