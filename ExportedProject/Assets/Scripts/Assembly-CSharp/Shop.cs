using System;
using CompleteProject;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
	public delegate void EventMethodContainer();

	public Button button_WatchAds;

	public GameObject go_PanelError;

	public GameObject go_PanelRestart;

	public GameObject go_PanelShop;

	public GameObject go_PanelSuccess;

	public GameObject go_PanelPause;

	public GameObject go_ButtonRestorePurchases;

	public Text text_ErrorNumber;

	public Text text_WatchAdsAwailableCount;

	private Purchaser _purchaser;

	private float _gameTimeScale = 1f;

	private int _goods;

	public static Shop This { get; private set; }

	public int Goods
	{
		get
		{
			return _goods;
		}
		set
		{
			_goods = value;
			PlayerPrefs.SetInt("Shop : Goods", value);
			if (this.goodsChanged != null)
			{
				this.goodsChanged();
			}
		}
	}

	public int WatchAdsAwailableCount
	{
		get
		{
			return PlayerPrefs.GetInt("Shop : WatchAdsAwailableCount", 0);
		}
		set
		{
			PlayerPrefs.SetInt("Shop : WatchAdsAwailableCount", value);
		}
	}

	public bool NoAds
	{
		get
		{
			return PlayerPrefs.GetInt("Shop : NoAds", 0) == 1;
		}
		set
		{
			PlayerPrefs.SetInt("Shop : NoAds", value ? 1 : 0);
		}
	}

	public event EventMethodContainer goodsChanged;

	private void Awake()
	{
		if (This != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			This = this;
		}
	}

	private void Start()
	{
		_purchaser = base.gameObject.GetComponent<Purchaser>();
		_goods = PlayerPrefs.GetInt("Shop : Goods", 0);
		string @string = PlayerPrefs.GetString("Shop : LastDay", "");
		if (_CheckIsNewDay(@string))
		{
			WatchAdsAwailableCount = 3;
			PlayerPrefs.SetString("Shop : LastDay", DateTime.Now.ToString());
		}
		text_WatchAdsAwailableCount.text = WatchAdsAwailableCount.ToString();
		button_WatchAds.interactable = WatchAdsAwailableCount > 0;
		go_ButtonRestorePurchases.SetActive(Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.WindowsEditor);
	}

	public void MY_DisableAds()
	{
		NoAds = true;
		go_PanelRestart.SetActive(value: true);
	}

	public void MY_EnableAds()
	{
		NoAds = false;
		go_PanelRestart.SetActive(value: true);
	}

	public void MY_BuyGoods(int id)
	{
		switch (id)
		{
		case 1:
			Goods += 55;
			break;
		case 2:
			Goods += 165;
			break;
		case 3:
			Goods += 320;
			break;
		case 4:
			Goods += 633;
			break;
		case 5:
			Goods += 1333;
			break;
		case 6:
			Goods += 3780;
			break;
		case 7:
			Goods += 8330;
			break;
		default:
			MY_ShowError("S: 1001");
			return;
		}
		go_PanelSuccess.SetActive(value: true);
	}

	public void MY_RestorePurchases()
	{
		_purchaser.MY_RestorePurchases();
	}

	public void MY_ShowError(string message)
	{
		text_ErrorNumber.text = message;
		go_PanelError.SetActive(value: true);
	}

	public void MY_GamePause(bool isPause)
	{
		_gameTimeScale = (isPause ? Time.timeScale : _gameTimeScale);
		Time.timeScale = (isPause ? 0f : _gameTimeScale);
		AudioListener.pause = isPause;
		go_PanelPause.SetActive(isPause);
	}

	public void UA_Open()
	{
		MY_GamePause(isPause: true);
		go_PanelShop.SetActive(value: true);
	}

	public void UA_BuyGoods(int id)
	{
		_purchaser.MY_BuyConsumable(id);
	}

	public void UA_DisableAds()
	{
		switch (Application.platform)
		{
		case RuntimePlatform.IPhonePlayer:
			_purchaser.MY_BuyNonConsumable(0);
			break;
		case RuntimePlatform.Android:
			_purchaser.MY_BuyConsumable(0);
			break;
		case RuntimePlatform.WindowsEditor:
			_purchaser.MY_BuyNonConsumable(0);
			break;
		case RuntimePlatform.PS3:
		case RuntimePlatform.XBOX360:
			break;
		}
	}

	public void UA_GetForWatchAds()
	{
	}

	public void UA_CloseSuccess()
	{
		go_PanelSuccess.SetActive(value: false);
	}

	public void UA_Close()
	{
		go_PanelShop.SetActive(value: false);
		go_PanelError.SetActive(value: false);
		go_PanelRestart.SetActive(value: false);
		go_PanelSuccess.SetActive(value: false);
		MY_GamePause(isPause: false);
	}

	public void UA_Restart()
	{
		Application.Quit();
	}

	private void _WatchAds()
	{
		Goods += 10;
		WatchAdsAwailableCount--;
		button_WatchAds.interactable = WatchAdsAwailableCount > 0;
		text_WatchAdsAwailableCount.text = WatchAdsAwailableCount.ToString();
	}

	private bool _CheckIsNewDay(string lastTime)
	{
		if (!DateTime.TryParse(lastTime, out var result))
		{
			return true;
		}
		DateTime now = DateTime.Now;
		int year = result.Year;
		int month = result.Month;
		int day = result.Day;
		int year2 = now.Year;
		int month2 = now.Month;
		int day2 = now.Day;
		if (year2 > year)
		{
			return true;
		}
		if (month2 > month)
		{
			return true;
		}
		if (day2 > day)
		{
			return true;
		}
		return false;
	}
}
