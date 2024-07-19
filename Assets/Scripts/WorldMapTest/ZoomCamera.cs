using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCamera : MonoBehaviour
{
    public MultiTouchManager touchManager;
    public CinemachineVirtualCamera vc;
    private CinemachineTransposer transposer;
    private void Awake()
    {
        vc = GameObject.FindWithTag(Tags.VirtualCamera).GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if(touchManager.Zoom != 0)
        {
            if (touchManager.Zoom < 0)
            {
                ZoomOut();
            }
            else if (touchManager.Zoom > 0)
            {
                ZoomIn();
            }
        }
    }

    private void ZoomIn()
    {
        if (transposer == null)
        {
            transposer = vc.GetCinemachineComponent<CinemachineTransposer>();
        }
    }
    private void ZoomOut()
    {
        if (transposer == null)
        {
            transposer = vc.GetCinemachineComponent<CinemachineTransposer>();
        }
    }
}
