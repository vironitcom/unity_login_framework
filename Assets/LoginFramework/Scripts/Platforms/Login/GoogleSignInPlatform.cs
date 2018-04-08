using Google;
using System.Collections.Generic;
using System.Threading.Tasks;

public class GoogleSignInPlatform : ILoginPlatform
{
    //Google auth variables
    private string webClientId = "634603205190-2kt90306p7jtsjn2a4smkn07phenvoh7.apps.googleusercontent.com";

    private GoogleSignInConfiguration configuration;

    public void Init()
    {
        //Setup for Google Sign In
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = webClientId,
            RequestIdToken = true
        };
    }

    public void SignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthenticationFinished);
    }

    public void SignOut()
    {
        GoogleSignIn.DefaultInstance.SignOut();
    }

    void OnGoogleAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<System.Exception> enumerator =
                    task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error =
                            (GoogleSignIn.SignInException)enumerator.Current;
                    WriteLog("Got Error: " + error.Status + " " + error.Message, "ERR");
                }
                else
                {
                    WriteLog("Got Unexpected Exception?!?" + task.Exception, "ERR");
                }
            }
        }
        else if (task.IsCanceled)
        {
            WriteLog("Canceled");
        }
        else
        {
            WriteLog("Google Sign-In successed");

            WriteLog("IdToken: " + task.Result.IdToken);
            WriteLog("ImageUrl: " + task.Result.ImageUrl.AbsolutePath);

            //Set imageUrl
            //imageUrl = task.Result.ImageUrl.AbsoluteUrlOrEmptyString();

            //Start Firebase Auth

            MessagingSystem.Instance.DispatchEvent(new LoginEvents.GoogleSignInAuthEvent(task.Result.IdToken));
        }
    }

    private void WriteLog(string mes, string logType = "INF")
    {
        MessagingSystem.Instance.DispatchEvent(new LoginEvents.LoginLogEvent(mes, logType));
    }
}