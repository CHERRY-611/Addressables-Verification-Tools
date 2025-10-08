using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoaderExample : MonoBehaviour
{
    [SerializeField] private string _currentScene = "LobbyScene";
    [SerializeField] private string _nextScene = "BattleScene";

    private SceneInstance _currentSceneInstance;

    public void LoadNextScene()
    {
        StartCoroutine(LoadSceneCoroutine());
    }

    private IEnumerator LoadSceneCoroutine()
    {
        Debug.Log($"현재 씬: {_currentScene}, 다음 씬: {_nextScene}");

        // 현재 씬 언로드 (Addressables 기준)
        if (_currentSceneInstance.Scene.isLoaded)
        {
            var unloadHandle = Addressables.UnloadSceneAsync(_currentSceneInstance);
            yield return unloadHandle;
            Debug.Log($"씬 {_currentScene} 언로드 완료");
        }

        LogMemoryUsage("언로드 후");

        // 다음 씬 로드
        var loadHandle = Addressables.LoadSceneAsync(_nextScene, LoadSceneMode.Single);
        yield return loadHandle;

        // 새로 로드된 씬의 SceneInstance 저장
        _currentSceneInstance = loadHandle.Result;

        Debug.Log($"씬 {_nextScene} 로드 완료");

        // 메모리 측정
        LogMemoryUsage("로드 후");

        _currentScene = _nextScene;
    }

    private void LogMemoryUsage(string stage)
    {
        long totalMemory = System.GC.GetTotalMemory(false);
        Debug.Log($"[{stage}] 메모리 사용량: {totalMemory / 1024 / 1024} MB");
    }
}
