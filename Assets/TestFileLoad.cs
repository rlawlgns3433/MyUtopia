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

    private async void Start()
    {
        await LoadAndSaveAssets();
    }

    private async UniTask LoadAndSaveAssets()
    {
        for (int i = 0; i < assets.Count; ++i)
        {
            if (i + 2 < SaveLoadSystem.SaveFileName.Length)
            {
                var assetReference = assets[i];
                AsyncOperationHandle<TextAsset> handle = assetReference.LoadAssetAsync<TextAsset>();

                await handle.Task;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    var path = Path.Combine(Application.persistentDataPath, "Save", SaveLoadSystem.SaveFileName[i + 2]);
                    SaveTextAssetToFile(handle.Result, path);
                    assetReference.ReleaseAsset();
                }
                else
                {
                    Debug.LogError("Failed to load asset: " + assetReference.AssetGUID);
                }
            }
            else
            {
                Debug.LogWarning("Not enough save file names specified in SaveLoadSystem.");
            }
        }
    }

    private void SaveTextAssetToFile(TextAsset asset, string path)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, asset.text);
    }
}
