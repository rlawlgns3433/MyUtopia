using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class AddressableImage : MonoBehaviour
{
    public string id;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        LoadImage();
    }

    private async void LoadImage()
    {
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(id);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            image.type = Image.Type.Sliced;
            image.sprite = handle.Result;
        }
        else
        {
            Debug.LogError("Failed to load image.");
        }
    }
}
