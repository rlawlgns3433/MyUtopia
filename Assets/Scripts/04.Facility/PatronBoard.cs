using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatronBoard : Furniture
{
    private void OnEnable()
    {
        clickEvent += UiManager.Instance.ShowPatronUi;
    }

    private void OnDisable()
    {
        clickEvent -= UiManager.Instance.ShowPatronUi;
    }
}
