using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiSell : MonoBehaviour
{
    public TextMeshProUGUI textCoinForSale;

    public void Sell()
    {
        var animalClick = ClickableManager.CurrentClicked as AnimalClick;
        if (animalClick == null)
            return;
        animalClick.AnimalWork.Animal.Sale();
    }
}
