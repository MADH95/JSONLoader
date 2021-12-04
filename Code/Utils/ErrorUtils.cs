
namespace JLPlugin.Utils
{
    public struct ErrorUtil
    {
        public static string Card { get; set; }

        public static string Encounter { get; set; }

        public static string Field { get; set; }

        public static string Message { get; set; }

        public static string Region { get; set; }

        public static void LogCard( string Data, string addition = "" )
            => Plugin.Log.LogError( string.Format( string.Concat( Message, addition ), Card, Field, Data ) );

        public static void LogEncounter( string Data, string addition = "" )
            => Plugin.Log.LogError( string.Format( string.Concat( Message, addition ), Encounter, Field, Data ) );

        public static void LogRegion(string Data, string addition = "")
            => Plugin.Log.LogError( string.Format( string.Concat( Message, addition ), Region, Field, Data ) );

        public static void Clear()
        {
            Card = null;
            Encounter = null;
            Field = null;
            Message = null;
        }
    }
}
