using System;
using UnityEditor;
using UnityEngine;

public class BuildPathHandler : MonoBehaviour
{
    public static string BuildPath() => $"{GetProjectPath()}/Artifacts/";
    public static string GetProjectPath() => Application.dataPath.Replace("/Assets", "");
}

public static class BuildNameGenerator
{
    public static string GetApkName(BuildConfig config) => GetName(config, "apk");
    public static string GetAabName(BuildConfig config) => GetName(config, "aab");

    private static string GetName(BuildConfig config, string extension)
    {
        string product = PlayerSettings.productName;
        string version = PlayerSettings.bundleVersion;
        string buildType = config.BuildType.ToString();
        string timestamp = DateTime.Now.ToString("yyyy-dd-mm hh-mm");

        return $"{product} ({buildType} v{version}) ({timestamp}).{extension}";
    }
}
