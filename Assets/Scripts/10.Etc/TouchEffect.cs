using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class TouchEffect : MonoBehaviour
{
    public GameObject psParentPrefab;
    public int objSize = 15;
    public Canvas particleCanvas;
    private List<GameObject> particleParents;

    public MultiTouchManager multiTouchManager;

    private void Start()
    {
        particleParents = new List<GameObject>();
        for (int i = 0; i < objSize; i++)
        {
            GameObject obj = Instantiate(psParentPrefab, particleCanvas.transform);
            obj.SetActive(false);
            particleParents.Add(obj);
        }
    }

    private void Update()
    {
        if (multiTouchManager.Tap && Touch.activeTouches.Count > 0)
        {
            Vector2 touchPosition = Touch.activeTouches[0].screenPosition;
            ActivateParticleParent(touchPosition);
        }
    }

    private void ActivateParticleParent(Vector2 position)
    {
        foreach (GameObject obj in particleParents)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                Vector2 localPoint;
                RectTransform canvasRectTransform = particleCanvas.GetComponent<RectTransform>();
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform,position,null,out localPoint);
                obj.GetComponent<RectTransform>().anchoredPosition = localPoint;
                DeactiveParticle(obj).Forget();
                break;
            }
        }
    }

    private async UniTaskVoid DeactiveParticle(GameObject parent)
    {
        ParticleSystem ps = parent.GetComponentInChildren<ParticleSystem>();
        if (ps != null)
        {
            await UniTask.Delay((int)(ps.main.duration * 1000));
        }
        parent.SetActive(false);
    }
}
