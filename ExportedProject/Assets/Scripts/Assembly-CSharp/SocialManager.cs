using UnityEngine;

public class SocialManager : MonoBehaviour
{
	public string url_RateUsGooglePlay;

	public string url_RateUsGooglePlay2;

	public string url_RateUsAppStore;

	public string url_RateUsAppStore2;

	public string url_MoreGamesUsGooglePlay;

	public string url_MoreGamesUsAppStore;

	public static SocialManager This { get; private set; }

	private void Awake()
	{
		This = this;
	}

	public void MY_RateUs()
	{
	}

	public void MY_RateUs2()
	{
	}

	public void MY_MoreGames()
	{
	}
}
