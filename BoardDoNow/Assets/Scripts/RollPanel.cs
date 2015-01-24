using UnityEngine;
using System.Collections;

public class RollPanel : Panel {

    public GameObject diceGreen;
    public GameObject diceRed;

    public override void Show(bool fade)
    {
        StartCoroutine(ShowDelay(fade));
        
    }

    IEnumerator ShowDelay(bool fade)
    {
        base.Show(fade);
        yield return new WaitForSeconds(1f);
        ItemManager greenDice = diceGreen.GetComponentInChildren<ItemManager>();
        ItemManager redDice = diceRed.GetComponentInChildren<ItemManager>();

        greenDice.SetItemofType(Boardmanager.instance.greenDice);
        redDice.SetItemofType(Boardmanager.instance.redDice);
    }
}
