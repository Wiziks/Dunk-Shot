using UnityEngine;
using UnityEngine.UI;

public class NightModeSettings : MonoBehaviour {
    [Header("Mode Colors")]
    [SerializeField] private Color _normalModeColor;
    [SerializeField] private Color _nightModeColor;

    [Header("Sensitive To Night Mode")]
    [SerializeField] private SpriteRenderer _background;
    [SerializeField] private Image _inGamePause;
    [SerializeField] private Image _settings;
    [SerializeField] private Image _shop;

    [Header("Button Visual")]
    [SerializeField] private Image _lampTurnOn;
    [SerializeField] private Image _lampTurnOff;
    [SerializeField] private Slider _nightModeSlider;

    private bool _isNightMode;

    private void Start() {
        if (PlayerPrefs.HasKey(GameManager.Instance.NightModeSave)) {
            if (PlayerPrefs.GetInt(GameManager.Instance.NightModeSave) == 0) {
                _nightModeSlider.SwitchOff();
                _isNightMode = false;
            } else {
                _nightModeSlider.SwitchOn();
                _isNightMode = true;
            }
        } else {
            _isNightMode = false;
        }

        SwitchNightMode();
    }

    public void SwitchNightMode() {
        _isNightMode = !_isNightMode;

        Color currentColor = _isNightMode ? _nightModeColor : _normalModeColor;
        _background.color = currentColor;
        _inGamePause.color = currentColor;
        _settings.color = currentColor;
        _shop.color = currentColor;

        _lampTurnOn.enabled = _isNightMode;
        _lampTurnOff.enabled = !_isNightMode;

        PlayerPrefs.SetInt(GameManager.Instance.NightModeSave, _isNightMode ? 0 : 1);
        PlayerPrefs.Save();

        if (_isNightMode) {
            _nightModeSlider.SwitchOn();
            AudioManager.Instance.PlayNightModeOn();
        } else {
            _nightModeSlider.SwitchOff();
            AudioManager.Instance.PlayNightModeOff();
        }
    }
}
