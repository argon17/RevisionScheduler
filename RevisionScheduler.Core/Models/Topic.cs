namespace RevisionScheduler.Core.Models;

public class Topic
{
    public required string Name { get; init; }
    public required string Category { get; init; }
    public required int RevisionTime { get; init; }

    public Topic(string name, string category, int revisionTime)
    {
        Name = name;
        Category = category;
        RevisionTime = revisionTime;
    }

    public DifficultyLevel CurrentDifficultyLevel { get; set; } = DifficultyLevel.MEDIUM;
    public ImportanceLevel ImportanceLevel { get; set; } = ImportanceLevel.MEDIUM;
    public int RevisionCount { get; set; } = 1;
    public DateTime LastRevisionDate { get; set; } = DateTime.Today;
    public DateTime NextRevisionDate { get; set; } = DateTime.Today;
}