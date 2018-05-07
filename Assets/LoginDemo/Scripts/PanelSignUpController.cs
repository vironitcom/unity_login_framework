using UnityEngine;
using UnityEngine.UI;

namespace VitonIT.LoginFramework.Demo
{
    public class PanelSignUpController : MonoBehaviour
    {

        [SerializeField]
        public InputField mailField;

        [SerializeField]
        public InputField passField;

        [SerializeField]
        public InputField nicknameField;

        [SerializeField]
        public Button backButton;

        [SerializeField]
        public Button createButton;

        // Use this for initialization
        void Start()
        {
            backButton.onClick.AddListener(BackButtonClickHandler);
            createButton.onClick.AddListener(CreateButtonClickHandler);
        }

        private void CreateButtonClickHandler()
        {
            ServerAuthManager.Instance.RegisterUser(ProjectSettings.SERVER_TYPE, mailField.text, passField.text, nicknameField.text);
        }

        private void BackButtonClickHandler()
        {
            MessagingSystem.Instance.DispatchEvent(new AppEvents.RegisterBackEvent());
        }
    }
}
