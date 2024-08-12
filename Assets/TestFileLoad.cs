using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[Serializable]
public class AssetReferenceTextAsset : AssetReferenceT<TextAsset>
{
    public AssetReferenceTextAsset(string guid) : base(guid)
    {
    }
}

public class TestFileLoad : MonoBehaviour
{
    public List<AssetReferenceTextAsset> assets = new List<AssetReferenceTextAsset>();
    public List<SaveLoadSystem.SaveType> assetTypes = new List<SaveLoadSystem.SaveType>();

    private async void Start()
    {
        await LoadAndSaveAssets();
    }

    private async UniTask LoadAndSaveAssets()
    {
        for (int i = 0; i < assets.Count; ++i)
        {
            var assetReference = assets[i];
            AsyncOperationHandle<TextAsset> handle = assetReference.LoadAssetAsync<TextAsset>();

            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var path = Path.Combine(Application.persistentDataPath, "Save", SaveLoadSystem.SaveFileName[(int)assetTypes[i]]);
                SaveTextAssetToFile(handle.Result, path);
                assetReference.ReleaseAsset();
            }
            else
            {
                Debug.LogError("Failed to load asset: " + assetReference.AssetGUID);
            }
        }
    }

    private void SaveTextAssetToFile(TextAsset asset, string path)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, asset.text);
    }
}
