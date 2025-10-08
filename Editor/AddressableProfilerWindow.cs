using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Threading.Tasks;

public class AddressableProfilerWindow : EditorWindow
{
    private string _currentScene = "LobbyScene";
    private string _nextScene = "BattleScene";
    private bool _isLoading = false;

    private SceneInstance _currentSceneInstance;
    private Vector2 _scrollPos;

    [MenuItem("Tools/Addressable Profiler")]
    public static void ShowWindow()
    {
        GetWindow<AddressableProfilerWindow>("Addressable Profiler");
    }

    private void OnGUI()
    {
        GUILayout.Label("Addressables Scene Performance Tester", EditorStyles.boldLabel);
        GUILayout.Space(10);

        _currentScene = EditorGUILayout.TextField("현재 씬 이름", _currentScene);
        _nextScene = EditorGUILayout.TextField("다음 씬 이름", _nextScene);

        EditorGUILayout.Space(10);

        if (!_isLoading)
        {
            if (GUILayout.Button("씬 전환 및 메모리 비교", GUILayout.Height(35)))
            {
                _ = LoadSceneAndProfile();
            }
        }
        else
        {
            GUILayout.Label("씬 로드 중...", EditorStyles.helpBox);
        }

        GUILayout.Space(10);
        DrawMemoryInfo();
    }

    private async Task LoadSceneAndProfile()
    {
        _isLoading = true;
        Debug.Log($"씬 전환 시작: {_currentScene} - {_nextScene}");

        // GC 및 Unity 메모리 측정 전 정리
        System.GC.Collect();
        Resources.UnloadUnusedAssets();

        LogMemoryUsage("로드 전");

        // 현재 씬 언로드
        if (_currentSceneInstance.Scene.isLoaded)
        {
            var unloadHandle = Addressables.UnloadSceneAsync(_currentSceneInstance);
            await unloadHandle.Task;
            Debug.Log($"씬 {_currentScene} 언로드 완료");
        }

        System.GC.Collect();
        Resources.UnloadUnusedAssets();
        LogMemoryUsage("언로드 후");

        // 다음 씬 로드
        float startTime = Time.realtimeSinceStartup;
        var loadHandle = Addressables.LoadSceneAsync(_nextScene, LoadSceneMode.Single);
        _currentSceneInstance = await loadHandle.Task;
        float loadTime = Time.realtimeSinceStartup - startTime;

        Debug.Log($"{_nextScene} 로드 완료 (소요 시간: {loadTime:F2}초)");
        LogMemoryUsage("로드 후");

        _isLoading = false;
    }

    private void LogMemoryUsage(string stage)
    {
        long gcMem = System.GC.GetTotalMemory(false);
        long unityAllocated = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong();
        long unityReserved = UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong();
        long unityUnused = UnityEngine.Profiling.Profiler.GetTotalUnusedReservedMemoryLong();

        Debug.Log($"[{stage}]");
        Debug.Log($"GC 메모리: {gcMem / 1024 / 1024} MB");
        Debug.Log($"Unity Allocated: {unityAllocated / 1024 / 1024} MB");
        Debug.Log($"Unity Reserved: {unityReserved / 1024 / 1024} MB");
        Debug.Log($"Unity Unused: {unityUnused / 1024 / 1024} MB");
        Debug.Log("-----------------------------------------------");
    }

    private void DrawMemoryInfo()
    {
        long gcMem = System.GC.GetTotalMemory(false) / 1024 / 1024;
        long unityMem = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong() / 1024 / 1024;

        GUILayout.Label($"현재 GC 메모리: {gcMem} MB", EditorStyles.boldLabel);
        GUILayout.Label($"현재 Unity 메모리: {unityMem} MB", EditorStyles.boldLabel);
    }
}
