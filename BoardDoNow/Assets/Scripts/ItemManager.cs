using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour {

	// Use this for initialization
    public List<GameObject> items;

	void Awake () 
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].SetActive(false);
        }
	}

    public void SetItem(int itemID)
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].SetActive(false);
        }
        Item item = Boardmanager.instance.GetItemData(itemID);
        items[((int)item.itemType) - 1].SetActive(true);
    }

    public void SetItemofType(int itemType)
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].SetActive(false);
        }
        items[itemType - 1].SetActive(true);
    }
}
