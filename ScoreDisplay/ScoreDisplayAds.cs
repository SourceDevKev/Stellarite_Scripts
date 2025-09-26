using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class ScoreDisplayAds : MonoBehaviour
{
    BannerView bannerView;

#if UNITY_ANDROID

    #if UNITY_EDITOR
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#else
        string adUnitId = "ca-app-pub-5132235039549798/4910933058";
#endif

#elif UNITY_IPHONE

#if UNITY_EDITOR
        string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        string adUnitId = "ca-app-pub-5132235039549798/2284769715";
#endif

#else
    string adUnitId = "unexpected_platform";

#endif

    public void Start()
    {
        MobileAds.Initialize(initStatus => { });

        LoadAd();
        ShowAd();
    }

    public void CreateBannerView()
    {
        Debug.Log("Creating banner view");

        if (bannerView != null)
        {
            DestroyAd();
        }

        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
    }

    public void LoadAd()
    {
        if (bannerView == null)
        {
            CreateBannerView();
        }

        AdRequest adRequest = new AdRequest();

        Debug.Log("Loading banner ad.");
        bannerView.LoadAd(adRequest);
    }

    public void ShowAd()
    {
        if (bannerView != null)
        {
            Debug.Log("Showing banner view.");
            bannerView.Show();
        }
    }

    public void HideAd()
    {
        if (bannerView != null)
        {
            Debug.Log("Hiding banner view.");
            bannerView.Hide();
        }
    }


    public void DestroyAd()
    {
        if (bannerView != null)
        {
            Debug.Log("Destroying banner ad.");
            bannerView.Destroy();
            bannerView = null;
        }
    }
}