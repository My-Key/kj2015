using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour {

	// Use this for initialization
    public List<GameObject> items;
	public GameObject priceTag;
	public Text price;

	void Awake () 
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].SetActive(false);
        }
        priceTag.SetActive(false);
	}

    public GameObject SetItem(int itemID)
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].SetActive(false);
            priceTag.SetActive(false);
        }
        if (itemID >= 0)
        {
            Item item = Boardmanager.instance.GetItemData(itemID);
            items[((int)item.itemType) - 1].SetActive(true);
            priceTag.SetActive(true);
			price.text = item.price.ToString() + "M $";
            
			return items[((int)item.itemType) - 1];
        }
		return null;
    }

    public void SetItemofType(int itemType)
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].SetActive(false);
            priceTag.SetActive(false);
        }
        if (itemType >= 0)
        items[itemType - 1].SetActive(true);

		priceTag.SetActive (false);
    }


}
