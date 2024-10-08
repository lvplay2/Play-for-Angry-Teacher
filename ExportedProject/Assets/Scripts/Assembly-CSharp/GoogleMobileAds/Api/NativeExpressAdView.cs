using System;
using System.Reflection;
using GoogleMobileAds.Common;

namespace GoogleMobileAds.Api
{
	public class NativeExpressAdView
	{
		private INativeExpressAdClient client;

		public event EventHandler<EventArgs> OnAdLoaded;

		public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

		public event EventHandler<EventArgs> OnAdOpening;

		public event EventHandler<EventArgs> OnAdClosed;

		public event EventHandler<EventArgs> OnAdLeavingApplication;

		public NativeExpressAdView(string adUnitId, AdSize adSize, AdPosition position)
		{
			MethodInfo method = Type.GetType("GoogleMobileAds.GoogleMobileAdsClientFactory,Assembly-CSharp").GetMethod("BuildNativeExpressAdClient", BindingFlags.Static | BindingFlags.Public);
			client = (INativeExpressAdClient)method.Invoke(null, null);
			client.CreateNativeExpressAdView(adUnitId, adSize, position);
			configureNativeExpressAdEvents();
		}

		public NativeExpressAdView(string adUnitId, AdSize adSize, int x, int y)
		{
			MethodInfo method = Type.GetType("GoogleMobileAds.GoogleMobileAdsClientFactory,Assembly-CSharp").GetMethod("BuildNativeExpressAdClient", BindingFlags.Static | BindingFlags.Public);
			client = (INativeExpressAdClient)method.Invoke(null, null);
			client.CreateNativeExpressAdView(adUnitId, adSize, x, y);
			configureNativeExpressAdEvents();
		}

		public void LoadAd(AdRequest request)
		{
			client.LoadAd(request);
		}

		public void Hide()
		{
			client.HideNativeExpressAdView();
		}

		public void Show()
		{
			client.ShowNativeExpressAdView();
		}

		public void Destroy()
		{
			client.DestroyNativeExpressAdView();
		}

		private void configureNativeExpressAdEvents()
		{
			client.OnAdLoaded += delegate(object sender, EventArgs args)
			{
				if (this.OnAdLoaded != null)
				{
					this.OnAdLoaded(this, args);
				}
			};
			client.OnAdFailedToLoad += delegate(object sender, AdFailedToLoadEventArgs args)
			{
				if (this.OnAdFailedToLoad != null)
				{
					this.OnAdFailedToLoad(this, args);
				}
			};
			client.OnAdOpening += delegate(object sender, EventArgs args)
			{
				if (this.OnAdOpening != null)
				{
					this.OnAdOpening(this, args);
				}
			};
			client.OnAdClosed += delegate(object sender, EventArgs args)
			{
				if (this.OnAdClosed != null)
				{
					this.OnAdClosed(this, args);
				}
			};
			client.OnAdLeavingApplication += delegate(object sender, EventArgs args)
			{
				if (this.OnAdLeavingApplication != null)
				{
					this.OnAdLeavingApplication(this, args);
				}
			};
		}
	}
}
