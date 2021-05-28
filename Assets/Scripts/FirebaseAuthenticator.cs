using Google;
using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseAuthenticator 
{
    private GoogleSignInConfiguration _configuration;

    public FirebaseAuth Auth { get; private set; }
    public  GoogleSignInUser GoogleUser { get; private set; }
    public FirebaseUser FirebaseUser { get; private set; }

    public async Task Init()
    {
        _configuration = new GoogleSignInConfiguration { WebClientId = Constants.WebClientId, RequestIdToken = true, UseGameSignIn = false };
        GoogleSignIn.Configuration = _configuration;
        Auth = FirebaseAuth.DefaultInstance;

        await SingInWithGoogle();
    }

    private async Task SingInWithGoogle()
    {
        await GoogleSignIn.DefaultInstance.SignIn().ContinueWith(task=>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Fucking google initialization: " + task.Exception);
            }
            else
            {
                Debug.LogError("Google initialized");
                GoogleUser = task.Result;
            }
               
        });

        if(GoogleUser!=null)
            await SingInWithFirebase(GoogleUser.IdToken);
    }

    private async Task SingInWithFirebase(string googleIdToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(googleIdToken, null);

        await Auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Fucking firebase initialization: " + task.Exception);
            }
            else
            {
                Debug.LogError("Firebase initialized");
                FirebaseUser = task.Result;
            }
                
        });
    }
}
