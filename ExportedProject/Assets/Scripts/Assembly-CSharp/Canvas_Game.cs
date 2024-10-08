using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Game : MonoBehaviour
{
	public Text currentBookCount;

	public Slider sliderDistance;

	public GameObject interfaceMenu;

	public GameObject pauseMenu;

	public Transform soundButtonOn;

	public Transform soundButtonOff;

	public Sprite spriteOn;

	public Sprite spriteOff;

	public Slider sensitivitySlider;

	public GameObject gameoverWindow;

	public GameObject controllTouch;

	public GameObject controllSlider;

	public GameObject sprayWindow;

	public Text timerMinute;

	public Text timerSecond;

	public AudioSource randomAudioSource;

	public AudioClip[] randomAudios;

	public GameObject boostScreen;

	public Text boostText;

	private Image soundButtonImageOn;

	private Image soundButtonImageOff;

	private Text soundButtonTextOn;

	private Text soundButtonTextOff;

	private int randomAudioCount;

	[HideInInspector]
	public int timeCount;

	private bool _isSound;

	private int _bookCount;

	private bool IsSound
	{
		get
		{
			return _isSound;
		}
		set
		{
			_isSound = value;
			if (_isSound)
			{
				soundButtonImageOn.sprite = spriteOn;
				soundButtonImageOff.sprite = spriteOff;
				soundButtonTextOn.color = Color.cyan;
				soundButtonTextOff.color = Color.white;
			}
			else
			{
				soundButtonImageOn.sprite = spriteOff;
				soundButtonImageOff.sprite = spriteOn;
				soundButtonTextOn.color = Color.white;
				soundButtonTextOff.color = Color.cyan;
			}
			MultiSceneManager.This.EnableSound(_isSound);
		}
	}

	public int BookCount
	{
		get
		{
			return _bookCount;
		}
		set
		{
			_bookCount = value;
			currentBookCount.text = _bookCount.ToString();
		}
	}

	private void Start()
	{
		StartCoroutine("TimerTick");
		randomAudioCount = randomAudios.Length;
		soundButtonImageOn = soundButtonOn.GetComponent<Image>();
		soundButtonImageOff = soundButtonOff.GetComponent<Image>();
		soundButtonTextOn = soundButtonOn.GetChild(0).GetComponent<Text>();
		soundButtonTextOff = soundButtonOff.GetChild(0).GetComponent<Text>();
		IsSound = MultiSceneManager.This.GetSound();
		sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", 70f);
	}

	public void MY_UpdateDistance(float _value)
	{
		sliderDistance.value = _value;
	}

	public void MY_Lose()
	{
		gameoverWindow.SetActive(value: true);
	}

	public void MY_EnableSpray(bool isOn)
	{
		sprayWindow.SetActive(isOn);
	}

	public void MY_SaveTimer()
	{
		int @int = PlayerPrefs.GetInt("Win time", 0);
		if (@int == 0 || @int > timeCount)
		{
			PlayerPrefs.SetInt("Win time", timeCount);
		}
	}

	public void UA_SetSound(bool _isOn)
	{
		IsSound = _isOn;
	}

	public void UA_OpenPause(bool _isOn)
	{
		interfaceMenu.SetActive(!_isOn);
		pauseMenu.SetActive(_isOn);
		Time.timeScale = ((!_isOn) ? 1 : 0);
		Cursor.visible = _isOn;
		Cursor.lockState = ((!_isOn) ? CursorLockMode.Locked : CursorLockMode.None);
	}

	public void UA_ExitButton()
	{
		Time.timeScale = 1f;
		MultiSceneManager.This.LoadScene("Menu");
	}

	public void UA_SensitivityChange(float _value)
	{
		PlayerPrefs.SetFloat("Sensitivity", _value);
		Sing_Game.This.teacherContr.UpdateRotateSpeed();
	}

	public void UA_RandomSound()
	{
		randomAudioSource.clip = randomAudios[Random.Range(0, randomAudioCount)];
		randomAudioSource.Play();
	}

	private IEnumerator TimerTick()
	{
		timeCount = 0;
		while (true)
		{
			timerMinute.text = (timeCount / 60).ToString();
			timerSecond.text = (timeCount % 60).ToString();
			yield return new WaitForSeconds(1f);
			timeCount++;
		}
	}
}
