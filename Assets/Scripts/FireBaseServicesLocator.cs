using Firebase;
using Firebase.Analytics;
using System.Threading.Tasks;
using UnityEngine;

public class FireBaseServicesLocator
{
    private ServiceManager _serviceManager;

    public bool IsInitialized { get; private set; }

    public FirebaseAuthenticator FirebaseAuthenticator { get; private set; }
    public FireBaseDataBaseController DataBase { get; private set; }

    public FireBaseServicesLocator(ServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    public async Task InitServices()
    {
        Debug.LogError("Start initing");
        DependencyStatus status = await FirebaseApp.CheckAndFixDependenciesAsync();
        IsInitialized = status == DependencyStatus.Available;
        if (!IsInitialized)
            Debug.LogError("Fucking initialization");
#if !UNITY_EDITOR
        FirebaseAuthenticator = new FirebaseAuthenticator();
        
        await FirebaseAuthenticator.Init();        
#endif
        DataBase = new FireBaseDataBaseController(_serviceManager, this);

        await DataBase.GetStartData();
        Debug.LogError("Services inited");
    }
}
