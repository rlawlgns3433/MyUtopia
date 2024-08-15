using Cinemachine;
using Cysharp.Threading.Tasks;
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
    private async void Start()
    {
        await UniTask.WaitUntil(() => FloorManager.Instance.GetFloor("B5") != null);
        await UniTask.WaitUntil(() => FloorManager.Instance.GetFloor("B4") != null);
        count = 0;
        if(!tutorialComplete)
        {
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
    }

    public void SetTutorial(int count)
    {
        if (tutorialComplete)
            return;
        if(count > targetObjects.Length)
        {
            gameObject.SetActive(false);
        }
        if(!target.gameObject.activeSelf)
        {
            target.gameObject.SetActive(true);
        }
        if (empty == targetObjects[count])
        {
            cursor.gameObject.SetActive(false);
        }
        else
        {
            cursor.gameObject.SetActive(true);
        }
        if (targetObjects[count].gameObject.layer == LayerMask.NameToLayer("UI"))
        {
            var rect = targetObjects[count].GetComponent<RectTransform>();
            if (progress == TutorialProgress.CreateItem)
            {
                var obj = targetObjects[count];
                var objRect = obj.transform.Find("ButtonCraft");
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
        var rect = empty.GetComponent<RectTransform>();
        TargettingUiObject(rect);
    }

    public void SetTutorialProgress()
    {
        count++;
        if(count == (int)TutorialProgress.Swipe)
        {
            targetObjects[(int)TutorialProgress.Swipe].gameObject.SetActive(true);
            progress = TutorialProgress.Swipe;
            FloorManager.Instance.multiTouchOff = false;
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
        if(count == (int)TutorialProgress.Product)
        {
            progress = TutorialProgress.Product;
        }
        if (count == (int)TutorialProgress.CloseProduct)
        {
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

        if (count == (int)TutorialProgress.Swipe + 1 && moveFloor)
        {
            moveFloor = false;
            targetObjects[(int)TutorialProgress.Swipe].gameObject.SetActive(false);
            FloorManager.Instance.multiTouchOff = true;
            progress = TutorialProgress.None;
        }
        if(count == (int)TutorialProgress.Move5F + 1 && moveSelectFloor)
        {
            moveSelectFloor = false;
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
        if (count == (int)TutorialProgress.OpenShop + 1)
        {
            progress = TutorialProgress.None;
        }
        if (count == (int)TutorialProgress.SellItem + 1)
        {
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
        if(count == (int)TutorialProgress.Clear + 1)
        {
            tutorialComplete = true;
            target.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
        SetTutorial(count);
    }

    public void SkipTutorial()
    {
        tutorialComplete = true;
        gameObject.SetActive(false);
        target.gameObject.SetActive(false);
        //저장 및 저장데이터 로드 추가
    }
}
