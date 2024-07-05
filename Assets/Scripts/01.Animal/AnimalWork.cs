using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;

public class AnimalWork : MonoBehaviour
{
    public TextMeshProUGUI textStamina;
    public Animal animal;
    public Animal myAnimalData;

    private async void Start()
    {
        myAnimalData = new Animal(animal);
        UniConsumeStamina().Forget();
    }

    private async UniTaskVoid UniConsumeStamina()
    {
        while (true)
        {
            textStamina.text = (--myAnimalData.Stamina).ToString();
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
        }
    }
}
