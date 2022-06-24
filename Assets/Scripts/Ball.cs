using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    public static Ball Instance;
    [SerializeField] private float _power = 0.05f;
    [SerializeField] private float _minSpeedMagnitude = 200f;
    [SerializeField] private float _maxSpeedMagnitude = 450f;
    [SerializeField] private TrajectoryRenderer _trajectoryRenderer;
    [SerializeField] private GameObject _inGamePanel;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private TextAnnouncers _textAnnouncers;
    [SerializeField] private float _timeToRespawn = 10f;
    [SerializeField] private GameObject _stuckButton;
    [SerializeField] private GameObject _smokeParticles;
    [SerializeField] private GameObject _fireParticles;
    private Rigidbody2D _ballRb;
    private Vector3 _startSpeed;
    private float _currentSpeedMagnitude;
    private int _perfectThrowsStrike = 0;
    private int _bounceStrike = 0;
    private bool _isBasketStarting = true;
    private Transform _respawnPoint;
    private float _timer;
    private bool _isThrown;

    private void Awake() {
        Instance = this;
    }

    void Start() {
        _ballRb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (transform.position.y < -5f) GameOver();

        if (_isThrown) _timer += Time.deltaTime;
        else _timer = 0f;

        if (_timer > _timeToRespawn)
            _stuckButton.SetActive(true);
    }

    public void Configure(Vector3 speed, float speedMagnitude) {
        _startSpeed = speed * _power;
        _currentSpeedMagnitude = speedMagnitude;

        if (_currentSpeedMagnitude < _minSpeedMagnitude) {
            _trajectoryRenderer.HideTrajectory();
            return;
        }

        float alpha = speedMagnitude / _maxSpeedMagnitude;
        _trajectoryRenderer.ShowTrajectory(transform.position, _startSpeed, alpha);
    }

    public void OnRing(bool firstTimeInBasket, out int throwScore) {
        _ballRb.bodyType = RigidbodyType2D.Kinematic;
        _ballRb.velocity = Vector2.zero;
        _ballRb.angularVelocity = 0;

        throwScore = 0;
        _isThrown = false;

        if (_perfectThrowsStrike == 1) _smokeParticles.SetActive(true);
        else if (_perfectThrowsStrike > 1) _fireParticles.SetActive(true);
        else {
            _smokeParticles.SetActive(false);
            _fireParticles.SetActive(false);
        }

        if (_isBasketStarting) {
            _isBasketStarting = false;
            return;
        }

        if (!firstTimeInBasket) return;

        _perfectThrowsStrike++;
        throwScore = 1 + _perfectThrowsStrike + _bounceStrike;
        _textAnnouncers.StartFlow(transform.position, _perfectThrowsStrike, _bounceStrike, throwScore);
        AudioManager.Instance.PlayScore(_perfectThrowsStrike);

        if (_perfectThrowsStrike > 0)
            GameManager.Instance.PlayVibration();

        _bounceStrike = 0;
    }

    public void ThrowBall(Basket currentBasket) {
        if (_currentSpeedMagnitude < _minSpeedMagnitude) return;

        float magnitudeSegment = (_maxSpeedMagnitude - _minSpeedMagnitude) / 3f;
        if (_currentSpeedMagnitude < _minSpeedMagnitude + magnitudeSegment) {
            if (_perfectThrowsStrike > 1) AudioManager.Instance.PlayFireReleaseLow();
            else AudioManager.Instance.PlayReleaseLow();
        } else if (_currentSpeedMagnitude > _maxSpeedMagnitude - magnitudeSegment) {
            if (_perfectThrowsStrike > 1) AudioManager.Instance.PlayFireReleaseHigh();
            else AudioManager.Instance.PlayReleaseHigh();
        } else {
            if (_perfectThrowsStrike > 1) AudioManager.Instance.PlayFireReleaseMedium();
            else AudioManager.Instance.PlayReleaseMedium();
        }

        _respawnPoint = currentBasket.BallTargetPoint;
        _isThrown = true;

        _ballRb.bodyType = RigidbodyType2D.Dynamic;
        _ballRb.AddForce(_startSpeed, ForceMode2D.Impulse);
        _ballRb.AddTorque(0.1f, ForceMode2D.Impulse);
        _trajectoryRenderer.HideTrajectory();
        currentBasket.PlayThrowEffect();
        currentBasket.BasketState = BasketState.Static;
    }

    private void GameOver() {
        _isThrown = false;
        _inGamePanel.SetActive(false);
        _losePanel.SetActive(true);
        BasketManager.Instance.GameOver();
        AudioManager.Instance.PlayGameOver();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.GetComponent<Basket>()) {
            _perfectThrowsStrike = -1;
            AudioManager.Instance.PlayBorderBall();
        } else if (other.gameObject.tag == "Bounce Bonus") {
            _bounceStrike++;
            AudioManager.Instance.PlayWallObstacle();
        }
    }

    public void RespawnBall() {
        transform.position = _respawnPoint.position;
        _isThrown = false;
    }

    public float Speed {
        get {
            float sqrMagnitude = _ballRb.velocity.sqrMagnitude;
            return sqrMagnitude * sqrMagnitude;
        }
    }

    public float MaxSpeedMagnitude { get => _maxSpeedMagnitude; }
}