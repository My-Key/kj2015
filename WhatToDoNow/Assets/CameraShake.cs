using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class CameraShake : MonoBehaviour 
{
    public static CameraShake instance;
    public List<ShakeData> shakeData;

	// Use this for initialization
	void Start ()
    {
        instance = this;
        IDOTweenInit tween = DOTween.Init(true, true, LogBehaviour.ErrorsOnly);
        
	}
	
	public void ShakeCamera(int id)
    {
        if (id > -1)
            camera.DOShakePosition(shakeData[id].duration, shakeData[id].strength, shakeData[id].vibrato, shakeData[id].randomness);
    }

    [System.SerializableAttribute]
    class ShakeData
    {
        public float duration = 2f;
        public float strength = 3f;
        public int vibrato = 10;
        public float randomness = 20f;
    }
}