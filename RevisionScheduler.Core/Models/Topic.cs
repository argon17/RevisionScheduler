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

    [XmlElement("DifficultyLevel")]
    public DifficultyLevel DifficultyLevel { get; set; } = DifficultyLevel.MEDIUM;
    [XmlElement("ImportanceLevel")]
    public ImportanceLevel ImportanceLevel { get; set; } = ImportanceLevel.MEDIUM;
    [XmlElement("RevisionCount")]
    public int RevisionCount { get; set; } = 0;
    [XmlElement("AddedDate")]
    public DateTime AddedDate { get; set; } = DateTime.Today;
    [XmlElement("NextRevision")]
    public DateTime NextRevision { get; set; } = DateTime.Today;
    /// <summary>
    /// 
    /// </summary>
    public int NextRevisionGap => CalculateOptimalNextRevisionGap();
    private int CalculateOptimalNextRevisionGap()
    {
        /*
        nextRevisionGap inv ImportanceLevel
        inv DifficultyLevel
        exp RevisionCount
        RevisionTime -> Constant(Will change it later)
        DifficultyLevel ImportanceLevel
        Medium          Medium

        nextRevisionGap depends on RevisionCount
        Ebbinghaus Forgetting Curve and Retention Function
        R(t) = exp(-kt); k is the Decay Constant
        t = -ln[R(t)]/k

        Assumptions
        1. After each revision, Retention resets to 100% & Decay Constant gets halved
        2. We need to revise when Retention drops to 80%
        3. Initial Decay Constant k = 0.5

        Calculation of nth Revision:
        1st revision at t_1 = -ln(0.8)/k
        2nd revision at t_2 = -2ln(0.8)/k
        nth revision at t_n = -[2^(n-1)]ln(0.8)/k

        -ln(0.8)/0.5 = 0.44628710262
        */
        const double factor = 0.44628710262;
        int nextRevisionGap = (int)(factor * Math.Pow(2, RevisionCount - 1));
        return nextRevisionGap;
    }
}