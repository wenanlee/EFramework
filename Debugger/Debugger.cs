public class Debugger
{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID
    public void Log(string msg)
    {
        UnityEngine.Debug.Log(msg);
    }
#else
    public void Log(string msg)
    {
        global::System.Console.WriteLine(msg);
    }
#endif
}