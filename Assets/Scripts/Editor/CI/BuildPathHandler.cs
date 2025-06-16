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

        return $"{product} ({buildType} v{version}) ({timestamp} {GetBranchPushVersion()}).{extension}";
    }
    
    static string GetBranchPushVersion()
    {
		string branchName = GetCurrentBranchName();
    	int commitCount = GetCommitCountSinceMidnight();
        return $"({branchName}-{commitCount})";
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

    static int GetCommitCountSinceMidnight()
    {
       string since = DateTime.Now.Date.ToString("yyyy-MM-dd");
    	ProcessStartInfo psi = new ProcessStartInfo
    	{
        	FileName = "git",
        	Arguments = $"log --since=\"{since}\" --oneline",
        	RedirectStandardOutput = true,
        	UseShellExecute = false,
        	CreateNoWindow = true
    	};

    	using (Process process = Process.Start(psi))
    	{
        	process.WaitForExit();
        	string output = process.StandardOutput.ReadToEnd();
        	return output.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
    	}
    }
}
