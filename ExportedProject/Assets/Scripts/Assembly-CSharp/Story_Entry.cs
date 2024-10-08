using UnityEngine;

public class Story_Entry : MonoBehaviour
{
	private void Start()
	{
		MultiSceneManager.This.LoadScene("Menu");
	}
}
