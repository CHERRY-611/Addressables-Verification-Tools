using UnityEngine;
using UnityEngine.AddressableAssets;

public class AddressableManager : MonoBehaviour
{
    [SerializeField] private Transform _spawnRoot;

    public async void SpawnAssetsByLabel(string label)
    {
        Debug.Log($"[{label}] 라벨 로드 시작");

        var handle = Addressables.LoadAssetsAsync<GameObject>(label, obj =>
        {
            var pos = Random.insideUnitSphere * 3f;
            Instantiate(obj, pos, Quaternion.identity, _spawnRoot);
        });

        await handle.Task;

        Debug.Log($"[{label}] 에셋 {handle.Result.Count}개 스폰 완료");
    }
}
