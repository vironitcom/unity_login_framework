using UnityEngine;
using UnityEngine.UI;

public class PanelSignedController : MonoBehaviour {

    [SerializeField]
    public Button signoutButton;

	void Start () {
        signoutButton.onClick.AddListener(SignoutButtonClickHandler);
	}

    private void SignoutButtonClickHandler()
    {
        MessagingSystem.Instance.DispatchEvent(new AppEvents.LogOutEvent());
    }
}
