using UnityEngine;
using UnityEngine.AddressableAssets;

public class AddressableManager : MonoBehaviour
{
    [SerializeField] private Transform _spawnRoot;

    public async void SpawnAssetsByLabel(string label)
    {
        Debug.Log($"[{label}] �� �ε� ����");

        var handle = Addressables.LoadAssetsAsync<GameObject>(label, obj =>
        {
            var pos = Random.insideUnitSphere * 3f;
            Instantiate(obj, pos, Quaternion.identity, _spawnRoot);
        });

        await handle.Task;

        Debug.Log($"[{label}] ���� {handle.Result.Count}�� ���� �Ϸ�");
    }
}
