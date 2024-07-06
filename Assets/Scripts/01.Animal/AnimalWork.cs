using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimalWork : MonoBehaviour
{
    public Animal animal;
    public Animal myAnimalData;
    public Slider staminaSlider;
    public string currentFloor;

    private void Awake()
    {
        myAnimalData = new Animal(animal);
    }

    private void Start()
    {
        staminaSlider.maxValue = myAnimalData.Stamina;
        staminaSlider.minValue = 0f;
        staminaSlider.value = myAnimalData.Stamina;

        staminaSlider.onValueChanged.AddListener(
        (float value) =>
        {
            myAnimalData.Stamina = (int)value;
        });

        UniConsumeStamina().Forget();
    }


    private async UniTaskVoid UniConsumeStamina()
    {
        while (myAnimalData.Stamina > 0)
        {
            myAnimalData.Stamina -= 100;
            staminaSlider.value = myAnimalData.Stamina;

            await UniTask.Delay(TimeSpan.FromSeconds(1f));
        }
    }
}