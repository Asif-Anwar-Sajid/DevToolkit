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
        public static void LogSmartException(Exception ex, bool printStackTrace = true, bool suggestSolution = true)
        {
            LogException(ex);

            if(printStackTrace) PrintStackTrace(ex);

            if(suggestSolution) SuggestSolutionForException(ex);
        }
        public static void LogException(Exception ex)
        {
            LogError($"Exception: {ex.GetType().Name} - {ex.Message}");
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
        public static void LogError(string message)
        {
            WriteColored(message, ConsoleColor.Red, "ERROR");
        }

        #region Private Methods
        private static void WriteColored(string message, ConsoleColor color, string level)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine($"[{level}] {message}");
            Console.ForegroundColor = previousColor;
        }
        private static void PrintStackTrace(Exception ex)
        {
            if(!string.IsNullOrEmpty(ex.StackTrace))
            {
                WriteColored(ex.StackTrace!, ConsoleColor.DarkRed, "STACKTRACE");
                var lines = ex.StackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach(var line in lines)
                {
                    WriteColored(line, ConsoleColor.DarkGray, "STACKTRACE");
                }
            }
        }
        private static void SuggestSolutionForException(Exception ex)
        {
            string suggestion = GetSuggestionForException(ex);
            
            if(!string.IsNullOrEmpty(suggestion))
            {
                LogInfo($"Suggestion: {suggestion}");   
            }
        }
        private static string GetSuggestionForException(Exception ex)
        {
            if (ex == null) return "";

            string suggestion = ex switch
            {
                NullReferenceException => "Check for null objects before accessing members. Consider using null-conditional operators (?.) or null checks.",
                IndexOutOfRangeException => "Check array/list indices to avoid out-of-bounds access. Verify loops and indexing logic.",
                ArgumentException => "Verify that method arguments are valid and not null. Consider using argument validation or Guard clauses.",
                InvalidOperationException => "Ensure the object is in a valid state before calling this method. Check object initialization or state transitions.",
                DivideByZeroException => "Ensure denominators are not zero before dividing. Add checks before performing division.",
                FileNotFoundException fnf => $"File not found: '{fnf.FileName}'. Check file path and ensure the file exists.",
                DirectoryNotFoundException dnf => $"Directory not found: '{dnf.Message}'. Verify directory paths.",
                FormatException => "Input format is invalid. Ensure strings, numbers, or dates are in correct format before parsing.",
                OverflowException => "A numeric operation exceeded the allowed range. Check arithmetic operations or type limits.",
                KeyNotFoundException => "The specified key does not exist in the collection. Verify dictionary keys before accessing.",
                InvalidCastException => "Object cannot be cast to the specified type. Verify type compatibility before casting.",
                StackOverflowException => "Check for infinite recursion or unbounded loops causing stack overflow.",
                OutOfMemoryException => "The system ran out of memory. Check for memory leaks, large object allocations, or optimize memory usage.",
                _ => ""
            };

            if (ex.InnerException != null)
            {
                suggestion += Environment.NewLine + "Inner Exception: " + GetSuggestionForException(ex.InnerException);
            }

            return suggestion;
        }
        #endregion
    }
}