using RevisionScheduler.Core.Models;

namespace RevisionScheduler.Core.Tests;

public class TaskSerializationTests
{
    [Theory]
    [InlineData("../../../Expected/SampleTopicSetSerialization/TopicSet.xml")]
    public void TestSampleTaskFileSerialization(string expectedFilePath)
    {
        // Arrange
        TopicSet topicSet = new TopicSet();
        Topic topic1 = new Topic(){
            Name = "Cognitive Dissonance",
            Category = "Psychology",
            RevisionTime = 30
        };
        topicSet.Topics.Add(topic1);
        string filePath = "../../../TopicSet.xml";

        // Act
        TaskWriter taskWriter = new TaskWriter(topicSet);
        taskWriter.Write(filePath);

        // Assert
        StreamReader streamReaderExpected = new StreamReader(expectedFilePath);
        StreamReader streamReaderActual = new StreamReader(filePath);
        string expectedData = streamReaderExpected.ReadToEnd();
        string actualData = streamReaderActual.ReadToEnd();
        Assert.Equal(expectedData, actualData);
    }
}