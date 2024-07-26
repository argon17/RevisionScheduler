using System.Xml.Serialization;

namespace RevisionScheduler.Core.Models;

[Serializable]
[XmlRoot("TopicSet")]
public class TopicSet
{
    [XmlElement("Topic")]
    public List<Topic> Topics { get; set; }

    public TopicSet()
    {
        Topics = new List<Topic>();
    }
}