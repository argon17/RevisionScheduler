namespace RevisionScheduler.Core;
using Models;
using Serilog;

public class RevisionScheduler
{
    private readonly int _availableMinsPerDay;
    private readonly string _dbPath;
    private readonly TopicSet topicSet;
    private readonly IDbWriter _dbWriter;
    private IDbReader _dbReader;
    private readonly Dictionary<DateTime, Dictionary<Topic, int>> _schedule;

    public RevisionScheduler(IDbReader dbReader, IDbWriter dbWriter, string dbPath, int availableMinsPerDay = 60)
    {
        _dbReader = dbReader;
        _dbWriter = dbWriter;
        _dbPath = dbPath;
        _availableMinsPerDay = availableMinsPerDay;
        try
        {
            topicSet = _dbReader.Read(dbPath);
        }
        catch (FileNotFoundException e)
        {
            Log.Error(e.Message);
            Log.Error($"Couldn't find db at the provided dbPath {dbPath}. Creating new TopicSet.");
            topicSet = new TopicSet();
        }
        catch (InvalidDataException e)
        {
            Log.Error(e.Message);
            Log.Fatal("Unhandled Error Found. Exiting the Application.");
            Environment.Exit(1);
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            Log.Fatal("Unknown Error Found. Exiting the Application.");
            Environment.Exit(1);
        }

        _schedule = topicSet.GetSchedule();
    }

    public void AddTopic(Topic newTopic)
    {
        newTopic.Id = topicSet.GetNewId();
        topicSet.Topics.Add(newTopic);
        ReviseTopic(newTopic);
    }

    public void ReviseTopic(Topic topic)
    {
        topic.RevisionCount++;
        DateTime nextRevision = topic.AddedDate.AddDays(topic.NextRevisionGap);
        int occupiedHours = 0;
        if (!_schedule.ContainsKey(nextRevision))
        {
            _schedule[nextRevision] = [];
        }
        occupiedHours = _schedule[nextRevision].Select(x => x.Value).Aggregate(0, (a, b) => a + b);
        int freeMinutes = _availableMinsPerDay - occupiedHours;
        while (freeMinutes < topic.RevisionTime)
        {
            nextRevision = nextRevision.AddDays(1);
            if (!_schedule.ContainsKey(nextRevision))
            {
                _schedule[nextRevision] = new();
            }
            freeMinutes = _availableMinsPerDay - _schedule[nextRevision].Select(x => x.Value).Aggregate(0, (a, b) => a + b);
        }
        topic.NextRevision = nextRevision;
        _schedule[nextRevision][topic] = topic.RevisionTime;
        UpdateDatabase();
    }

    public void UpdateDatabase()
    {
        _dbWriter.Write(topicSet, _dbPath);
    }
    public Dictionary<Topic, int> GetSchedule(DateTime date)
    {
        if (_schedule.ContainsKey(date))
            return _schedule[date];
        return [];
    }

    public List<Topic> GetTopics()
    {
        return topicSet.Topics;
    }

    public void DeleteTopic(int id)
    {
        topicSet.Topics = topicSet.Topics.Where(topic => topic.Id!=id).ToList();
    }
}