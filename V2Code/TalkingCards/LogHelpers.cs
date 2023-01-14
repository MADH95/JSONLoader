using JLPlugin;

namespace JSONLoader.Data.TalkingCards
{
    internal static class LogHelpers
    {
        internal static void LogInfo(string message)
            => Plugin.Log?.LogInfo(message);

        internal static void LogError(string message)
            => Plugin.Log?.LogError(message);

        internal static void DebugLog(string message)
            => Plugin.Log?.LogDebug(message);
    }
}
