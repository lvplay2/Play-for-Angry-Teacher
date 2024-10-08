using UnityEngine;

public class Story_Game : MonoBehaviour
{
	public ExitWall[] exitWalls;

	public AudioSource getBookAudio;

	public GameObject roffs;

	private void Start()
	{
		MY_EnableExitWalls(_isOn: true);
		roffs.SetActive(value: true);
	}

	public void MY_EnableExitWalls(bool _isOn)
	{
		ExitWall[] array = exitWalls;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].EnableWall(_isOn);
		}
	}

	public void MY_LockExit(int index)
	{
		exitWalls[index].EnableWall(_isOn: true);
	}

	public void MY_GetBook()
	{
		getBookAudio.Play();
	}
}
