using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UIElements;

public static class ClickableManager
{
    private static LinkedList<IClickable> clickables = new LinkedList<IClickable>();

    public static void AddClickable(IClickable clickable)
    {
        if(clickables.Contains(clickable))
            return;

        clickables.AddLast(clickable);
    }

    public static void AddClickable(IClickable clickable, params Action[] clickEvent)
    {
        if (clickables.Contains(clickable))
            return;

        clickables.AddLast(clickable);

        foreach(var ev in clickEvent)
        {
            clickable.clickEvent += ev;
        }
    }

    public static void RemoveClickable(IClickable clickable)
    {
        clickables.Remove(clickable);
    }

    public static void OnClicked(IClickable clickable)
    {
        foreach(var c in clickables)
        {
            if (c.Equals(clickable))
                continue;
            else
                c.IsClicked = false;
        }
    }
}
