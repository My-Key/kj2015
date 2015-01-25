using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour {

	// Use this for initialization
    public List<GameObject> items;
	public GameObject priceTag;
	public Text price;

    public int id = 0;

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
        id = itemID;
        for (int i = 0; i < items.Count; i++)
        {
            items[i].SetActive(false);
            priceTag.SetActive(false);
        }
        if (itemID >= 0)
        {
            Item item = Boardmanager.instance.GetItemData(itemID);
            items[((int)item.itemType)].SetActive(true);
            priceTag.SetActive(true);
			price.text = item.price.ToString() + "M $";
            
			return items[((int)item.itemType)];
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
        items[itemType].SetActive(true);

		priceTag.SetActive (false);
    }


}
