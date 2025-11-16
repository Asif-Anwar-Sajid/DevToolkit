using System.Threading.Tasks;

namespace DevToolkit
{
    public static class Debug
    {
        public static void AssertNotNull(object? obj, string message = "Object should not be null")
        {
            if(obj is null)
            {
                throw new ArgumentNullException(nameof(obj), message);
            }
        }
        public static void AssertTrue(bool condition, string message = "Condition should be true")
        {
            if(!condition)
            {
                throw new InvalidOperationException(message);
            }
        }
        public static void LogIf(bool condition, string message)
        {
            if(condition)
            {
               WriteColored(message, ConsoleColor.Cyan, "LOG");
            }
        }
        public static void LogInfo(string message)
        {
            WriteColored(message, ConsoleColor.Green, "INFO");
        }
        public static void LogWarning(string message)
        {
            WriteColored(message, ConsoleColor.Yellow, "WARNING");
        }

        #region Private Methods
        private static void WriteColored(string message, ConsoleColor color, string level)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine($"[{level}] {message}");
            Console.ForegroundColor = previousColor;
        }
        #endregion
    }
}