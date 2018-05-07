namespace VitonIT.LoginFramework
{
    public interface IBackerndPlatform
    {
        LoginUser GetUser();
        void Init();
        void RegisterUser(string mail, string pass, string name);
        void SignOut();
    }
}