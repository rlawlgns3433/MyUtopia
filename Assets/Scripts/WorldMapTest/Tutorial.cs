using Cinemachine;
using Cysharp.Threading.Tasks;
using System;
using System.Diagnostics.Contracts;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
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
    public Button purchaseButton;
    public Button missionButton;
    public Button catalogueButton;
    public Button floorInfoButton;
    public Button inventoryButton;
    public Button animalListButton;
    public bool moveFloor = false;
    public bool moveSelectFloor = false;
    public int tutorialTouchCount = 0;
    public TutorialProgress progress;
    public GameObject cursor;
    public GameObject empty;
    public int targetAnimalId;
    public bool isClosing = false;
    public bool isStop = false;
    public TestPanel testPanel;
    public GameObject focusImage;
    private async void Start()
    {
        await UniTask.WaitUntil(() => FloorManager.Instance.GetFloor("B5") != null);
        await UniTask.WaitUntil(() => FloorManager.Instance.GetFloor("B4") != null);
        count = 0;
        if (PlayerPrefs.GetInt("TutorialCheck") == 0 || !PlayerPrefs.HasKey("TutorialCheck"))
        {
            gameObject.SetActive(true);
            testPanel.ResetSaveData();
            purchaseButton.gameObject.SetActive(false);
            missionButton.gameObject.SetActive(false);
            catalogueButton.gameObject.SetActive(false);
            floorInfoButton.gameObject.SetActive(false);
            inventoryButton.gameObject.SetActive(false);
            animalListButton.gameObject.SetActive(false);
            target.gameObject.SetActive(true);
            progress = TutorialProgress.None;
            SetTutorial(count);
            FloorManager.Instance.multiTouchOff = true;
        }
        else
        {
            tutorialComplete = true;
        }
        
    }

    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("TutorialCheck") == 0 || !PlayerPrefs.HasKey("TutorialCheck"))
        {
            gameObject.SetActive(true);
            targetObjects[(int)TutorialProgress.Swipe].gameObject.SetActive(false);
            testPanel.ResetSaveData();
            purchaseButton.gameObject.SetActive(false);
            missionButton.gameObject.SetActive(false);
            catalogueButton.gameObject.SetActive(false);
            floorInfoButton.gameObject.SetActive(false);
            inventoryButton.gameObject.SetActive(false);
            animalListButton.gameObject.SetActive(false);
            target.gameObject.SetActive(true);
            progress = TutorialProgress.None;
            SetTutorial(count);
            FloorManager.Instance.multiTouchOff = true;
        }
        else
        {
            tutorialComplete = true;
        }
    }

    public async UniTask SetTutorial(int count)
    {
        if (isStop)
            return;

        if (count <= 0)
        {
            targetObjects[(int)TutorialProgress.Swipe].gameObject.SetActive(false);
            progress = TutorialProgress.None;
        }
        if (tutorialComplete)
        {
            moveFloor = false;
            moveSelectFloor = false;
            FloorManager.Instance.multiTouchOff= false;
            return;
        }
            
        if(!target.gameObject.activeSelf)
        {
            target.gameObject.SetActive(true);
        }
        if(count > -1)
        {
            if (empty != targetObjects[count])
            {
                cursor.gameObject.SetActive(true);
            }
            else
            {
                cursor.gameObject.SetActive(false);
            }
        }

        if (targetObjects[count].gameObject.layer == LayerMask.NameToLayer("UI"))
        {
            var rect = targetObjects[count].GetComponent<RectTransform>();
            if (progress == TutorialProgress.CreateItem)
            {
                var obj = targetObjects[count];
                var objRect = obj.transform.Find("Recipe(Clone)");
                await UniTask.WaitUntil(() => objRect != null);
                if (objRect != null)
                {
                    rect = objRect.GetComponent<RectTransform>();
                }
            }
            else if (progress == TutorialProgress.SellItem)
            {
                {
                    var obj = targetObjects[count];
                    var objRect = obj.transform.Find("Product");
                    if (objRect != null)
                    {
                        rect = objRect.GetComponent<RectTransform>();
                    }
                }
            }
            TargettingUiObject(rect);
        }
        else
        {
            TargettingWorldObject(targetObjects[count]);
        }
        SetTextFormation(tutorialTextFormations[count], tutorialStringFormat[count]);
        SetTargetRayCast(count);
    }

    private void SetTargetRayTrue()
    {
        var obj = target.GetComponent<Image>();
        obj.raycastTarget = true;
    }

    private void SetTargetRayCast(int count)
    {
        var obj = target.GetComponent<Image>();
        if(obj.raycastTarget != targetRayCast[count])
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
        if(progress == TutorialProgress.Confirm)
        {
            SetTextFormation(TutorialTextFormation.None, "");
        }
    }

    public void SetTutorialProgress()
    {
        if (tutorialComplete)
            return;
        if(isStop)
            return;

        count++;
        if(count == 0)
        {
            targetObjects[(int)TutorialProgress.Swipe].gameObject.SetActive(false);
            progress = TutorialProgress.None;
        }
        if(count == (int)TutorialProgress.Swipe)
        {
            progress = TutorialProgress.Swipe;
            targetObjects[(int)TutorialProgress.Swipe].gameObject.SetActive(true);
            FloorManager.Instance.multiTouchOff = false;
            focusImage.gameObject.SetActive(false);
            moveFloor = true;
        }
        if(count == (int)TutorialProgress.Move5F)
        {
            progress = TutorialProgress.Move5F;
            moveSelectFloor = true;
        }
        if (count == (int)TutorialProgress.TouchCopper)
        {
            progress = TutorialProgress.TouchCopper;
            FloorManager.Instance.touchManager.tutorialOffMultiTouch = true;
        }
        if (count == (int)TutorialProgress.Product)
        {
            progress = TutorialProgress.Product;
        }
        if (count == (int)TutorialProgress.CloseProduct)
        {
            focusImage.gameObject.SetActive(true);
            isClosing = true;
            progress = TutorialProgress.CloseProduct;
        }
        if(count == (int)TutorialProgress.Move4F)
        {
            progress = TutorialProgress.Move4F;
            moveSelectFloor = true;
        }
        if(count == (int)TutorialProgress.AddCurrenctCopper)
        {
            CurrencyManager.product[(CurrencyProductType.CopperStone)] = new BigNumber(100);
        }
        if(count == (int)TutorialProgress.MakeIngot)
        {
            progress = TutorialProgress.MakeIngot;
            FloorManager.Instance.touchManager.tutorialOffMultiTouch = true;
        }
        if(count == (int)TutorialProgress.Move3F)
        {
            progress = TutorialProgress.Move3F;
            moveSelectFloor = true;
        }
        if(count == (int)TutorialProgress.CreateItem)
        {
            CurrencyManager.product[(CurrencyProductType.CopperIngot)] = new BigNumber(5000);
            progress = TutorialProgress.CreateItem;
        }
        if(count == ( int)TutorialProgress.Accelerate)
        {
            progress = TutorialProgress.Accelerate;
        }
        if(count == (int)TutorialProgress.CloseItemUi)
        {
            progress = TutorialProgress.CloseItemUi;
        }
        if(count == (int)TutorialProgress.OpenShop)
        {
            progress = TutorialProgress.OpenShop;
        }
        if(count == (int)TutorialProgress.SellItem)
        {
            progress = TutorialProgress.SellItem;
        }
        if(count == (int)TutorialProgress.CloseShop)
        {
            progress = TutorialProgress.CloseShop;
        }
        if(count == (int)TutorialProgress.PurchaseAnimal)
        {
            progress = TutorialProgress.PurchaseAnimal;
        }
        if(count == (int)TutorialProgress.Confirm)
        {
            progress = TutorialProgress.Confirm;
        }
        if(count == (int)TutorialProgress.AnimalList)
        {
            progress = TutorialProgress.AnimalList;
        }
        if(count == (int)TutorialProgress.AnimalStat)
        {
            focusImage.gameObject.SetActive(true);
            progress = TutorialProgress.AnimalStat;
        }
        if(count == (int)TutorialProgress.CloseAnimalStat)
        {
            progress = TutorialProgress.CloseAnimalStat;
        }
        if(count == (int)TutorialProgress.MurgeAnimalPurchase)
        {
            progress = TutorialProgress.MurgeAnimalPurchase;
        }
        if(count == (int)TutorialProgress.MurgeAnimalConfirm)
        {
            progress = TutorialProgress.MurgeAnimalConfirm;
        }
        if(count == (int)TutorialProgress.ShowAnimalFocus)
        {
            progress = TutorialProgress.ShowAnimalFocus;
        }
        if(count == (int)TutorialProgress.Murge)
        {
            progress = TutorialProgress.Murge;
        }
        if(count == (int)TutorialProgress.CloseMurgeAnimalStat)
        {
            progress = TutorialProgress.CloseMurgeAnimalStat;
        }
        if(count == (int)TutorialProgress.MurgeAnimalMove5F)
        {
            progress = TutorialProgress.MurgeAnimalMove5F;
            moveSelectFloor = true;
        }
        if(count == (int)TutorialProgress.MoveAnimal)
        {
            progress = TutorialProgress.MoveAnimal;
        }
        if(count == (int)TutorialProgress.MoveMurgeAnimal)
        {
            progress = TutorialProgress.MoveMurgeAnimal;
            focusImage.gameObject.SetActive(true);
        }
        if(count == (int)TutorialProgress.CloseAnimalList)
        {
            progress = TutorialProgress.CloseAnimalList;
        }
        if(count == (int)TutorialProgress.FloorInfo)
        {
            progress = TutorialProgress.FloorInfo;
        }
        if(count == (int)TutorialProgress.BuildingLevelUp)
        {
            progress = TutorialProgress.BuildingLevelUp;
        }
        if(count == (int)TutorialProgress.CompleteLevelUp)
        {
            progress = TutorialProgress.CompleteLevelUp;
        }
        if(count == (int)TutorialProgress.Clear)
        {
            progress = TutorialProgress.Clear;
        }
        if (count == (int)TutorialProgress.OpenCraftTable)
        {
            progress = TutorialProgress.OpenCraftTable;
        }

        if (count == (int)TutorialProgress.Swipe + 1 && moveFloor)
        {
            moveFloor = false;
            targetObjects[(int)TutorialProgress.Swipe].gameObject.SetActive(false);
            FloorManager.Instance.multiTouchOff = true;
            focusImage.gameObject.SetActive(true);
            progress = TutorialProgress.None;
        }
        if(count == (int)TutorialProgress.Move5F + 1 && moveSelectFloor)
        {
            moveSelectFloor = false;
            FloorManager.Instance.multiTouchOff = true;
            progress = TutorialProgress.None;

        }
        if(count == (int)TutorialProgress.TouchCopper + 1 && tutorialTouchCount >= 10)
        {
            tutorialTouchCount = 0;
            FloorManager.Instance.touchManager.tutorialOffMultiTouch = false;
            progress = TutorialProgress.None;
            inventoryButton.gameObject.SetActive(true);
        }
        if(count == (int)TutorialProgress.CloseProduct + 1 && isClosing)
        {
            isClosing = false;
        }
        if (count == (int)TutorialProgress.Move4F+ 1 && moveSelectFloor)
        {
            moveSelectFloor = false;
            progress = TutorialProgress.None;
        }
        if(count == (int)TutorialProgress.MakeIngot +1 && tutorialTouchCount >= 10)
        {
            tutorialTouchCount = 0;
            FloorManager.Instance.touchManager.tutorialOffMultiTouch = false;
            progress = TutorialProgress.None;
        }
        if (count == (int)TutorialProgress.Move3F + 1 && moveSelectFloor)
        {
            moveSelectFloor = false;
            progress = TutorialProgress.None;
        }
        if (count == (int)TutorialProgress.CreateItem + 1)
        {
            progress = TutorialProgress.None;
        }
        if (count == (int)TutorialProgress.Product + 1)
        {
            focusImage.gameObject.SetActive(false);
            progress = TutorialProgress.None;
        }
        if (count == (int)TutorialProgress.OpenShop + 1)
        {
            focusImage.gameObject.SetActive(false);
            progress = TutorialProgress.None;
        }
        if (count == (int)TutorialProgress.SellItem + 1)
        {
            focusImage.gameObject.SetActive(true);
            progress = TutorialProgress.None;
        }
        if(count == (int)TutorialProgress.CloseItemUi + 1)
        {
            progress = TutorialProgress.None;
        }
        if (count == (int)TutorialProgress.CloseShop + 1)
        {
            progress = TutorialProgress.None;
            purchaseButton.gameObject.SetActive(true);
        }
        if(count == (int)TutorialProgress.Confirm +1)
        {
            progress = TutorialProgress.None;
            animalListButton.gameObject.SetActive(true);
        }
        if (count == (int)TutorialProgress.AnimalList + 1)
        {
            focusImage.gameObject.SetActive(false);
            progress = TutorialProgress.None;
        }
        if (count == (int)TutorialProgress.AnimalStat + 1)
        {
            progress = TutorialProgress.None;
        }
        if (count == (int)TutorialProgress.CloseAnimalStat + 1)
        {
            progress = TutorialProgress.None;
        }
        if (count == (int)TutorialProgress.MurgeAnimalConfirm + 1)
        {
            progress = TutorialProgress.None;
        }
        if (count == (int)TutorialProgress.Murge + 1)
        {
            progress = TutorialProgress.None;
        }
        if (count == (int)TutorialProgress.CloseMurgeAnimalStat + 1)
        {
            progress = TutorialProgress.None;
        }
        if (count == (int)TutorialProgress.MurgeAnimalMove5F + 1 && moveSelectFloor)
        {
            moveSelectFloor = false;
            progress = TutorialProgress.MoveAnimal;
        }
        if (count == (int)TutorialProgress.MoveAnimal + 1)
        {
            focusImage.gameObject.SetActive(false);
            progress = TutorialProgress.None;
        }
        if (count == (int)TutorialProgress.MoveMurgeAnimal + 1)
        {
            progress = TutorialProgress.None;
        }
        if (count == (int)TutorialProgress.CloseAnimalList + 1)
        {
            progress = TutorialProgress.None;
            floorInfoButton.gameObject.SetActive(true);
        }
        if (count == (int)TutorialProgress.FloorInfo + 1)
        {
            progress = TutorialProgress.None;
        }
        if (count == (int)TutorialProgress.BuildingLevelUp + 1)
        {
            progress = TutorialProgress.None;
        }
        if (count == (int)TutorialProgress.CompleteLevelUp + 1)
        {
            progress = TutorialProgress.None;
        }
        if (count == (int)TutorialProgress.OpenCraftTable + 1)
        {
            progress = TutorialProgress.None;
        }
        if (count == (int)TutorialProgress.Clear + 1)
        {
            tutorialComplete = true;
            progress = TutorialProgress.None;
            target.gameObject.SetActive(false);
            missionButton.gameObject.SetActive(true);
            catalogueButton.gameObject.SetActive(true);
            gameObject.SetActive(false);
            moveFloor = false;
            moveSelectFloor = false;
            FloorManager.Instance.multiTouchOff = false;
            PlayerPrefs.SetInt("TutorialCheck", 1);
            return;
        }
        SetTutorial(count).Forget();
    }

    public void SkipTutorial()
    {
        isStop = true;
        SetTargetRayTrue();
        var index = (int)tutorialTextFormations[count];
        if (index > -1)
        {
            SetTextFormation(tutorialTextFormations[count], ButtonText.TutorialSkip);
            tutorialSkipButtons[index].gameObject.SetActive(true);
        }
    }

    public void SkipConfirm()
    {
        tutorialSkipButtons[(int)tutorialTextFormations[count]].gameObject.SetActive(false);
        targetObjects[(int)TutorialProgress.Swipe].gameObject.SetActive(false);
        cursor.gameObject.SetActive(false);
        purchaseButton.gameObject.SetActive(true);
        missionButton.gameObject.SetActive(true);
        catalogueButton.gameObject.SetActive(true);
        floorInfoButton.gameObject.SetActive(true);
        inventoryButton.gameObject.SetActive(true);
        animalListButton.gameObject.SetActive(true);
        FloorManager.Instance.multiTouchOff = false;
        FloorManager.Instance.touchManager.tutorialOffMultiTouch = false;
        moveFloor = false;
        moveSelectFloor = false;
        tutorialComplete = true;
        target.gameObject.SetActive(false);
        PlayerPrefs.SetInt("TutorialCheck", 1);
        UiManager.Instance.ShowMainUi();
        testPanel.SetPlayingData();
        isStop = false;
        count = -1;
        tutorialTouchCount = 0;
        progress = TutorialProgress.None;
        gameObject.SetActive(false);
    }

    public void SkipCancle()
    {
        isStop = false;
        tutorialSkipButtons[(int)tutorialTextFormations[count]].gameObject.SetActive(false);
        SetTargetRayCast(count);
        SetTutorial(count).Forget();
    }
}
