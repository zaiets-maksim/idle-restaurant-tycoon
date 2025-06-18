using System;
using UnityEngine;

[CreateAssetMenu(menuName = "StaticData/BuildConfig", fileName = "BuildConfig", order = 0)]
public class BuildConfig : ScriptableObject
{
    [Space(10)]
    public BuildType BuildType = BuildType.Dev;
    [Space(10)]
    public StackTrace StackTraceRelease = new StackTrace();
    public StackTrace StackTraceDev = new StackTrace();
    public StackTrace StackTraceDebug = new StackTrace();
    [Space(10)]
    public bool UseCustomKeystore;
    public string KeystoreName;
    public string KeystorePass;
    public string KeyaliasName;
    public string KeyaliasPass;
}

[Serializable]
public class StackTrace
{
    public StackTraceLogType StackTraceLogType = new StackTraceLogType();
    public LogType[] LogType = Array.Empty<LogType>();
}

public enum BuildType
{
    Release,
    Dev,
    Debug
}
