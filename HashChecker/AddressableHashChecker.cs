#if UNITY_EDITOR
using UnityEditor.AddressableAssets.Build.Layout;
using UnityEngine;
using System.IO;

public static class AddressableHashChecker
{
    public static void CompareBuildLayouts(string pathA, string pathB)
    {
        string fullPathA = Path.Combine(Application.dataPath, "Build/buildLayoutA.json");
        string fullPathB = Path.Combine(Application.dataPath, "Build/buildLayoutB.json");

        if (!File.Exists(fullPathA) || !File.Exists(fullPathB))
        {
            Debug.LogError($"파일 경로가 잘못되었습니다.\nA: {fullPathA}\nB: {fullPathB}");
            return;
        }

        var layoutA = BuildLayout.Open(fullPathA, readFullFile: true);
        var layoutB = BuildLayout.Open(fullPathB, readFullFile: true);

        foreach (var bundleA in BuildLayoutHelpers.EnumerateBundles(layoutA))
        {
            foreach (var bundleB in BuildLayoutHelpers.EnumerateBundles(layoutB))
            {
                if (bundleA.InternalName == bundleB.InternalName &&
                    bundleA.Hash != bundleB.Hash)
                {
                    Debug.LogWarning($"[해시 변경 감지] {bundleA.Name}");
                }
            }
        }

        Debug.Log("해시 비교 완료");
    }
}
#endif
