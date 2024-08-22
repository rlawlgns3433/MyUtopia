using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiWarningPanel : MonoBehaviour
{
    public TextMeshProUGUI textWarning;
    public WaringType waringType;

    public void SetWaring(WaringType type)
    {
        waringType = type;
        textWarning.text = WaringTexts.warnings[(int)waringType];
    }
}
