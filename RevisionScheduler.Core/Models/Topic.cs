using System.Xml.Serialization;

namespace RevisionScheduler.Core.Models;

[Serializable]
public class Topic
{
    [XmlElement("Name")]
    public required string Name { get; init; }
    [XmlElement("Category")]
    public required string Category { get; init; }
    [XmlElement("RevisionTime")]
    public required int RevisionTime { get; init; }

    public Topic()
    {
    }

    [XmlElement("CurrentDifficultyLevel")]
    public DifficultyLevel CurrentDifficultyLevel { get; set; } = DifficultyLevel.MEDIUM;
    [XmlElement("ImportanceLevel")]
    public ImportanceLevel ImportanceLevel { get; set; } = ImportanceLevel.MEDIUM;
    [XmlElement("RevisionCount")]
    public int RevisionCount { get; set; } = 1;
    private DateTime lastRevision = DateTime.Today;
    [XmlElement("LastRevision")]
    public string LastRevision
    {
        get { return lastRevision.ToString("yyyy-MM-dd"); }
        set { lastRevision = DateTime.Parse(value); }
    }

    private DateTime nextRevision = DateTime.Today;
    [XmlElement("NextRevision")]
    public string NextRevision
    {
        get { return nextRevision.ToString("yyyy-MM-dd"); }
        set { nextRevision = DateTime.Parse(value); }
    }
}