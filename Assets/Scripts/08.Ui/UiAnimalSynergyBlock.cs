using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiAnimalSynergyBlock : MonoBehaviour
{
    private static readonly string formatName = "�ó��� �̸� : {0}";
    private static readonly string formatDesc = "�ó��� ȿ�� : {0}";
    

    [SerializeField]
    private TextMeshProUGUI textSynergyName;
    [SerializeField]
    private TextMeshProUGUI textSynergyDescription;
    public void Set(SynergyStat synergyEffect)
    {
        textSynergyName.text = string.Format(formatName, synergyEffect.SynergyData.GetName());
        textSynergyDescription.text = string.Format(formatDesc, synergyEffect.SynergyData.GetDesc());
    }
}
