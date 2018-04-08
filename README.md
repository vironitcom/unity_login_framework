# Unity3D Login Framework (Firebase, Playfab, Gamespark) + (Facebook, Mail, Google Sign in, etc)

Unity3D Login Framework is a module codebase to help you organize and realise different type of login (Facebook, GoogleSignIn, Mail, Device ID, etc ) to backend services (Firebase, PlayFab, GameSpark and so on) with ready made realization some of them.

## Table Of Contents
* Сhoose backend system to your goals
* Ready made Features
* How to use 
* Known issues
* Release notes
* License

## Сhoose backend system to your goals
There are a lot of backend platforms for live games and application with rich API of necessary functionality to develop success project. Once to authorise to one of them you get access to many features. 
Some features are ready made from the box, some features can be implemented manually, such as login using a custom token ID or implementing Leaderboard using the Database and Cloud Functions

Feature | Firebase | Playfab | Gamespark
------------ | ------------- | ------------- | -------------
_Authentication_ |  |  |  
Anonymous | + | + | -
Device ID (iOS, Android) | - | + | -
Google Sign in | + | + | +
Facebook | + | + | +
Email | + |+ |+
GitHub | Token id | + | Token id
Twitter | + |Token id | Token id
Twitch |+ |Token id | -
_Features_ | | |
Cloud Functions | +(beta) | + | +
Realtime Database | + |+ |+
Crashlytics | + | - | -
Cloud Storage/ Hosting | + | - |-
Cloud Messaging |+ |+|+
Invites | + | + |+

## Ready made Features

The framework is easily extensible, in the current version it support these features, see below:
**Backend:**
Firebase
Playfab 

**Login:**
Google Sign In
Facebook
Mail login (Email/Password)

## How to use
Clone this repo, after that you should choose the backend service and login system to use. 
After that configure each one.
You will see some error, but don’t worry, after import SDKs, we will resolve all error.

### Firebase
* Click https://firebase.google.com/download/unity to download Firebase SDK for Unity. Extract and you will see all plugin of Firebase for Unity.
Select Assets -> Import Package -> Custom Package… select FirebaseAuth plugin to import to this template.
* After that or before your should register on the Firebase site. 
* Go to https://console.firebase.google.com/ and create new project.
On Welcome screen, click Add Project.
* After that configure Android and iOS apps on the site
* Don't forget to download Google-Services-Info.plist for iOS and  google-services.json to Android. 
* Go to Authentication and add all necessary type of login in your project (Facebook, Twitter...)

That's all!

### Playfab 
* For configure Playfab, please follow this instructions
https://api.playfab.com/docs/getting-started/unity-getting-started

### Facebook
Bedore configure Facebook you should configure OpenSSL and JDK

**OpenSSL**
Download: https://code.google.com/archive/p/openssl-for-windows/downloads
* Extract
* Configure on System Environment: add ssl path and JDK to Path variable

**Setup Facebook**

* Go to https://developers.facebook.com/ and click Add New App.
* Input project information and click Create App ID
* Select Facebook Login product
* After that add iOS and Android from Dashboard panel.
* Input package name and main class (this information you should be got from Unity Facebook plugin)
* (If you use Firebase, please copy OAuth redirect URI from Firebase Facebook setup and put to Valid OAuth redirect URIs -> Save.)

* Also please check that you turned on setting below
![Sign On](https://github.com/vironitcom/test/blob/master/Help/image046.png)

* Click https://developers.facebook.com/docs/unity/downloads to download Facebook SDK
Select Assets -> Import Package -> Custom Package… select facebook-unity-sdk-7.10.0 plugin to import to this template. Remember uncheck import PlaySerrviceResolver if you already have after import Firebase SDK and Google Sign in SDK or some other Google SDKs

### Init application
So, if you are there you have already import Login framework with demo and all necessary Login and Backend SDKs
Let's go ahead to setup framework.

See the LoginDemo Scene and LoginDemoApplication.cs

First, we need init framework and add all nessesary login services and one backend platform

Example:
```CSharp
private void InitLogin()
{
    ServerAuthManager.Instance.AddBackerndPlatform(PlatformEnums.Backend.Firebase, new FirebasePlatform());
    ServerAuthManager.Instance.AddBackerndPlatform(PlatformEnums.Backend.Playfab, new PlayfabPlatform());
    ServerAuthManager.Instance.AddBackerndPlatform(PlatformEnums.Backend.Gamespark, new GameSparkPlatform());

    ServerAuthManager.Instance.AddLoginPlatform(PlatformEnums.Login.Facebook, new FacebookPlatform());
    ServerAuthManager.Instance.AddLoginPlatform(PlatformEnums.Login.GoogleSignin, new GoogleSignInPlatform());
    ServerAuthManager.Instance.AddLoginPlatform(PlatformEnums.Login.Mail, new LoginPassPlatform());

    ServerAuthManager.Instance.Init(ProjectSettings.SERVER_TYPE);
}
```

After that, please add all necessary handlers 

```CSharp
//Login events
MessagingSystem.Instance.AddEventListener(typeof(LoginEvents.FacebookAuthEvent), FacebookAuthHandler);
MessagingSystem.Instance.AddEventListener(typeof(LoginEvents.FacebookCredentialsLoginEvent), FacebookCredentialsLoginHandler);
MessagingSystem.Instance.AddEventListener(typeof(LoginEvents.GoogleSignInAuthEvent), GoogleSignInAuthHandler);
MessagingSystem.Instance.AddEventListener(typeof(LoginEvents.GoogleSignInCredentialsLoginEvent), GoogleSignInCredentialsLoginHandler);
MessagingSystem.Instance.AddEventListener(typeof(LoginEvents.LoginLogEvent), LoginLogHandler);
MessagingSystem.Instance.AddEventListener(typeof(LoginEvents.LoginSignInDoneEvent), LoginSignInDoneHandler);
MessagingSystem.Instance.AddEventListener(typeof(LoginEvents.UserCreateDoneEvent), UserCreateDoneHandler);
```

And use Login with parameters
```CSharp
ServerAuthManager.Instance.Login(PlatformEnums.Login.Mail, ProjectSettings.SERVER_TYPE);
```

or 

```CSharp
ServerAuthManager.Instance.Login(PlatformEnums.Login.GoogleSignin, ProjectSettings.SERVER_TYPE);
```

and use RegisterUser and LogOut to register user via email and logout

```CSharp
//log out
ServerAuthManager.Instance.LogOut();

//register user
ServerAuthManager.Instance.RegisterUser(ProjectSettings.SERVER_TYPE, mailField.text, passField.text, nicknameField.text);
```

If you don't use some Platforms, please detete nessesary classses from Platforms folder (ex: TwitterPlatform or GamesparkPlatform)

## Known issues
* Remember login
* Gamespark realization
* Login via Devide Id
* Twitter realization
* API realization for each backend platforms

## Release notes
0.1: Login Framework with base functionality

## Licensing
```
The MIT License (MIT)

Copyright (c) 2018 VironIT  http://vironit.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```
