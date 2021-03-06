using UnityEngine;

public class AudioManager : MonoBehaviour {
    [Header("Audio Clips")]
    [SerializeField] private AudioClip _basketSpawn;
    [SerializeField] private AudioClip _buttonClick;
    [SerializeField] private AudioClip _borderBall;
    [SerializeField] private AudioClip _coin;
    [SerializeField] private AudioClip _gameOver;
    [SerializeField] private AudioClip _releaseLow;
    [SerializeField] private AudioClip _releaseMedium;
    [SerializeField] private AudioClip _releaseHigh;
    [SerializeField] private AudioClip _releaseFireLow;
    [SerializeField] private AudioClip _releaseFireMedium;
    [SerializeField] private AudioClip _releaseFireHigh;
    [SerializeField] private AudioClip _scoreSimple;
    [SerializeField] private AudioClip[] _scorePerfect;
    [SerializeField] private AudioClip _wallObstacle;
    [SerializeField] private AudioClip _nightModeOn;
    [SerializeField] private AudioClip _nightModeOff;
    [SerializeField] private AudioClip _shopBuy;
    [SerializeField] private AudioClip _shopLocked;
    [SerializeField] private AudioClip _shopSelect;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _mainAudioSource;
    [SerializeField] private AudioSource _playerAudioSource;


    [Header("Sound Slider")]
    [SerializeField] private Slider _soundStateSlider;

    private bool _isAudioOn;
    private string _audioStateSave { get; } = "AudioStateSave";
    public static AudioManager Instance;

    private void Awake() {
        if (Instance == null)
            Instance = this;
    }

    private void Start() {
        _mainAudioSource = GetComponent<AudioSource>();

        if (PlayerPrefs.HasKey(_audioStateSave)) {
            _isAudioOn = PlayerPrefs.GetInt(_audioStateSave) == 0 ? false : true;
        } else {
            _isAudioOn = true;
            PlayerPrefs.SetInt(_audioStateSave, _isAudioOn ? 1 : 0);
            PlayerPrefs.Save();
        }

        if (_isAudioOn) _soundStateSlider.SwitchOn();
        else _soundStateSlider.SwitchOff();
    }

    public void PlayBasketSpawn() {
        if (_isAudioOn)
            _mainAudioSource.PlayOneShot(_basketSpawn);
    }

    public void PlayButtonClick() {
        if (_isAudioOn)
            _mainAudioSource.PlayOneShot(_buttonClick);
    }

    public void PlayBorderBall() {
        if (_isAudioOn)
            _mainAudioSource.PlayOneShot(_borderBall);
    }

    public void PlayCoin() {
        if (_isAudioOn)
            _mainAudioSource.PlayOneShot(_coin);
    }

    public void PlayGameOver() {
        if (_isAudioOn)
            _mainAudioSource.PlayOneShot(_gameOver);
    }

    public void PlayReleaseLow() {
        if (_isAudioOn)
            _playerAudioSource.PlayOneShot(_releaseLow);
    }

    public void PlayReleaseMedium() {
        if (_isAudioOn)
            _playerAudioSource.PlayOneShot(_releaseMedium);
    }

    public void PlayReleaseHigh() {
        if (_isAudioOn)
            _playerAudioSource.PlayOneShot(_releaseHigh);
    }

    public void PlayFireReleaseLow() {
        if (_isAudioOn)
            _playerAudioSource.PlayOneShot(_releaseFireLow);
    }

    public void PlayFireReleaseMedium() {
        if (_isAudioOn)
            _playerAudioSource.PlayOneShot(_releaseFireMedium);
    }

    public void PlayFireReleaseHigh() {
        if (_isAudioOn)
            _playerAudioSource.PlayOneShot(_releaseFireHigh);
    }

    public void PlayScore(int scorePerfect) {
        if (!_isAudioOn) return;

        if (scorePerfect == 0) {
            _mainAudioSource.PlayOneShot(_scoreSimple);
            return;
        }

        if (scorePerfect > _scorePerfect.Length) scorePerfect = _scorePerfect.Length;
        _mainAudioSource.PlayOneShot(_scorePerfect[scorePerfect - 1]);
    }

    public void PlayWallObstacle() {
        if (_isAudioOn)
            _mainAudioSource.PlayOneShot(_wallObstacle);
    }

    public void PlayNightModeOn() {
        if (_isAudioOn)
            _mainAudioSource.PlayOneShot(_nightModeOn);
    }

    public void PlayNightModeOff() {
        if (_isAudioOn)
            _mainAudioSource.PlayOneShot(_nightModeOff);
    }

    public void PlayShopBuy() {
        if (_isAudioOn)
            _mainAudioSource.PlayOneShot(_shopBuy);
    }

    public void PlayShopLocked() {
        if (_isAudioOn)
            _mainAudioSource.PlayOneShot(_shopLocked);
    }

    public void PlayShopSelect() {
        if (_isAudioOn)
            _mainAudioSource.PlayOneShot(_shopSelect);
    }

    public void ChangeAudioState() {
        _isAudioOn = !_isAudioOn;
        _soundStateSlider.SwitchState();

        PlayerPrefs.SetInt(_audioStateSave, _isAudioOn ? 1 : 0);
        PlayerPrefs.Save();
    }
}
