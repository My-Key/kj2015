using UnityEngine;
using System.Collections;

public class StartResumePanel : Panel 
{
    public GameObject startButton;
    public GameObject resumeButton;
    public override void Show(bool fade)
    {
        startButton.SetActive(true);
        resumeButton.SetActive(false);
        base.Show(fade);
    }

    public void Show()
    {
        startButton.SetActive(false);
        resumeButton.SetActive(true);
        base.Show(true);
    }
	
}
