using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiSceneManager : MonoBehaviour
{
	private IEnumerator asyncLoadingCoroutine;

	private string sound = "Sound";

	private string controllTouch = "Controll touch";

	private string difficulty = "Difficulty";

	public static MultiSceneManager This { get; private set; }

	private void Awake()
	{
		This = this;
		Object.DontDestroyOnLoad(this);
	}

	public void LoadScene(string _name)
	{
		SceneManager.LoadScene(_name);
	}

	public void LoadSceneAsync(string _name)
	{
		if (asyncLoadingCoroutine != null)
		{
			StopCoroutine(asyncLoadingCoroutine);
		}
		asyncLoadingCoroutine = LoadindSceneAsync(_name);
		StartCoroutine(asyncLoadingCoroutine);
	}

	private IEnumerator LoadindSceneAsync(string _name)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_name);
		while (!asyncLoad.isDone)
		{
			yield return null;
		}
	}

	public void EnableSound(bool _isOn)
	{
		AudioListener.pause = !_isOn;
		PlayerPrefs.SetInt(sound, AudioListener.pause ? 1 : 0);
	}

	public bool GetSound()
	{
		if (PlayerPrefs.GetInt(sound, 0) != 1)
		{
			return true;
		}
		return false;
	}

	public bool GetControllTouch()
	{
		if (PlayerPrefs.GetInt(controllTouch, 0) != 0)
		{
			return true;
		}
		return false;
	}

	public void SetControllTouch(bool isTouch)
	{
		PlayerPrefs.SetInt(controllTouch, isTouch ? 1 : 0);
	}

	public int GetDifficulty()
	{
		return PlayerPrefs.GetInt(difficulty, 1);
	}

	public void SetDifficulty(int _d)
	{
		PlayerPrefs.SetInt(difficulty, _d);
	}
}
