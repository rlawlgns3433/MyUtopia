using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using System.Threading;

public class TouchEffect : MonoBehaviour
{
    public GameObject psParentPrefab;
    public int objSize = 15;
    public Canvas particleCanvas;
    private List<GameObject> particleParents;
    private CancellationTokenSource cts;

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

        cts = new CancellationTokenSource();
    }

    private void OnDestroy()
    {
        // 씬 전환 또는 오브젝트 파괴 시 모든 비동기 작업을 중단
        cts.Cancel();
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
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, position, null, out localPoint);
                obj.GetComponent<RectTransform>().anchoredPosition = localPoint;
                DeactiveParticle(obj, cts.Token).Forget();
                break;
            }
        }
    }

    private async UniTaskVoid DeactiveParticle(GameObject parent, CancellationToken token)
    {
        ParticleSystem ps = parent.GetComponentInChildren<ParticleSystem>();
        if (ps != null)
        {
            await UniTask.Delay((int)(ps.main.duration * 1000), cancellationToken: token);

            // 파티클 시스템이 아직 활성 상태이고 오브젝트가 파괴되지 않았는지 확인
            if (parent != null && !token.IsCancellationRequested)
            {
                parent.SetActive(false);
            }
        }
    }
}
