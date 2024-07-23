using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvitationBuilding : Building
{
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Start()
    {
        base.Start();
        clickEvent += UiManager.Instance.ShowInvitationUi;
        clickEvent += UiManager.Instance.uiInvitation.Set;
    }
}
