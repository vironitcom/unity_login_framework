namespace VitonIT.LoginFramework
{
    public interface ILoginPlatform
    {
        void Init();
        void SignIn();
        void SignOut();
    }
}