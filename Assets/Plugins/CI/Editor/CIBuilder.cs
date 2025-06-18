using System;
using UnityEditor;
using UnityEngine;
using System.IO;

public static class CIBuilder
{
    private static BuildConfig _buildConfig;

    [MenuItem("CI/Build Android", priority = 0)]
    public static void Build()
    {
        _buildConfig = GetBuildConfig();
        switch (_buildConfig.BuildType)
        {
            case BuildType.Release:
                PlayerSettings.stripEngineCode = true;
                BuildAndroid_APK();
                BuildAndroid_AAB();
                break;
            
            case BuildType.Dev:
                DisableSplashScreen();
                PlayerSettings.stripEngineCode = true;
                EditorUserBuildSettings.development = true;
                BuildAndroid_APK();
                break;
            
            case BuildType.Debug:
                DisableSplashScreen();
                EditorUserBuildSettings.allowDebugging = true;
                BuildAndroid_APK();
                break;
        }
    }
    
    [MenuItem("CI/Build Android APK", priority = 1)]
    public static void BuildAndroid_APK()
    {
        _buildConfig = GetBuildConfig();

        BuildAndroid(BuildNameGenerator.GetApkName(_buildConfig));
    }
    
    [MenuItem("CI/Build Android AAB", priority = 2)]
    public static void BuildAndroid_AAB()
    {
        _buildConfig = GetBuildConfig();

        EditorUserBuildSettings.buildAppBundle = true;
        BuildAndroid(BuildNameGenerator.GetAabName(_buildConfig));
    }

    private static void BuildAndroid(string buildName)
    {
        if (_buildConfig == null)
        {
            Debug.LogError("\u2714 BuildConfig does not exist!");
            return;
        }
        
        string path = BuildPathHandler.BuildPath();
        string fullPath = Path.Combine(path, buildName);
        var options = SetupBuildOptions(fullPath);

        SetupStackTrace();
        SetupKeystore();
        TryCreateBuildFolder(path);
        OutputBuildReport(options, fullPath);
    }

    private static void OutputBuildReport(BuildPlayerOptions options, string fullPath)
    {
        var report = BuildPipeline.BuildPlayer(options);
        if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            Debug.LogError($"\u2716 Build failed -> {report.summary.result}");
        else
            Debug.Log($"\u2714 Build succeeded -> {fullPath}");
    }

    private static void TryCreateBuildFolder(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    private static BuildPlayerOptions SetupBuildOptions(string fullPath)
    {
        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = GetEnabledScenes(),
            locationPathName = fullPath,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };
        return options;
    }

    private static void SetupKeystore()
    {
        if (_buildConfig.UseCustomKeystore)
        {
            PlayerSettings.Android.useCustomKeystore = true;
            PlayerSettings.Android.keystoreName = _buildConfig.KeystoreName;
            PlayerSettings.Android.keystorePass = _buildConfig.KeystorePass;
            PlayerSettings.Android.keyaliasName = _buildConfig.KeyaliasName;
            PlayerSettings.Android.keyaliasPass = _buildConfig.KeyaliasPass;
        }
    }

    private static BuildConfig GetBuildConfig()
    {
        string[] guids = AssetDatabase.FindAssets("t:BuildConfig");
        if (guids.Length == 0) return null;
    
        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        return AssetDatabase.LoadAssetAtPath<BuildConfig>(path);
    }

    private static void SetupStackTrace()
    {
        switch (_buildConfig.BuildType)
        {
            case BuildType.Release:
                PlayerSettings.logObjCUncaughtExceptions = false;
                foreach (var logType in _buildConfig.StackTraceRelease.LogType) 
                    PlayerSettings.SetStackTraceLogType(logType, _buildConfig.StackTraceRelease.StackTraceLogType);
                break;
            
            case BuildType.Dev:
                foreach (var logType in _buildConfig.StackTraceDev.LogType) 
                    PlayerSettings.SetStackTraceLogType(logType, _buildConfig.StackTraceDev.StackTraceLogType);
                break;
            
            case BuildType.Debug:
                foreach (var logType in _buildConfig.StackTraceDebug.LogType) 
                    PlayerSettings.SetStackTraceLogType(logType, _buildConfig.StackTraceDebug.StackTraceLogType);
                break;
        }
    }

    private static string[] GetEnabledScenes()
    {
        var scenes = EditorBuildSettings.scenes;
        var list = new System.Collections.Generic.List<string>();
        foreach (var scene in scenes)
        {
            if (scene.enabled)
                list.Add(scene.path);
        }

        return list.ToArray();
    }
    
    private static void DisableSplashScreen()
    {
        PlayerSettings.SplashScreen.show = false;
        PlayerSettings.SplashScreen.logos = Array.Empty<PlayerSettings.SplashScreenLogo>();
    }
}
