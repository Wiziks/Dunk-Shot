using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _finalScoreText;
    [SerializeField] private TextMeshProUGUI _bestScoreText;
    private string _highScoreSave { get; } = "HighScoreSave";
    private string _nightModeSave { get; } = "NightModeSave";
    public static GameManager Instance;

    private void Awake() {
        Instance = this;
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

    public string HighScoreSave { get => _highScoreSave; }
    public string NightModeSave { get => _nightModeSave; }

}
