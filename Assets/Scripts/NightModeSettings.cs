using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NightModeSettings : MonoBehaviour {
    [SerializeField] private Color _normalModeColor;
    [SerializeField] private Color _nightModeColor;
    [SerializeField] private SpriteRenderer _background;
    [SerializeField] private Image _inGamePause;
    [SerializeField] private Image _settings;
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
            _isNightMode = true;
        }
        SwitchNightMode();
    }

    public void SwitchNightMode() {
        if (_isNightMode) {
            _isNightMode = false;

            _background.color = _normalModeColor;
            _inGamePause.color = _normalModeColor;
            _settings.color = _normalModeColor;

            _lampTurnOn.enabled = false;
            _lampTurnOff.enabled = true;

            PlayerPrefs.SetInt(GameManager.Instance.NightModeSave, 1);
            PlayerPrefs.Save();
            _nightModeSlider.SwitchOff();
        } else {
            _isNightMode = true;

            _background.color = _nightModeColor;
            _inGamePause.color = _nightModeColor;
            _settings.color = _nightModeColor;

            _lampTurnOn.enabled = true;
            _lampTurnOff.enabled = false;

            PlayerPrefs.SetInt(GameManager.Instance.NightModeSave, 0);
            PlayerPrefs.Save();
            _nightModeSlider.SwitchOn();
        }
    }
}
