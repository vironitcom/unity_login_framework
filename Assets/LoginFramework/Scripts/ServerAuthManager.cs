using System.Collections.Generic;
using UnityEngine;

public class ServerAuthManager : SingletonAsComponent<ServerAuthManager>
{
    private Dictionary<PlatformEnums.Login, ILoginPlatform> loginDictionary = new Dictionary<PlatformEnums.Login, ILoginPlatform>();
    private Dictionary<PlatformEnums.Backend, IBackerndPlatform> backendPlatformDictionary = new Dictionary<PlatformEnums.Backend, IBackerndPlatform>();
    private IBackerndPlatform currentBackend;
    private ILoginPlatform currentLoginPlatform;

    public static ServerAuthManager Instance
    {
        get { return ((ServerAuthManager)_Instance); }
        set { _Instance = value; }
    }

    public void Init(PlatformEnums.Backend backendName)
    {
        IBackerndPlatform backerndPlatform = GetServer(backendName);
        backerndPlatform.Init();

        foreach (var tempLoginPlatform in loginDictionary)
        {
            tempLoginPlatform.Value.Init();
        }
    }

    public void AddLoginPlatform(PlatformEnums.Login loginName, ILoginPlatform loginPlatform)
    {
        loginDictionary.Add(loginName, loginPlatform);
    }

    public void AddBackerndPlatform(PlatformEnums.Backend backendName, IBackerndPlatform backerndPlatform)
    {
        backendPlatformDictionary.Add(backendName, backerndPlatform);
    }

    public void Login(PlatformEnums.Login loginType, PlatformEnums.Backend backendType)
    {
        IBackerndPlatform backerndPlatform;

        if (backendPlatformDictionary.TryGetValue(backendType, out backerndPlatform))
        {
            ILoginPlatform loginPlatform;

            if (loginDictionary.TryGetValue(loginType, out loginPlatform))
            {
                //login
                currentBackend = backerndPlatform;
                backerndPlatform.Init();
                loginPlatform.SignIn();
                currentLoginPlatform = loginPlatform;
            }
            else
            {
                Debug.LogError("Can't find login platform: " + loginType);
            }

        }
        else
        {
            Debug.LogError("Can't find backend: " + backendType);
        }
    }

    public LoginUser GetUser()
    {
        if (currentBackend != null)
        {
            return currentBackend.GetUser();
        }
        return null;
    }

    public IBackerndPlatform GetServer (PlatformEnums.Backend serverType )
    {
        IBackerndPlatform server;
        if (backendPlatformDictionary.TryGetValue(serverType, out server))
        {
            return server;
        } else
        {
            return null;
        }
    }

    public ILoginPlatform GetLogin (PlatformEnums.Login loginType)
    {
        ILoginPlatform loginPlatform;

        if (loginDictionary.TryGetValue(loginType, out loginPlatform))
        {
            return loginPlatform;
        }

        return null;
    }

    public void RegisterUser (PlatformEnums.Backend serverType, string mail, string pass, string name)
    {
        GetServer(serverType).RegisterUser(mail, pass, name);
    }

    public void LogOut ()
    {
        currentBackend.SignOut();
        currentLoginPlatform.SignOut();
    }
}
