using System.Globalization;
using RevisionScheduler.Core.Models;

namespace RevisionScheduler.Core.Tests;

public class TaskSerializationTests
{
    [Theory]
    [InlineData("../../../Expected/SampleTopicSetSerialization/TopicSet.xml")]
    public void TestSampleTopicSetFileSerialization(string expectedFilePath)
    {
        // Arrange
        TopicSet topicSet = new TopicSet();
        Topic topic1 = new Topic(){
            Name = "Cognitive Dissonance",
            Category = "Psychology",
            RevisionTime = 30,
            AddedDate = DateTime.ParseExact("2024-07-26", "yyyy-MM-dd", CultureInfo.InvariantCulture)
        };
        topicSet.Topics.Add(topic1);
        string dbPath = "../../../DebugPublic/database.xml";
        if(File.Exists(dbPath)) File.Delete(dbPath);
        string? dbPathDirName = Path.GetDirectoryName(dbPath);
        if(dbPathDirName != null){
            Directory.CreateDirectory(dbPathDirName);
        }

        // Act
        TopicSetWriter taskWriter = new TopicSetWriter();
        taskWriter.Write(topicSet, dbPath);

        // Assert
        StreamReader streamReaderExpected = new StreamReader(expectedFilePath);
        StreamReader streamReaderActual = new StreamReader(dbPath);
        string expectedData = streamReaderExpected.ReadToEnd();
        string actualData = streamReaderActual.ReadToEnd();
        // TODO:
        // Assert.Equal(expectedData, actualData);
    }
}
