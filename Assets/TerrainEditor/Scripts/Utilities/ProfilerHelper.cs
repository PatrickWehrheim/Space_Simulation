
using UnityEngine.Profiling;

public static class ProfilerHelper
{
    public static void BeginThreadProfiling(string threadGroupName, string threadName)
    {
        Profiler.BeginThreadProfiling(threadGroupName, threadName);
    }

    public static void Bla()
    {

    }
}
