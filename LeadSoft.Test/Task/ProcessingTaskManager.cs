using System.Collections.Concurrent;

public static class ProcessingTasksManager
{
    private static readonly ConcurrentDictionary<string, CancellationTokenSource> _tasks = new();

    public static void AddTask(string groupId, CancellationTokenSource cts)
    {
        _tasks[groupId] = cts;
    }

    public static bool TryGetTask(string groupId, out CancellationTokenSource cts)
    {
        return _tasks.TryGetValue(groupId, out cts);
    }

    public static void RemoveTask(string groupId)
    {
        _tasks.TryRemove(groupId, out _);
    }
}
