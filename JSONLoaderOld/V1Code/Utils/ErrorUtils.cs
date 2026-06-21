
namespace JLPlugin.Utils
{
    public struct ErrorUtil
    {
        public static string Card { get; set; }

        public static string Field { get; set; }

        public static string Message { get; set; }

        public static void Log( string Data, string addition = "" )
            => Plugin.Log.LogError( string.Format( string.Concat( Message, addition ), Card, Field, Data ) );

        public static void Clear()
        {
            Card = null;
            Field = null;
            Message = null;
        }
    }
}
