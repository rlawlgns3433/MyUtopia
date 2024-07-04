
using System.Collections.Generic;

public static class ClickableManager
{
    private static List<IClickable> clickables = new List<IClickable>();

    public static void AddClickable(IClickable clickable)
    {
        if(clickables.Contains(clickable))
            return;

        clickables.Add(clickable);
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
