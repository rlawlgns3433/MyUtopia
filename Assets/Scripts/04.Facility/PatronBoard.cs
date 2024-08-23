using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatronBoard : Building
{
    protected override void Start()
    {
        base.Start();
        clickEvent += UiManager.Instance.ShowPatronUi;
    }
}
