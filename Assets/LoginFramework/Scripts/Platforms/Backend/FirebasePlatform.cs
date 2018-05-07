/***************************** Module Header *****************************\
Module Name:  FirebasePlatform.cs
Project:      Vironit Unity3D Login Framework
Copyright (c) VironIT, http://https://vironit.com

Template for work with Firebase Platform

The MIT License (MIT)

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

using Firebase.Auth;
using System;
using System.Threading.Tasks;

namespace VitonIT.LoginFramework
{
    public class FirebasePlatform : IBackerndPlatform
    {
        private Firebase.Auth.FirebaseAuth auth;
        private FirebaseUser user;

        //protected Firebase.Auth.FirebaseAuth auth;
        private Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;

        public void Init()
        {
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    Log("Firebase initializing...");
                    InitializeFirebase();
                }
                else
                {
                    Log("Could not resolve all Firebase dependencies: " + dependencyStatus, "ERR");
                }
            });

            //Email sign in auto when open app if remember me toggle checked
            // if (PlayerPrefs.GetInt(Utils.REMEMBER_ME) == 1 && PlayerPrefs.GetInt(Utils.LOGGED) == 1) //Keep auth
            // {
            //     SigninAsync(PlayerPrefs.GetString("Email"), PlayerPrefs.GetString("Pwd"));
            // }

            MessagingSystem.Instance.AddEventListener(typeof(LoginEvents.FacebookAuthEvent), FacebookAuthHandler);
            MessagingSystem.Instance.AddEventListener(typeof(LoginEvents.GoogleSignInAuthEvent), GoogleSignInAuthHandler);
            MessagingSystem.Instance.AddEventListener(typeof(LoginEvents.LoginViaLoginPassEvent), LoginViaLoginPassHandler);
        }

        private void LoginViaLoginPassHandler(BaseEvent message)
        {
            LoginEvents.LoginViaLoginPassEvent loginData = message as LoginEvents.LoginViaLoginPassEvent;
            Log("Check email...");
            if (string.IsNullOrEmpty(loginData.login.Trim()) || string.IsNullOrEmpty(loginData.pass.Trim()))
            {
                Log("Email or Pwd is null");
                MessagingSystem.Instance.DispatchEvent(new LoginEvents.LoginErrorEvent(LoginErrorCodes.EMAIL_PASS_IS_NULL));

                return;
            }

            MessagingSystem.Instance.DispatchEvent(new LoginEvents.LoginLoadingEvent(true));

            //Signin
            SigninAsync(loginData.login, loginData.pass);
        }

        public Task SigninAsync(string email, string pwd)
        {
            Log(String.Format("Attempting to sign in as {0}...", email));

            return auth.SignInWithEmailAndPasswordAsync(email, pwd)
            .ContinueWith(HandleEmailSigninResult);
        }

        private void HandleEmailSigninResult(Task<Firebase.Auth.FirebaseUser> authTask)
        {
            Log("HandleEmailSigninResult");

            MessagingSystem.Instance.DispatchEvent(new LoginEvents.LoginLoadingEvent(false));

            if (authTask.IsCanceled)
            {
                Log("SigninAsync was canceled.");
                MessagingSystem.Instance.DispatchEvent(new LoginEvents.LoginErrorEvent(LoginErrorCodes.SIGNINASYNC_WAS_CANCELED));
                return;
            }
            if (authTask.IsFaulted)
            {
                Log("SigninAsync encountered an error: " + authTask.Exception);
                MessagingSystem.Instance.DispatchEvent(new LoginEvents.LoginErrorEvent(LoginErrorCodes.SIGNINASYNC_ENCOUNTERED_AN_ERROR));
                return;
            }

            MessagingSystem.Instance.DispatchEvent(new LoginEvents.LoginSignInDoneEvent());
        }

        private void GoogleSignInAuthHandler(BaseEvent message)
        {
            LoginEvents.GoogleSignInAuthEvent castEvent = message as LoginEvents.GoogleSignInAuthEvent;
            Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(castEvent.idToken, null);
            auth.SignInWithCredentialAsync(credential).ContinueWith(t =>
            {
                if (t.IsCanceled)
                {
                    Log("SignInWithCredentialAsync was canceled.");
                    return;
                }
                if (t.IsFaulted)
                {
                    Log("SignInWithCredentialAsync encountered an error: " + t.Exception);
                    return;
                }

                user = auth.CurrentUser;
                MessagingSystem.Instance.DispatchEvent(new LoginEvents.GoogleSignInCredentialsLoginEvent());
            });
        }

        void OnDestroy()
        {
            auth.StateChanged -= AuthStateChanged;
            auth = null;
        }

        private void InitializeFirebase()
        {
            auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            auth.StateChanged += AuthStateChanged;
            AuthStateChanged(this, null);
        }

        // Track state changes of the auth object.
        void AuthStateChanged(object sender, System.EventArgs eventArgs)
        {
            Log("Auth State Changed");
        }

        private void FacebookAuthHandler(BaseEvent message)
        {
            LoginEvents.FacebookAuthEvent castEvent = message as LoginEvents.FacebookAuthEvent;

            Firebase.Auth.Credential credential =
            Firebase.Auth.FacebookAuthProvider.GetCredential(castEvent.accessToken);

            auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Log("SignInWithCredentialAsync was canceled.", "ERR");
                    return;
                }
                if (task.IsFaulted)
                {
                    Log("SignInWithCredentialAsync encountered an error: " + task.Exception, "ERR");
                    return;
                }

                user = auth.CurrentUser;
                MessagingSystem.Instance.DispatchEvent(new LoginEvents.FacebookCredentialsLoginEvent());
            });
        }

        private void Log(string mes, string logType = "INF")
        {
            MessagingSystem.Instance.DispatchEvent(new LoginEvents.LoginLogEvent(mes, logType));
        }

        public LoginUser GetUser()
        {
            return new LoginUser(auth.CurrentUser.DisplayName, auth.CurrentUser.PhotoUrl, auth.CurrentUser.UserId); ;
        }

        public void RegisterUser(string mail, string pass, string name)
        {
            Log("Register user: " + mail);
            CreateUserAsync(mail, pass, name);
        }

        public void CreateUserAsync(string email, string pwd, string username)
        {
            Log(String.Format("Attempting to create user {0}...", email));

            try
            {
                auth.CreateUserWithEmailAndPasswordAsync(email, pwd).ContinueWith(
                    task =>
                    {

                        Log("Firebase user created successfully");
                        if (task.IsCanceled)
                        {
                            Log("CreateUserWithEmailAndPasswordAsync was canceled.");
                            return;
                        }
                        if (task.IsFaulted)
                        {
                            Log("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                            return;
                        }

                        Log("Firebase user created successfully");

                        UpdateUserProfileAsync(username);

                        MessagingSystem.Instance.DispatchEvent(new LoginEvents.UserCreateDoneEvent());
                    });
            }
            catch (Exception e)
            {
                Log("Exception:" + e.Message);
            }
        }

        private void UpdateUserProfileAsync(string username)
        {
            Log("UpdateUserProfileAsync");
            if (auth.CurrentUser == null)
            {
                Log("Not signed in, unable to update user profile");
                return;
            }

            Log("Updating user profile");

            user = auth.CurrentUser;
            if (user != null)
            {
                Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
                {
                    DisplayName = username,
                    PhotoUrl = user.PhotoUrl,
                };

                user.UpdateUserProfileAsync(profile).ContinueWith(
                    task =>
                    {
                        if (task.IsCanceled)
                        {
                            Log("UpdateUserProfileAsync was canceled.");
                            return;
                        }
                        if (task.IsFaulted)
                        {
                            Log("UpdateUserProfileAsync encountered an error: " + task.Exception);
                            return;
                        }
                        if (task.IsCompleted)
                        {
                            Log("User profile updated completed");
                        }

                        user = auth.CurrentUser;
                    }
                    );
            }
        }

        public void SignOut()
        {
            auth.SignOut();



        }
    }
}