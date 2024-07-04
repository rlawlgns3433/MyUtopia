using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Floor : Subject, IClickable, IPointerClickHandler, IGrowable
{
    // °èÃþ 

    [SerializeField]
    private bool isClicked = false;
    public bool IsClicked
    {
        get
        {
            return isClicked;
        }

        set
        {
            isClicked = value;
            if (isClicked)
            {
                clickEvent?.Invoke();
                ClickableManager.AddClickable(this);
            }
        }
    }

    public int CurrentLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int MaxLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public BigInteger CostForLevelUp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public event Action clickEvent;

    public bool LevelUp()
    {
        throw new NotImplementedException();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    public void RegisterClickable()
    {
        throw new NotImplementedException();
    }
}
