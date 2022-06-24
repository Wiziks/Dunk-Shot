using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _finalScoreText;
    [SerializeField] private TextMeshProUGUI _bestScoreText;
    [SerializeField] private TextMeshProUGUI _startCountText;
    [SerializeField] private Slider _vibrationStateSlider;
    private string _highScoreSave { get; } = "HighScoreSave";
    private string _nightModeSave { get; } = "NightModeSave";
    private string _starCountSave { get; } = "StarCountSave";
    private bool _isVibrationOn;
    private string _vibrationStateSave { get; } = "VibrationStateSave";
    private string _choosenBallSkinSave { get; } = "ChoosenBallSkinSave";
    private int _choosenBallSkinSequenceNumber;
    private int _starCount;
    public static GameManager Instance;

    private void Awake() {
        Instance = this;
        _choosenBallSkinSequenceNumber = PlayerPrefs.HasKey(_choosenBallSkinSave) ? PlayerPrefs.GetInt(_choosenBallSkinSave) : 0;
    }

    private void Start() {
        _starCount = PlayerPrefs.HasKey(_starCountSave) ? PlayerPrefs.GetInt(_starCountSave) : 0;
        UpdateStarCountText();


        if (PlayerPrefs.HasKey(_vibrationStateSave)) {
            _isVibrationOn = PlayerPrefs.GetInt(_vibrationStateSave) == 0 ? false : true;
        } else {
            _isVibrationOn = true;
            PlayerPrefs.SetInt(_vibrationStateSave, _isVibrationOn ? 1 : 0);
            PlayerPrefs.Save();
        }

        if (_isVibrationOn) _vibrationStateSlider.SwitchOn();
        else _vibrationStateSlider.SwitchOff();
    }

    public void SetTimeScale(float value) {
        Time.timeScale = value;
    }

    public void ReloadScene() {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetHighScore(int currentScore) {
        _finalScoreText.text = currentScore + "";

        if (PlayerPrefs.HasKey(_highScoreSave)) {
            float highScore = PlayerPrefs.GetInt(_highScoreSave);
            if (currentScore <= highScore) {
                _bestScoreText.text = highScore + "";
                return;
            }
        }

        _bestScoreText.text = currentScore + "";
        PlayerPrefs.SetInt(_highScoreSave, currentScore);
        PlayerPrefs.Save();
    }

    public void SaveNightModePrefs(bool isNightMode) {
        PlayerPrefs.SetInt(_nightModeSave, isNightMode ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ChangeStarCount(int delta) {
        _starCount += delta;
        PlayerPrefs.SetInt(_starCountSave, _starCount);
        PlayerPrefs.Save();
        UpdateStarCountText();
    }

    private void UpdateStarCountText() {
        _startCountText.text = _starCount + "";
    }

    public void PlayVibration() {
        if (_isVibrationOn)
            Handheld.Vibrate();
    }

    public void ChangeVibrationState() {
        _isVibrationOn = !_isVibrationOn;
        _vibrationStateSlider.SwitchState();
        PlayerPrefs.SetInt(_vibrationStateSave, _isVibrationOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public string HighScoreSave { get => _highScoreSave; }
    public string NightModeSave { get => _nightModeSave; }
    public int ChoosenBallSkinSequenceNumber {
        get => _choosenBallSkinSequenceNumber;
        set {
            _choosenBallSkinSequenceNumber = value;
            PlayerPrefs.SetInt(_choosenBallSkinSave, _choosenBallSkinSequenceNumber);
            PlayerPrefs.Save();
        }
    }

    public int StarCount { get => _starCount; }

    [ContextMenu("Give Stars")]
    public void GiveStars() {
        ChangeStarCount(500);
    }

    [ContextMenu("Delete All")]
    public void DeleteAll() {
        PlayerPrefs.DeleteAll();
    }
}
