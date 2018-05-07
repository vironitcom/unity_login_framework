/***************************** Module Header *****************************\
Module Name:  FacebookPlatform.cs
Project:      Vironit Unity3D Login Framework
Copyright (c) VironIT, http://https://vironit.com

Module for handle Facebook Login API 

The MIT License (MIT)

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
using UnityEngine;
using Facebook.Unity;
using System.Collections.Generic;
using System;

namespace VitonIT.LoginFramework
{
    public class FacebookPlatform : ILoginPlatform
    {
        public void Init()
        {
            //Setup for Facebook Sign In
            if (!FB.IsInitialized)
            {
                // Initialize the Facebook SDK
                FB.Init(OnInitComplete, OnHideUnity);
            }
            else
            {
                // Already initialized, signal an app activation App Event
                FB.ActivateApp();
            }
        }

        public void SignIn()
        {
            WriteLog("Fb SignIn");
            FB.LogInWithReadPermissions(
                new List<string>() {
                "public_profile",
                "email",
                "user_friends"
                },

                OnFacebookAuthenticationFinished

            );
        }

        private void OnFacebookAuthenticationFinished(IResult result)
        {
            if (result.Error != null)
            {
                MessagingSystem.Instance.DispatchEvent(new LoginEvents.FacebookLoginErrorEvent(result.Error, result.RawResult, result.Cancelled));
                WriteLog("Facebook login error");
                return;
            }
            WriteLog("OnFacebookAuthenticationFinished: " + FB.IsLoggedIn + " " + result.Error + " " + result.RawResult);
            if (FB.IsLoggedIn)
            {
                WriteLog("FB Logged In.");
                WriteLog("IdToken: " + AccessToken.CurrentAccessToken.TokenString);
                WriteLog("ImageUrl: " + String.Format("https://graph.facebook.com/{0}/picture?type=large&width=100&height=100", AccessToken.CurrentAccessToken.UserId));

                //TODO
                //imageUrl = String.Format("https://graph.facebook.com/{0}/picture?type=large&width=100&height=100", AccessToken.CurrentAccessToken.UserId);

                //Firebase Auth
                FacebookAuth(AccessToken.CurrentAccessToken.TokenString);
            }
            else
            {
                WriteLog("User cancelled login");
                MessagingSystem.Instance.DispatchEvent(new LoginEvents.FacebookUserCancelLoginEvent());
            }
        }

        private void WriteLog(string mes, string logType = "INF")
        {
            MessagingSystem.Instance.DispatchEvent(new LoginEvents.LoginLogEvent(mes, logType));
        }

        private void OnInitComplete()
        {
            WriteLog("FB Init done,  logged in: " + FB.IsLoggedIn);

            if (FB.IsLoggedIn)
            {
                WriteLog(String.Format("FB Logged In. TokenString:" + AccessToken.CurrentAccessToken.TokenString));
                WriteLog(AccessToken.CurrentAccessToken.ToString());

                FacebookAuth(AccessToken.CurrentAccessToken.TokenString);
            }
            else
            {
                WriteLog("User not yet loged FB or loged out");
            }
        }

        private void FacebookAuth(string tokenString)
        {
            MessagingSystem.Instance.DispatchEvent(new LoginEvents.FacebookAuthEvent(tokenString));
        }

        private void OnHideUnity(bool isGameShown)
        {
            //this.Status = "Success - Check log for details";
            //this.LastResponse = string.Format("Success Response: OnHideUnity Called {0}\n", isGameShown);
            WriteLog("Is game shown: " + isGameShown);
        }

        public void SignOut()
        {
            FB.LogOut();
        }
    }
}