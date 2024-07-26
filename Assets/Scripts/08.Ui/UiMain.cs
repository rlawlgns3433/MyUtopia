using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiMain : MonoBehaviour
{
    public ScrollRect scrollRect;
    public UiAnimalInventory animalInventoryUi;
    public GameObject addSlot;
    public TextMeshProUGUI currentFloorName;
    public void Refresh()
    {
        addSlot.GetComponent<Transform>().SetAsLastSibling();
        var floor = FloorManager.Instance.GetCurrentFloor();
        currentFloorName.text = floor.FloorStat.FloorData.GetFloorName();
    }
}
