namespace RevisionScheduler.Core;
using Models;

public class RevisionScheduler
{
    private readonly int _availableHoursPerDay;
    private readonly TopicSet topicSet = new TopicSet();

    public RevisionScheduler(int availableHoursPerDay)
    {
        _availableHoursPerDay = availableHoursPerDay;
    }

    public void AddTopic(Topic newTopic) => throw new NotImplementedException();

    public void UpdateTopic(Topic topic) => throw new NotImplementedException();

    public void UpdateState() => throw new NotImplementedException();

    public Dictionary<Topic, int> GetSchedule(DateTime date) => throw new NotImplementedException();
}