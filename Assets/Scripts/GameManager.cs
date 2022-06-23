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
    private string _highScoreSave { get; } = "HighScoreSave";
    private string _nightModeSave { get; } = "NightModeSave";
    private string _starCountSave { get; } = "StarCountSave";
    private int _starCount;
    public static GameManager Instance;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        _starCount = PlayerPrefs.HasKey(_starCountSave) ? PlayerPrefs.GetInt(_starCountSave) : 0;
        UpdateStarCountText();
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

    public string HighScoreSave { get => _highScoreSave; }
    public string NightModeSave { get => _nightModeSave; }

}
