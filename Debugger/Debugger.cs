public class Debugger
{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID
    public static void Log(params string[] msg)
    {
        UnityEngine.Debug.Log("<color=white> >>> " + System.String.Join("", msg)+ "</color>");
    }
    public static void LogError(params string[] msg)
    {
        UnityEngine.Debug.LogError("<color=red> >>> " + System.String.Join("", msg) + "</color>");
    }
    public static void LogWarning(params string[] msg)
    {
        UnityEngine.Debug.LogWarning("<color=yellow> >>> " + System.String.Join("", msg) + "</color>");
    }

#else
        public static void Log(params string[] msg)
        {
            Print(System.ConsoleColor.White, System.String.Join("", msg));
        }
        public static void LogError(params string[] msg)
        {
            Print(System.ConsoleColor.Red, System.String.Join("", msg));
        }
        public static void LogWarning(params string[] msg)
        {
            Print(System.ConsoleColor.Yellow, System.String.Join("", msg));
        }
        private static void Print(System.ConsoleColor color, string logs)
        {
            System.Console.ForegroundColor = color;
            System.Console.WriteLine(System.DateTime.Now.TimeOfDay + " >>> " + logs);
            System.Console.ForegroundColor = System.ConsoleColor.White;
        }
#endif
}