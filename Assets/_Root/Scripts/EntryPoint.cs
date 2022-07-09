using Profile;
using Services.Analytics;
using UnityEngine;
using Services.Ads.UnityAds;
using Services.IAP;

internal class EntryPoint : MonoBehaviour
{
    private const float SpeedCar = 15f;
    private const GameState InitialState = GameState.Start;

    [SerializeField] private Transform _placeForUi;
    [SerializeField] private AnalyticsManager _analyticsManager;
    [SerializeField] private UnityAdsService _adsService;
    [SerializeField] private IAPService _IAPService;

    private MainController _mainController;



    private void Start()
    {
        var profilePlayer = new ProfilePlayer(SpeedCar, InitialState);
        _mainController = new MainController(_placeForUi, profilePlayer,_analyticsManager, _adsService, _IAPService);
        _analyticsManager.SendMainMenuOpened();
    }

    private void OnDestroy()
    {
        _mainController.Dispose();
    }
}