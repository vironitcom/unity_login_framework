
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;


namespace VitonIT.LoginFramework.Demo
{
    public class LoginDemoApplication : MonoBehaviour
    {
        //Panels
        [SerializeField]
        public GameObject PanelSignIn;

        [SerializeField]
        public GameObject PanelSignUp;

        [SerializeField]
        public GameObject PanelSigned;

        [SerializeField]
        public GameObject PanelProfile;

        [SerializeField]
        public GameObject PanelSettings;

        [SerializeField]
        public GameObject PanelLoading;

        [SerializeField]
        public GameObject PanelPopup;

        [SerializeField]
        public GameObject PanelLog;

        //Signed
        public Text txtSignedUsername;
        public Image Avata;

        private void Start()
        {
            ProjectSettings.SERVER_TYPE = PlatformEnums.Backend.Playfab;

            //Login events
            MessagingSystem.Instance.AddEventListener(typeof(LoginEvents.FacebookAuthEvent), FacebookAuthHandler);
            MessagingSystem.Instance.AddEventListener(typeof(LoginEvents.FacebookCredentialsLoginEvent), FacebookCredentialsLoginHandler);

            MessagingSystem.Instance.AddEventListener(typeof(LoginEvents.GoogleSignInAuthEvent), GoogleSignInAuthHandler);
            MessagingSystem.Instance.AddEventListener(typeof(LoginEvents.GoogleSignInCredentialsLoginEvent), GoogleSignInCredentialsLoginHandler);

            MessagingSystem.Instance.AddEventListener(typeof(LoginEvents.LoginLogEvent), LoginLogHandler);
            MessagingSystem.Instance.AddEventListener(typeof(LoginEvents.LoginSignInDoneEvent), LoginSignInDoneHandler);

            MessagingSystem.Instance.AddEventListener(typeof(LoginEvents.UserCreateDoneEvent), UserCreateDoneHandler);


            //App events
            MessagingSystem.Instance.AddEventListener(typeof(AppEvents.OpenRegisterWindowEvent), OpenRegisterWindowHandler);
            MessagingSystem.Instance.AddEventListener(typeof(AppEvents.RegisterBackEvent), RegisterBackHandler);
            MessagingSystem.Instance.AddEventListener(typeof(AppEvents.LogOutEvent), LogoutHandler);

            InitLogin();
        }

        private void LogoutHandler(BaseEvent message)
        {
            PanelProfile.SetActive(true);
            PanelSettings.SetActive(false);

            PanelSigned.SetActive(false);
            PanelSignUp.SetActive(false);
            PanelSignIn.SetActive(true);

            ServerAuthManager.Instance.LogOut();
        }

        private void UserCreateDoneHandler(BaseEvent message)
        {
            PanelLoading.SetActive(false);
            PanelSignIn.SetActive(false);
            PanelSigned.SetActive(true);
        }

        private void RegisterBackHandler(BaseEvent message)
        {
            PanelSignIn.gameObject.SetActive(true);
            PanelSignUp.gameObject.SetActive(false);
        }

        private void OpenRegisterWindowHandler(BaseEvent message)
        {
            PanelSignIn.gameObject.SetActive(false);
            PanelSignUp.gameObject.SetActive(true);
        }

        private void LoginSignInDoneHandler(BaseEvent message)
        {
            PanelSignIn.SetActive(false);
            PanelSigned.SetActive(true);

            WriteLog("Email signin done.");

            LoginUser user = ServerAuthManager.Instance.GetUser();

            if (user != null)
            {
                txtSignedUsername.text = String.Format("Welcome {0}!", user.DisplayName);

                WriteLog("User:" + user.DisplayName);
                WriteLog("PhotoUrl:" + user.PhotoUrl.ToString());

                if (!string.IsNullOrEmpty(user.PhotoUrl.ToString()))
                {
                    StartCoroutine(LoadImage(user.PhotoUrl.ToString()));
                }
            }
            else
            {
                WriteLog("User is null.");
            }
        }

        private void LoginLogHandler(BaseEvent message)
        {
            System.Diagnostics.StackFrame frame = new System.Diagnostics.StackFrame(1);
            LoginEvents.LoginLogEvent log = message as LoginEvents.LoginLogEvent;
            Debug.Log("[" + frame.GetMethod().Name + "] " + log.message + "\n");
        }

        private void GoogleSignInCredentialsLoginHandler(BaseEvent message)
        {
            LoginUser user = ServerAuthManager.Instance.GetUser();
            txtSignedUsername.text = String.Format("Welcome {0}!", user.DisplayName);

            PanelSignIn.SetActive(false);
            PanelSignUp.SetActive(false);
            PanelSigned.SetActive(true);

            StartCoroutine(LoadImage(CheckImageUrl(user.PhotoUrl.AbsoluteUri)));
        }

        private void GoogleSignInAuthHandler(BaseEvent message)
        {
            PanelSignIn.SetActive(false);
            PanelSignUp.SetActive(false);
            PanelSigned.SetActive(true);
        }

        private void FacebookCredentialsLoginHandler(BaseEvent mes)
        {
            LoginUser user = ServerAuthManager.Instance.GetUser();
            WriteLog(String.Format("User signed in successfully: {0} ({1})", user.DisplayName, user.UserId));

            txtSignedUsername.text = user.DisplayName;

            //StartCoroutine(LoadImage(CheckImageUrl(user._photoUrl.AbsoluteUrlOrEmptyString())));
            StartCoroutine(LoadImage(CheckImageUrl(user._photoUrl.AbsoluteUri)));
        }

        private void FacebookAuthHandler(BaseEvent message)
        {
            PanelSignIn.SetActive(false);
            PanelSigned.SetActive(true);
        }

        private void InitLogin()
        {
            ServerAuthManager.Instance.AddBackendPlatform(PlatformEnums.Backend.Firebase, new FirebasePlatform());
            ServerAuthManager.Instance.AddBackendPlatform(PlatformEnums.Backend.Playfab, new PlayfabPlatform());
            ServerAuthManager.Instance.AddBackendPlatform(PlatformEnums.Backend.Gamespark, new GameSparkPlatform());

            ServerAuthManager.Instance.AddLoginPlatform(PlatformEnums.Login.Facebook, new FacebookPlatform());
            ServerAuthManager.Instance.AddLoginPlatform(PlatformEnums.Login.GoogleSignin, new GoogleSignInPlatform());
            ServerAuthManager.Instance.AddLoginPlatform(PlatformEnums.Login.Mail, new LoginPassPlatform());

            ServerAuthManager.Instance.Init(ProjectSettings.SERVER_TYPE);
        }

        IEnumerator LoadImage(string imageUri)
        {
            WriteLog("Loading Image");

            WWW www = new WWW(imageUri);
            yield return www;

            WriteLog("Get Image success, width = " + www.texture.width + ", height = " + www.texture.height);
            Avata.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
        }

        private string CheckImageUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                return url;
            }

            return null;
        }

        private void WriteLog(string mes, string logType = "INF")
        {
            MessagingSystem.Instance.DispatchEvent(new LoginEvents.LoginLogEvent(mes, logType));
        }

        private void OnDestroy()
        {
            //remove all events
        }
    }
}