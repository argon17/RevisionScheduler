using System.Globalization;
using RevisionScheduler.Core.Models;

namespace RevisionScheduler.Core.Tests;

public class RevisionSchedulerTests
{
    [Theory]
    [InlineData(new int[]{0, 1, 3, 7, 14, 28, 57, 114, 228})]
    public void TestNextRevisionGaps(int[] expectedRevisionGaps)
    {
        // Arrange
        Topic topic = new(){
            Name = "Cognitive Dissonance",
            Category = "Psychology",
            RevisionTime = 30,
            AddedDate = DateTime.ParseExact("2024-07-26", "yyyy-MM-dd", CultureInfo.InvariantCulture)
        };
        IDbReader dbReader = new TopicSetReader();
        IDbWriter dbWriter = new TopicSetWriter();
        string dbPath = "../../../DebugPublic/database.xml";
        if(File.Exists(dbPath)) File.Delete(dbPath);
        string? dbPathDirName = Path.GetDirectoryName(dbPath);
        if(dbPathDirName != null){
            Directory.CreateDirectory(dbPathDirName);
        }
        RevisionScheduler revisionScheduler = new(dbReader, dbWriter, dbPath);
        List<int> nextRevisionGaps = [];

        // Act
        revisionScheduler.AddTopic(topic);
        for(int index=0; index<expectedRevisionGaps.Length; ++index)
        {
            revisionScheduler.ReviseTopic(topic);
            nextRevisionGaps.Add(topic.NextRevisionGap);
        }

        // Assert
        Assert.Equal(expectedRevisionGaps, nextRevisionGaps);
    }
}