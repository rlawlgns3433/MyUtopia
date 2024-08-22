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
        // �� ��ȯ �Ǵ� ������Ʈ �ı� �� ��� �񵿱� �۾��� �ߴ�
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

            // ��ƼŬ �ý����� ���� Ȱ�� �����̰� ������Ʈ�� �ı����� �ʾҴ��� Ȯ��
            if (parent != null && !token.IsCancellationRequested)
            {
                parent.SetActive(false);
            }
        }
    }
}
