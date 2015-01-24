using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour {

    public static Dice instance;

    public int greenDice = 0;
    public int redDice = 0;
    
    // Use this for initialization
	void Start () 
    {
        instance = this;
	}
	
	public void RollDices()
    {
        greenDice = Random.Range(1, 7);
        redDice = Random.Range(1, 7);
    }





}
