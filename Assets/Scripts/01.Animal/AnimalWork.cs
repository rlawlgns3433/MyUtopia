using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class AnimalWork : MonoBehaviour
{
    private Animal animal;
    public Animal Animal
    {
        get
        {
            if(animal == null)
            {
                animal = new Animal(10105001);
            }
            return animal;
        }
        set
        {
            animal = value;
        }
    }
    public Slider staminaSlider;
    public string currentFloor;

    private void Awake()
    {
        Animal = new Animal(10105001);
    }

    private void Start()
    {
        staminaSlider.maxValue = Animal.Stamina;
        staminaSlider.minValue = 0f;
        staminaSlider.value = Animal.Stamina;

        staminaSlider.onValueChanged.AddListener(
        (float value) =>
        {
            Animal.Stamina = (int)value;
        });

        UniConsumeStamina().Forget();
    }


    private async UniTaskVoid UniConsumeStamina()
    {
        while (Animal.Stamina > 0)
        {
            Animal.Stamina -= 100;
            staminaSlider.value = Animal.Stamina;

            await UniTask.Delay(3000);
        }
    }
}