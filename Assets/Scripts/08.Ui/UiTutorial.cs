using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiTutorial : MonoBehaviour
{
    private static readonly string formatPages = "{0}/{1}";
    public List<Sprite> sprites = new List<Sprite>();
    public Image window;
    public Button buttonPrev;
    public Button buttonNext;
    public TextMeshProUGUI textPages;
    public int index = 0;

    private void OnEnable()
    {
        window.sprite = sprites[index];
        textPages.text = string.Format(formatPages, index + 1, sprites.Count);
    }

    public void OnClickPrev()
    {
        if(index <= 0)
            return;

        window.sprite = sprites[--index];
        textPages.text = string.Format(formatPages, index + 1, sprites.Count);
    }

    public void OnClickNext()
    {
        if (index >= sprites.Count - 1)
        {
            // ´Ý±â
            UiManager.Instance.ShowMainUi();
            ClearTutorial();
            index = 0;
            return;
        }

        window.sprite = sprites[++index];
        textPages.text = string.Format(formatPages, index + 1, sprites.Count);
    }

    public void ClearTutorial()
    {
        PlayerPrefs.SetInt("TutorialCheck", 1);
    }

    public void ResetTutorial()
    {
        PlayerPrefs.SetInt("TutorialCheck", 0);

        UiManager.Instance.ShowTutorialUi();
    }
}
