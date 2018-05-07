using UnityEngine;
using UnityEngine.UI;

namespace VitonIT.LoginFramework.Demo
{
    public class PanelSignInController : MonoBehaviour
    {

        [SerializeField]
        public Button googleSignInButton;

        [SerializeField]
        public Button facebookButton;

        [SerializeField]
        public Button loginButton;

        [SerializeField]
        public Text errorMessage;

        [SerializeField]
        public InputField inputField;

        [SerializeField]
        public InputField passField;

        [SerializeField]
        public Button registerButton;

        // Use this for initialization
        void Start()
        {

            ClearErrorMessage();

            facebookButton.onClick.AddListener(FacebookButtonClickHandler);
            googleSignInButton.onClick.AddListener(GoogleButtonClickHandler);
            loginButton.onClick.AddListener(EmailLoginButtonClickHandler);
            registerButton.onClick.AddListener(RegisterButtonClickHandler);

            MessagingSystem.Instance.AddEventListener(typeof(LoginEvents.LoginErrorEvent), LoginErrorHandler);
        }

        private void RegisterButtonClickHandler()
        {
            MessagingSystem.Instance.DispatchEvent(new AppEvents.OpenRegisterWindowEvent());
        }

        private void EmailLoginButtonClickHandler()
        {
            var mailLogin = ServerAuthManager.Instance.GetLogin(PlatformEnums.Login.Mail) as ILoginPassPlatform;
            mailLogin.SetLoginPass(inputField.text, passField.text);
            ServerAuthManager.Instance.Login(PlatformEnums.Login.Mail, ProjectSettings.SERVER_TYPE);
        }

        private void LoginErrorHandler(BaseEvent message)
        {
            LoginEvents.LoginErrorEvent errorEvent = message as LoginEvents.LoginErrorEvent;
            errorMessage.gameObject.SetActive(true);
            errorMessage.text = errorEvent.errorCode;
        }

        private void ClearErrorMessage()
        {
            errorMessage.text = "";
            errorMessage.gameObject.SetActive(false);
        }

        private void GoogleButtonClickHandler()
        {
            ServerAuthManager.Instance.Login(PlatformEnums.Login.GoogleSignin, ProjectSettings.SERVER_TYPE);
        }

        private void FacebookButtonClickHandler()
        {
            ServerAuthManager.Instance.Login(PlatformEnums.Login.Facebook, ProjectSettings.SERVER_TYPE);
        }
    }
}