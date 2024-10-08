using UnityEngine;

public class AdRepeater : MonoBehaviour
{
	public float secondsFirst;

	public float secondsRepeating;

	private void Start()
	{
	}

	private void _Repeate()
	{
		AdsController.This.MY_ShowInterstitial();
	}
}
