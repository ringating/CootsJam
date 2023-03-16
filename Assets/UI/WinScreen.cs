using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    public static WinScreen instance;

    public Image[] images;

    void Start()
    {
        if (instance)
            Debug.LogError("there's more than 1 instance of WinScreen!");
        
        instance = this;

        print(images[0].color.a);

        HideWinScreen(); // it's probably hidden by default in the editor, but might as well
    }

	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.P))
            print(images[0].color.a);
    }

	public static void ShowWinScreen()
    {
        foreach (Image i in instance.images)
        {
            ShowImageWhenClose.SetImageAlpha(i, 1);
        }
    }

    public static void HideWinScreen()
    {
        foreach (Image i in instance.images)
        {
            ShowImageWhenClose.SetImageAlpha(i, 0);
        }
    }
}
