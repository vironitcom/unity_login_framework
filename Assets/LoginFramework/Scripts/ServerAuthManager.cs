/***************************** Module Header *****************************\
Module Name:  ServerAuthManager.cs
Project:      Vironit Unity3D Login Framework
Copyright (c) VironIT, http://https://vironit.com

The main class to work with LoginFramework

The MIT License (MIT)

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace VitonIT.LoginFramework
{
    public class ServerAuthManager : SingletonAsComponent<ServerAuthManager>
    {
        private Dictionary<PlatformEnums.Login, ILoginPlatform> loginDictionary = new Dictionary<PlatformEnums.Login, ILoginPlatform>();
        private Dictionary<PlatformEnums.Backend, IBackendPlatform> backendPlatformDictionary = new Dictionary<PlatformEnums.Backend, IBackendPlatform>();
        private IBackendPlatform currentBackend;
        private ILoginPlatform currentLoginPlatform;

        public static ServerAuthManager Instance
        {
            get { return ((ServerAuthManager)_Instance); }
            set { _Instance = value; }
        }

        public void Init(PlatformEnums.Backend backendName)
        {
            IBackendPlatform backerndPlatform = GetServer(backendName);
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

        public void AddBackendPlatform(PlatformEnums.Backend backendName, IBackendPlatform backerndPlatform)
        {
            backendPlatformDictionary.Add(backendName, backerndPlatform);
        }

        public void Login(PlatformEnums.Login loginType, PlatformEnums.Backend backendType)
        {
            IBackendPlatform backerndPlatform;

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

        public IBackendPlatform GetServer(PlatformEnums.Backend serverType)
        {
            IBackendPlatform server;
            if (backendPlatformDictionary.TryGetValue(serverType, out server))
            {
                return server;
            }
            else
            {
                return null;
            }
        }

        public ILoginPlatform GetLogin(PlatformEnums.Login loginType)
        {
            ILoginPlatform loginPlatform;

            if (loginDictionary.TryGetValue(loginType, out loginPlatform))
            {
                return loginPlatform;
            }

            return null;
        }

        public void RegisterUser(PlatformEnums.Backend serverType, string mail, string pass, string name)
        {
            GetServer(serverType).RegisterUser(mail, pass, name);
        }

        public void LogOut()
        {
            currentBackend.SignOut();
            currentLoginPlatform.SignOut();
        }
    }
}