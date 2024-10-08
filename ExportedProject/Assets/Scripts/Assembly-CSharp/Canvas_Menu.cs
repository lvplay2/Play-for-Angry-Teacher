using UnityEngine;
using UnityEngine.UI;

public class Canvas_Menu : MonoBehaviour
{
	public GameObject go_ControlDemo_Slider;

	public GameObject go_ControlDemo_Touch;

	public Transform studentTform;

	public GameObject mainWindow;

	public GameObject winWindow;

	public GameObject settingsMenu;

	public GameObject controllsMenu;

	public GameObject tutorialWindow;

	public Transform soundButtonOn;

	public Transform soundButtonOff;

	public Sprite spriteOn;

	public Sprite spriteOff;

	public Transform difficults;

	public Slider sensitivitySlider;

	public MeshRenderer studentRenderer;

	public Material menuMaterial;

	public Material winMaterial;

	public Toggle controllSlider;

	public Toggle controllTouch;

	public Text bestMinutes;

	public Text bestSeconds;

	private Image soundButtonImageOn;

	private Image soundButtonImageOff;

	private Text soundButtonTextOn;

	private Text soundButtonTextOff;

	private Text[] difficultTexts = new Text[3];

	private bool isSettings;

	private bool isWin;

	private bool isControlls;

	private Vector3 studentPos = new Vector3(-2.7f, 0f, -31.75f);

	private Vector3 studentPosWin = new Vector3(0f, 0f, -31.104f);

	private bool _isSound;

	private int _difficult;

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

	private int Difficult
	{
		get
		{
			return _difficult;
		}
		set
		{
			_difficult = value;
			difficultTexts[0].color = ((_difficult == 0) ? Color.cyan : Color.white);
			difficultTexts[1].color = ((_difficult == 1) ? Color.cyan : Color.white);
			difficultTexts[2].color = ((_difficult == 2) ? Color.cyan : Color.white);
			MultiSceneManager.This.SetDifficulty(_difficult);
		}
	}

	private void Start()
	{
		soundButtonImageOn = soundButtonOn.GetComponent<Image>();
		soundButtonImageOff = soundButtonOff.GetComponent<Image>();
		soundButtonTextOn = soundButtonOn.GetChild(0).GetComponent<Text>();
		soundButtonTextOff = soundButtonOff.GetChild(0).GetComponent<Text>();
		difficultTexts[0] = difficults.GetChild(0).GetComponent<Text>();
		difficultTexts[1] = difficults.GetChild(1).GetComponent<Text>();
		difficultTexts[2] = difficults.GetChild(2).GetComponent<Text>();
		IsSound = MultiSceneManager.This.GetSound();
		Difficult = MultiSceneManager.This.GetDifficulty();
		sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", 70f);
		if (PlayerPrefs.GetInt("Game Win", 0) == 1)
		{
			PlayerPrefs.SetInt("Game Win", 0);
			isWin = true;
			winWindow.SetActive(value: true);
			mainWindow.SetActive(value: false);
			tutorialWindow.SetActive(value: false);
			studentTform.position = studentPosWin;
			studentRenderer.material = winMaterial;
		}
		if (MultiSceneManager.This.GetControllTouch())
		{
			controllTouch.isOn = true;
			go_ControlDemo_Touch.SetActive(value: true);
			go_ControlDemo_Slider.SetActive(value: false);
		}
		else
		{
			controllSlider.isOn = true;
			go_ControlDemo_Touch.SetActive(value: false);
			go_ControlDemo_Slider.SetActive(value: true);
		}
		int @int = PlayerPrefs.GetInt("Win time", 0);
		bestMinutes.text = (@int / 60).ToString();
		bestSeconds.text = (@int % 60).ToString();
	}

	public void UA_OpenShop()
	{
		Shop.This.UA_Open();
	}

	public void UA_StartGame()
	{
		MultiSceneManager.This.LoadScene("Game");
	}

	public void UA_SetSound(bool _isOn)
	{
		IsSound = _isOn;
	}

	public void UA_SetDifficult(int _d)
	{
		Difficult = _d;
	}

	public void UA_OpenSettings()
	{
		settingsMenu.SetActive(value: true);
		isSettings = true;
	}

	public void UA_OpenControlls()
	{
		mainWindow.SetActive(value: false);
		controllsMenu.SetActive(value: true);
		isControlls = true;
	}

	public void UA_ExitButton()
	{
		if (isWin)
		{
			isWin = false;
			winWindow.SetActive(value: false);
			mainWindow.SetActive(value: true);
			studentTform.position = studentPos;
			studentRenderer.material = menuMaterial;
		}
		else if (isSettings)
		{
			isSettings = false;
			settingsMenu.SetActive(value: false);
		}
		else if (isControlls)
		{
			isControlls = false;
			controllsMenu.SetActive(value: false);
			mainWindow.SetActive(value: true);
		}
	}

	public void UA_RateUs()
	{
		SocialManager.This.MY_RateUs();
	}

	public void UA_MoreGames()
	{
		SocialManager.This.MY_MoreGames();
	}

	public void UA_SensitivityChange(float _value)
	{
		PlayerPrefs.SetFloat("Sensitivity", _value);
	}

	public void UA_ChangeControllTouch(bool isTouch)
	{
		MultiSceneManager.This.SetControllTouch(isTouch);
		if (MultiSceneManager.This.GetControllTouch())
		{
			go_ControlDemo_Touch.SetActive(value: true);
			go_ControlDemo_Slider.SetActive(value: false);
		}
		else
		{
			go_ControlDemo_Touch.SetActive(value: false);
			go_ControlDemo_Slider.SetActive(value: true);
		}
	}
}
