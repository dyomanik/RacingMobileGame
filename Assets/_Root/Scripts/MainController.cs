using Features.AbilitySystem;
using Features.Shed;
using Game;
using Profile;
using Services.Ads.UnityAds;
using Services.Analytics;
using Services.IAP;
using Ui;
using UnityEngine;

internal class MainController : BaseController
{
    private readonly Transform _placeForUi;
    private readonly ProfilePlayer _profilePlayer;

    private MainMenuController _mainMenuController;
    private SettingsMenuController _settingsMenuController;
    private GameController _gameController;
    private BaseContext _shedContext;

    public MainController(Transform placeForUi, ProfilePlayer profilePlayer)
    {
        _placeForUi = placeForUi;
        _profilePlayer = profilePlayer;

        profilePlayer.CurrentState.SubscribeOnChange(OnChangeGameState);
        OnChangeGameState(_profilePlayer.CurrentState.Value);
    }

    private void OnChangeGameState(GameState state)
    {
        DisposeSubControllers();
        switch (state)
        {
            case GameState.Start:
                _mainMenuController = new MainMenuController(_placeForUi, _profilePlayer);
                break;
            case GameState.Game:
                _gameController = new GameController(_placeForUi, _profilePlayer);
                AnalyticsManager.Instance.SendGameStarted();
                break;
            case GameState.Shed:
                _shedContext = CreateShedContext(_placeForUi, _profilePlayer);
                break;
            case GameState.Settings:
                _settingsMenuController = new SettingsMenuController(_placeForUi, _profilePlayer);
                break;
            case GameState.RewardedAds:
                PlayAd();
                break;
            case GameState.Buying:
                Buy();
                break;
        }
    }

    private void DisposeSubControllers()
    {
        _mainMenuController?.Dispose();
        _gameController?.Dispose();
        _settingsMenuController?.Dispose();
        _shedContext?.Dispose();
    }

    private ShedContext CreateShedContext(Transform placeForUi, ProfilePlayer profilePlayer)
    {
        var context = new ShedContext(placeForUi, profilePlayer);
        AddContext(context);
        return context;
    }

    private void PlayAd() => UnityAdsService.Instance.RewardedPlayer.Play();

    private void Buy() => IAPService.Instance.Buy("product_1");

    protected override void OnDispose()
    {
        DisposeSubControllers();
        _profilePlayer.CurrentState.UnSubscribeOnChange(OnChangeGameState);
    }


}