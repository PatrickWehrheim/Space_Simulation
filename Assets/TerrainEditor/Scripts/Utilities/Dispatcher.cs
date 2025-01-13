
using System;
using System.Collections.Generic;

public static class Dispatcher
{
    private static bool _queued = false;

    private static List<Action> _actions = new List<Action>();
    private static List<Action> _backlog = new List<Action>();

    public static void RunOnMainThread(Action action)
    {
        lock (_backlog)
        {
            _backlog.Add(action);
            _queued = true;
        }
    }

    public static void Update()
    {
        if (!_queued) return;

        lock (_backlog)
        {
            (_actions, _backlog) = (_backlog, _actions);
            _queued = false;
        }

        foreach (Action action in _actions)
        {
            action();
        }

        _actions.Clear();
    }
}
