using UnityEngine;
using UnityEngine.UI;

public class UiMain : MonoBehaviour
{
    public ScrollRect scrollRect;
    public UiAnimalInventory animalInventoryUi;
    public GameObject addSlot;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            Refresh();
        }
    }
    public void Refresh()
    {
        addSlot.GetComponent<Transform>().SetAsLastSibling();
    }
}
