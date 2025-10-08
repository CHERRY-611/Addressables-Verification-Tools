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
        Debug.Log($"���� ��: {_currentScene}, ���� ��: {_nextScene}");

        // ���� �� ��ε� (Addressables ����)
        if (_currentSceneInstance.Scene.isLoaded)
        {
            var unloadHandle = Addressables.UnloadSceneAsync(_currentSceneInstance);
            yield return unloadHandle;
            Debug.Log($"�� {_currentScene} ��ε� �Ϸ�");
        }

        LogMemoryUsage("��ε� ��");

        // ���� �� �ε�
        var loadHandle = Addressables.LoadSceneAsync(_nextScene, LoadSceneMode.Single);
        yield return loadHandle;

        // ���� �ε�� ���� SceneInstance ����
        _currentSceneInstance = loadHandle.Result;

        Debug.Log($"�� {_nextScene} �ε� �Ϸ�");

        // �޸� ����
        LogMemoryUsage("�ε� ��");

        _currentScene = _nextScene;
    }

    private void LogMemoryUsage(string stage)
    {
        long totalMemory = System.GC.GetTotalMemory(false);
        Debug.Log($"[{stage}] �޸� ��뷮: {totalMemory / 1024 / 1024} MB");
    }
}
