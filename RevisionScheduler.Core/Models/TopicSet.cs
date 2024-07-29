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

    public Dictionary<DateTime, Dictionary<Topic, int>> GetSchedule()
    {
        Dictionary<DateTime, Dictionary<Topic, int>> schedule = [];
        foreach(Topic topic in Topics){
            if(!schedule.ContainsKey(topic.NextRevision)){
                schedule[topic.NextRevision] = [];
            }
            schedule[topic.NextRevision][topic] = topic.RevisionTime;
        }
        return schedule;
    }

    internal int GetNewId(){
        int maxId = Topics.Count==0?0:Topics.Max(topic => topic.Id);
        return ++maxId;
    }
}