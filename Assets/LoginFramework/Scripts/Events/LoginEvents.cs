/***************************** Module Header *****************************\
Module Name:  LoginEvents.cs
Project:      Vironit Unity3D Login Framework
Copyright (c) VironIT, http://https://vironit.com

Login Events

The MIT License (MIT)

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
namespace VitonIT.LoginFramework
{
    public class BaseEvent
    {
        public string name;

        public BaseEvent()
        {
            name = this.GetType().Name;
        }
    }

    public class LoginEvents
    {
        //Log
        public class LoginLogEvent : BaseEvent
        {
            public string message;
            public string logType;

            public LoginLogEvent(string mes, string logType = "INF")
            {
                this.message = mes;
                this.logType = logType;
            }
        }

        //Facebbok auth
        public class FacebookAuthEvent : BaseEvent
        {
            public string accessToken;

            public FacebookAuthEvent(string accessToken)
            {
                this.accessToken = accessToken;
            }
        }

        //Facebook login with credentials
        public class FacebookCredentialsLoginEvent : BaseEvent { }

        //FacebookLoginErrorEvent 
        public class FacebookLoginErrorEvent : BaseEvent
        {
            public string Error;
            public string RawResult;
            public bool Cancelled;

            public FacebookLoginErrorEvent(string error, string rawResult, bool canceled)
            {
                this.Error = error;
                this.RawResult = rawResult;
                this.Cancelled = canceled;
            }
        }

        //User Cancel Login
        public class FacebookUserCancelLoginEvent : BaseEvent { }

        //Google SignIn login with credentials
        public class GoogleSignInAuthEvent : BaseEvent
        {
            public string idToken;

            public GoogleSignInAuthEvent(string idToken)
            {
                this.idToken = idToken;
            }
        }

        //Google log in done credensial
        public class GoogleSignInCredentialsLoginEvent : BaseEvent { }

        //Login via login/pass
        public class LoginViaLoginPassEvent : BaseEvent
        {
            public string login;
            public string pass;

            public LoginViaLoginPassEvent(string login, string pass)
            {
                this.login = login;
                this.pass = pass;
            }
        }

        //Login error
        public class LoginErrorEvent : BaseEvent
        {
            public string errorCode;

            public LoginErrorEvent(string errCode)
            {
                this.errorCode = errCode;
            }
        }

        //Loading load...
        public class LoginLoadingEvent : BaseEvent
        {
            public bool isLoading;

            public LoginLoadingEvent(bool loading)
            {
                this.isLoading = loading;
            }
        }

        public class LoginSignInDoneEvent : BaseEvent { }

        //user created successfully
        public class UserCreateDoneEvent : BaseEvent { }
    }
}