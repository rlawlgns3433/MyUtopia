using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class DynamicTextManager : MonoBehaviour
{

    public static DynamicTextData clickData;
    public static DynamicTextData autoWorkData;
    public static GameObject canvasPrefab;
    public static Transform mainCamera;

    public static List<GameObject> usingText = new List<GameObject>();
    public static List<GameObject> unusingText = new List<GameObject>();

    [SerializeField] private DynamicTextData _clickData;
    [SerializeField] private DynamicTextData _autoWorkData;
    [SerializeField] private GameObject _canvasPrefab;
    [SerializeField] private Transform _mainCamera;

    private void Awake()
    {
        clickData = _clickData;
        autoWorkData = _autoWorkData;
        mainCamera = _mainCamera;
        canvasPrefab = _canvasPrefab;
    }

    public static void CreateText2D(Vector2 position, string text, DynamicTextData data)
    {
        GameObject newText;
        if (unusingText.Count <= 0)
        {
            newText = Instantiate(canvasPrefab, position, Quaternion.identity);
            usingText.Add(newText);
        }
        else
        {
            newText = GetText();
            newText.SetActive(true);
            newText.transform.position = position;
            newText.transform.rotation = Quaternion.identity;
        }
        newText.transform.GetComponent<DynamicText>().Initialise(text, data);
    }

    public static void CreateText(Vector3 position, string text, DynamicTextData data)
    {
        GameObject newText = Instantiate(canvasPrefab, position, Quaternion.identity);
        newText.transform.GetComponent<DynamicText>().Initialise(text, data);
    }

    public static void CreateText(Vector3 position, string text, DynamicTextData data, float moveDistance, float moveDuration)
    {
        GameObject newText = Instantiate(canvasPrefab, position, Quaternion.identity);
        data.moveDistance = moveDistance;
        data.moveDuration = moveDuration;
        newText.transform.GetComponent<DynamicText>().Initialise(text, data);
        newText.transform.DOMove(newText.transform.position + Vector3.up * moveDistance, moveDuration).OnComplete(() => Destroy(newText));
    }

    public static void CreateText(Vector3 position, string text, DynamicTextData data, Vector3 moveDelta, float moveDuration)
    {
        GameObject newText = Instantiate(canvasPrefab, position, Quaternion.identity);
        data.moveDelta = moveDelta;
        data.moveDuration = moveDuration;
        newText.transform.GetComponent<DynamicText>().Initialise(text, data);
        newText.transform.DOMove(newText.transform.position + data.moveDelta, data.moveDuration).OnComplete(() => Destroy(newText));
    }

    public static void ReturnText(GameObject text)
    {
        usingText.Remove(text);
        unusingText.Add(text);
        text.SetActive(false);
    }

    public static GameObject GetText()
    {
        var text = unusingText[0];
        usingText.Add(text);
        unusingText.Remove(text);

        return text;
    }

}
