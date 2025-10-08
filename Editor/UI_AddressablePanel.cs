using UnityEngine;
using UnityEngine.UI;

public class UI_AddressablePanel : MonoBehaviour
{
    [SerializeField] private AddressableManager _manager;
    [SerializeField] private Button _spawnEssentialBtn;
    [SerializeField] private Button _spawnExtraBtn;
    [SerializeField] private Button _checkHashBtn;

#if UNITY_EDITOR
    [SerializeField] private string _layoutPathA = "Assets/Build/buildLayoutA.json";
    [SerializeField] private string _layoutPathB = "Assets/Build/buildLayoutB.json";
#endif

    private void Start()
    {
        _spawnEssentialBtn.onClick.AddListener(() => _manager.SpawnAssetsByLabel("Essential"));
        _spawnExtraBtn.onClick.AddListener(() => _manager.SpawnAssetsByLabel("Extra"));

#if UNITY_EDITOR
        _checkHashBtn.onClick.AddListener(() =>
            AddressableHashChecker.CompareBuildLayouts(_layoutPathA, _layoutPathB));
#endif
    }
}
