using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatronBoard : Building
{
    protected override void OnEnable()
    {
        base.OnEnable();
        clickEvent += UiManager.Instance.ShowPatronUi;
    }

    private void OnDisable()
    {
        clickEvent -= UiManager.Instance.ShowPatronUi;
    }
}
