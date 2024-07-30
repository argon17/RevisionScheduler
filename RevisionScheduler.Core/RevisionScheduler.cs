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
    private int _pendingChangesCount = 0;
    private const int PENDING_CHANGES_THRESHOLD = 10;

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
        // Hook up application exit event
        // To make sure, we dont' lose any change
        AppDomain.CurrentDomain.ProcessExit += (sender, e) => UpdateDatabase();
    }

    public Topic AddTopic(Topic newTopic)
    {
        ++_pendingChangesCount;
        newTopic.Id = topicSet.GetNewId();
        topicSet.Topics.Add(newTopic);
        ReviseTopic(newTopic);
        UpdateDatabasePeriodic();
        return newTopic;
    }

    public void ReviseTopic(Topic topic)
    {
        ++_pendingChangesCount;
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
        UpdateDatabasePeriodic();
    }

    public void UpdateDatabase()
    {
        _dbWriter.Write(topicSet, _dbPath);
        _pendingChangesCount = 0;
    }

    public void UpdateDatabasePeriodic(){
        if(_pendingChangesCount >= PENDING_CHANGES_THRESHOLD){
            UpdateDatabase();
        }
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
        ++_pendingChangesCount;
        topicSet.Topics = topicSet.Topics.Where(topic => topic.Id!=id).ToList();
        UpdateDatabasePeriodic();
    }
}