public class LoginPassPlatform : ILoginPlatform, ILoginPassPlatform
{
    private string login;
    private string pass;

    public void Init()
    {
        //not nessesary to do something
    }

    public void SetLoginPass(string login, string pass)
    {
        this.login = login;
        this.pass = pass;
    }

    public void SignIn()
    {
        MessagingSystem.Instance.DispatchEvent(new LoginEvents.LoginViaLoginPassEvent(login, pass));
    }

    public void SignOut()
    {
        //no reason to do thomething
    }
}