using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class DiscardPanel : Panel {
	
	public List<Button> cards;
	public List<GameObject> cardImages;
	public Button ok;
	private List<int> items;
	private int choosedItem = -1;
	private Person person;
	
	public void Show(bool fade, Person person, List<int> listOfItems)
	{
		choosedItem = -1;
		ok.interactable = false;
		this.person = person;
		base.Show(fade);
		cardImages.Clear ();
		
		items = listOfItems;

		int startPosX = items.Count - 1 == 3 ? -500 : items.Count - 1 == 2 ? -250 : 0;

		for (int i=0; i < items.Count - 1; i++)
		{
			cards[i].gameObject.SetActive(true);
			
			cards[i].transform.localPosition = new Vector3(startPosX, cards[i].transform.localPosition.y, cards[i].transform.localPosition.z);
			startPosX += 500;

			ItemManager itemManager = cards[i].GetComponentInChildren<ItemManager>();
			cardImages.Add(itemManager.SetItem(items[i]));
		}

		for (int i=items.Count - 1; i < 3; i++)
		{
			cards[i].gameObject.SetActive(false);
		}
	}

	public void  OnClick(int buttonId)
	{
		if (choosedItem >= 0)
			cardImages [choosedItem].GetComponent<Image> ().color = Color.white;

		choosedItem = buttonId;
		cardImages [choosedItem].GetComponent<Image> ().color = Color.gray;
		ok.interactable = true;
	}

	public void OnOk()
	{
        if (choosedItem >= 0)
            cardImages[choosedItem].GetComponent<Image>().color = Color.white;

        Boardmanager.instance.m_ListOfPlayers[0].RemoveCardID(person, items[choosedItem]);
		Hide (true);
	}
}
