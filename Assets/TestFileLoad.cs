using Spine;
using System.Collections.Generic;
using System.IO;
using UnityEditor.VersionControl;
using UnityEngine;

public class TestFileLoad : MonoBehaviour
{
    public List<TextAsset> assets = new List<TextAsset>();
    private void Start()
    {
        for(int i = 0; i < assets.Count; ++i)
        {
            var path = Path.Combine($"{Application.persistentDataPath}/Save", SaveLoadSystem.SaveFileName[i+2]);
            File.WriteAllText(path, assets[i].text);
        }
    }
}
