using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldMapTutorial : MonoBehaviour
{
    public bool tutorialComplete = false;
    public TutorialTextFormation[] tutorialTextFormations;
    public GameObject[] targetObjects;
    public bool[] targetRayCast;
    public string[] tutorialStringFormat;
    public Image target;
    public GameObject[] tutorialTexts;
    public Button[] skipButtons;
    public GameObject[] tutorialSkipButtons;
    public int count = 0;
    public GameObject cursor;
    public GameObject empty;
    public int targetAnimalId;
    public bool stopDrag = false;
    public bool isStop = false;
    public GameObject focusImage;
    public WorldMapTutorialProgress progress;
    public WorldMapManager manager;
    private bool settingTutorial = false;
    public void CheckTutorial()
    {
        count = 0;
        if (PlayerPrefs.GetInt("WorldTutorialCheck") == 0 || !PlayerPrefs.HasKey("WorldTutorialCheck"))
        {
            PlayerPrefs.SetInt("WorldTutorialCheck", 0);
            gameObject.SetActive(true);
            target.gameObject.SetActive(true);
            SetTutorial(count);
            stopDrag = true;
            progress = WorldMapTutorialProgress.None;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    public void SetTutorial(int count)
    {
        if (isStop)
            return;

        if (tutorialComplete)
        {
            return;
        }

        if (!target.gameObject.activeSelf)
        {
            target.gameObject.SetActive(true);
        }

        if (targetObjects[count].gameObject.layer == LayerMask.NameToLayer("UI"))
        {
            var rect = targetObjects[count].GetComponent<RectTransform>();
            TargettingUiObject(rect);
        }
        else
        {
            TargettingWorldObject(targetObjects[count]);
        }
        SetTextFormation(tutorialTextFormations[count], tutorialStringFormat[count]);
        SetTargetRayCast(count);
        settingTutorial = false;
    }

    private void SetTargetRayTrue()
    {
        var obj = target.GetComponent<Image>();
        obj.raycastTarget = true;
    }

    private void SetTargetRayCast(int count)
    {
        var obj = target.GetComponent<Image>();
        if (obj.raycastTarget != targetRayCast[count])
        {
            obj.raycastTarget = targetRayCast[count];
        }
    }

    private void TargettingUiObject(RectTransform obj)
    {
        var rect = obj.GetComponent<RectTransform>();

        target.rectTransform.position = rect.position;

        target.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        target.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        target.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        target.rectTransform.sizeDelta = rect.rect.size;
        target.rectTransform.localScale = rect.localScale;
    }
    private void TargettingWorldObject(GameObject targetObject)
    {
        if (targetObject != null)
        {
            var worldPosition = targetObject.transform.position;
            var screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            var canvasRectTransform = target.rectTransform.parent as RectTransform;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPosition, null, out Vector2 localPoint))
            {
                target.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                target.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                target.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                target.rectTransform.anchoredPosition = localPoint;
                var collider = targetObject.GetComponent<Collider>();
                if (collider != null)
                {
                    var screenSizeMin = Camera.main.WorldToScreenPoint(collider.bounds.min);
                    var screenSizeMax = Camera.main.WorldToScreenPoint(collider.bounds.max);
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenSizeMin, null, out Vector2 uiSizeMin);
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenSizeMax, null, out Vector2 uiSizeMax);
                    var uiSize = uiSizeMax - uiSizeMin;
                    target.rectTransform.sizeDelta = new Vector2(uiSize.x, uiSize.y);
                }
            }
        }
    }
    public void SetTextFormation(TutorialTextFormation formation, string tutorialText)
    {
        switch (formation)
        {
            case TutorialTextFormation.None:
                for (int i = 0; i < tutorialTexts.Length; ++i)
                {
                    tutorialTexts[i].gameObject.SetActive(false);
                    skipButtons[i].gameObject.SetActive(false);
                }
                break;
            case TutorialTextFormation.Upper:
                tutorialTexts[(int)TutorialTextFormation.Upper].gameObject.SetActive(true);
                skipButtons[(int)TutorialTextFormation.Upper].gameObject.SetActive(true);
                var upperText = tutorialTexts[(int)TutorialTextFormation.Upper].GetComponentInChildren<TextMeshProUGUI>();
                upperText.text = tutorialText;
                tutorialTexts[(int)TutorialTextFormation.Middle].gameObject.SetActive(false);
                skipButtons[(int)TutorialTextFormation.Middle].gameObject.SetActive(false);
                tutorialTexts[(int)TutorialTextFormation.Bottom].gameObject.SetActive(false);
                skipButtons[(int)TutorialTextFormation.Bottom].gameObject.SetActive(false);
                break;
            case TutorialTextFormation.Middle:
                tutorialTexts[(int)TutorialTextFormation.Upper].gameObject.SetActive(false);
                skipButtons[(int)TutorialTextFormation.Upper].gameObject.SetActive(false);
                tutorialTexts[(int)TutorialTextFormation.Middle].gameObject.SetActive(true);
                skipButtons[(int)TutorialTextFormation.Middle].gameObject.SetActive(true);
                var middleText = tutorialTexts[(int)TutorialTextFormation.Middle].GetComponentInChildren<TextMeshProUGUI>();
                middleText.text = tutorialText;
                tutorialTexts[(int)TutorialTextFormation.Bottom].gameObject.SetActive(false);
                skipButtons[(int)TutorialTextFormation.Bottom].gameObject.SetActive(false);
                break;
            case TutorialTextFormation.Bottom:
                tutorialTexts[(int)TutorialTextFormation.Upper].gameObject.SetActive(false);
                skipButtons[(int)TutorialTextFormation.Upper].gameObject.SetActive(false);
                tutorialTexts[(int)TutorialTextFormation.Middle].gameObject.SetActive(false);
                skipButtons[(int)TutorialTextFormation.Middle].gameObject.SetActive(false);
                tutorialTexts[(int)TutorialTextFormation.Bottom].gameObject.SetActive(true);
                skipButtons[(int)TutorialTextFormation.Bottom].gameObject.SetActive(true);
                var bottomText = tutorialTexts[(int)TutorialTextFormation.Bottom].GetComponentInChildren<TextMeshProUGUI>();
                bottomText.text = tutorialText;
                break;
        }

    }

    public void SetEmpty()
    {
        cursor.gameObject.SetActive(false);
        var rect = empty.GetComponent<RectTransform>();
        TargettingUiObject(rect);
        var obj = target.GetComponent<Image>();
        obj.raycastTarget = true;
    }
    public async void SetTutorialProgress()
    {
        if (tutorialComplete)
            return;
        if (isStop)
            return;
        if (settingTutorial)
            return;
        settingTutorial = true;
        await UniTask.WaitForSeconds(0.2f);
        count++;
        if(count == (int)WorldMapTutorialProgress.Drag)
        {
            progress = WorldMapTutorialProgress.Drag;
            stopDrag = false;
        }
        if(count == (int)WorldMapTutorialProgress.SelectWorld)
        {
            manager.SetTutorialRotation();
            progress = WorldMapTutorialProgress.SelectWorld;

        }

        if(count == (int)WorldMapTutorialProgress.Drag + 1)
        {
            progress = WorldMapTutorialProgress.None;
            stopDrag = true;
        }
        if(count == (int)WorldMapTutorialProgress.SelectWorld + 1)
        {
            progress = WorldMapTutorialProgress.None;
            PlayerPrefs.SetInt("WorldTutorialCheck", 1);
            tutorialComplete = true;
        }
        SetTutorial(count);
    }

    public void SkipTutorial()
    {
        isStop = true;
        SetTargetRayTrue();
        var index = (int)tutorialTextFormations[count];
        if (index > -1)
        {
            SetTextFormation(tutorialTextFormations[count], StringTextFormatKr.TutorialSkip);
            tutorialSkipButtons[index].gameObject.SetActive(true);
        }
    }

    public void SkipConfirm()
    {
        stopDrag = false;
        cursor.gameObject.SetActive(false);
        tutorialComplete = true;
        target.gameObject.SetActive(false);
        PlayerPrefs.SetInt("WorldTutorialCheck", 1);
        isStop = false;
        gameObject.SetActive(false);
    }

    public void SkipCancle()
    {
        isStop = false;
        SetTargetRayCast(count);
        SetTutorial(count);
    }
}
