using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    public void MoveAnimal(string toFloor)
    {
        var animalClick = ClickableManager.CurrentClicked as AnimalClick;

        if (animalClick == null)
            return;

        FloorManager.MoveAnimal(animalClick.AnimalWork.currentFloor, toFloor, animalClick.AnimalWork.myAnimalData);
        animalClick.gameObject.SetActive(false);
        animalClick.gameObject.transform.SetParent(FloorManager.GetFloor(toFloor).transform);
        animalClick.gameObject.transform.localPosition = Vector3.zero;
        animalClick.gameObject.SetActive(true);
    }
}
