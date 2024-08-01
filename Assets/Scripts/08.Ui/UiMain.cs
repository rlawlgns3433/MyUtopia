using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiMain : MonoBehaviour
{
    public ScrollRect scrollRect;
    public UiAnimalInventory animalInventoryUi;
    public GameObject addSlot;
    public TextMeshProUGUI currentFloorName;
    public GameObject swipeTutorial;

    public void Refresh()
    {
        addSlot.GetComponent<Transform>().SetAsLastSibling();
        var floor = FloorManager.Instance.GetCurrentFloor();
        if (floor == null)
            return;

        currentFloorName.text = floor.FloorStat.FloorData.GetFloorName();
        FloorManager.Instance.SetCurrentFloorText();
    }

    public void OffSwipeTutorial()
    {
        swipeTutorial.SetActive(false);
    }
}
