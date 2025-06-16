using System;
using UnityEditor;
using UnityEngine;
using System.Diagnostics;

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
        string timestamp = DateTime.Now.ToString("yyyy-dd-MM HH-mm");

        return $"{product} ({buildType} v{version}) ({timestamp}) ({GetBranchCommitsVersion()}).{extension}";
    }
    
    static string GetBranchCommitsVersion()
    {
		string branchName = GetCurrentBranchName();
    	int commitCount = GetCommitCountToday();
        return $"{branchName}-{commitCount}";
    }

    static string GetCurrentBranchName()
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = "rev-parse --abbrev-ref HEAD",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process process = Process.Start(psi))
        {
            process.WaitForExit();
            return process.StandardOutput.ReadToEnd().Trim();
        }
    }

    static int GetCommitCountToday()
    {
        string output = RunGitCommand("rev-list --count --since=midnight HEAD");
        return int.TryParse(output.Trim(), out int count) ? count : 0;
    }

    static string RunGitCommand(string args)
    {
        ProcessStartInfo psi = new ProcessStartInfo("git", args)
        {
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        using Process process = Process.Start(psi);
        return process.StandardOutput.ReadToEnd();
    }
}
