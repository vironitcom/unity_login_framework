/***************************** Module Header *****************************\
Module Name:  PlayfabPlatform.cs
Project:      Vironit Unity3D Login Framework
Copyright (c) VironIT, http://https://vironit.com

Template for work with Playfab Platform

The MIT License (MIT)

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using LoginResult = PlayFab.ClientModels.LoginResult;

namespace VitonIT.LoginFramework
{
    public class PlayfabPlatform : IBackerndPlatform
    {
        public LoginUser GetUser()
        {
            throw new System.NotImplementedException();
        }

        public void Init()
        {
            MessagingSystem.Instance.AddEventListener(typeof(LoginEvents.FacebookAuthEvent), FacebookAuthHandler);
            MessagingSystem.Instance.AddEventListener(typeof(LoginEvents.LoginViaLoginPassEvent), LoginViaLoginPassHandler);
        }

        private void LoginViaLoginPassHandler(BaseEvent message)
        {
            LoginEvents.LoginViaLoginPassEvent loginData = message as LoginEvents.LoginViaLoginPassEvent;

            PlayFabClientAPI.LoginWithEmailAddress(new LoginWithEmailAddressRequest()
            {
                Email = loginData.login,
                Password = loginData.pass,
            }, result =>
            {
                // success
                MessagingSystem.Instance.DispatchEvent(new LoginEvents.LoginSignInDoneEvent());
            }, error =>
            {
                // 'error' object is our point of access to error data
            });
        }

        private void FacebookAuthHandler(BaseEvent message)
        {
            LoginEvents.FacebookAuthEvent castEvent = message as LoginEvents.FacebookAuthEvent;

            PlayFabClientAPI.LoginWithFacebook(new LoginWithFacebookRequest { CreateAccount = true, AccessToken = castEvent.accessToken },
                    OnPlayfabFacebookAuthComplete,
                    OnPlayfabFacebookAuthFailed
                    );
        }

        private void OnPlayfabFacebookAuthComplete(LoginResult result)
        {
            Debug.Log("PlayFab Facebook Auth Complete. Session ticket: " + result.SessionTicket);
        }

        private void OnPlayfabFacebookAuthFailed(PlayFabError error)
        {
            Debug.Log("PlayFab Facebook Auth Failed: " + error.GenerateErrorReport());
        }

        public void RegisterUser(string mail, string pass, string name)
        {
            throw new System.NotImplementedException();
        }

        public void SignOut()
        {
            throw new System.NotImplementedException();
        }
    }
}