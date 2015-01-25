using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class EndPanel : Panel {
	
	public GameObject winLabel;
	public GameObject lostLabel;
	public Text earnedText;
	
	public void Show(bool fade, bool win, int earned)
	{
		winLabel.SetActive (win);
		lostLabel.SetActive (!win);
		earnedText.text = earned.ToString () + "M $";
		base.Show(fade);
	}

	public void Hide()
	{
		base.Hide (false);
		Boardmanager.instance.StartGame ();
	}
}
