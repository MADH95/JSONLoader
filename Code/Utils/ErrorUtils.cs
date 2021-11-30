
namespace JLPlugin.Utils
{
    public struct ErrorUtil
    {
        public static string Identifier { get; set; }

        public static string Field { get; set; }

        /// <summary>
        /// {0} - Identifier, {1} - Field, {2} - Data Passed to Log call 
        /// </summary>
        public static string Message { get; set; }

        public static void Log( string Data, string addition = "" )
            => Plugin.Log.LogError( string.Format( string.Concat( Message, addition ), Identifier, Field, Data ) );

        public static void Clear()
        {
            Identifier = null;
            Field = null;
            Message = null;
        }
    }
}
