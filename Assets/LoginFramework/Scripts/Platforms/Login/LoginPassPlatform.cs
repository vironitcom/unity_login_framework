/***************************** Module Header *****************************\
Module Name:  LoginPassPlatform.cs
Project:      Vironit Unity3D Login Framework
Copyright (c) VironIT, http://https://vironit.com

Module for handle Pass Login 

The MIT License (MIT)

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
namespace VitonIT.LoginFramework
{
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
}