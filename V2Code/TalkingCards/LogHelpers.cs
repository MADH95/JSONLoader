using JLPlugin;

namespace JSONLoader.Data.TalkingCards
{
    internal static class LogHelpers
    {
        internal static void LogInfo(string message)
            => Plugin.VerboseLog(message);

        internal static void LogError(string message)
            => Plugin.VerboseLog(message);

        internal static void DebugLog(string message)
            => Plugin.VerboseLog(message);
    }
}
