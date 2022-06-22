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
    private Rigidbody2D _ballRb;
    private Vector3 _startSpeed;
    private float _currentSpeedMagnitude;

    private void Awake() {
        Instance = this;
    }

    void Start() {
        _ballRb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (transform.position.y < -5f) {
            _inGamePanel.SetActive(false);
            _losePanel.SetActive(true);
        }
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

    public void SetKinematic() {
        _ballRb.bodyType = RigidbodyType2D.Kinematic;
        _ballRb.velocity = Vector2.zero;
        _ballRb.angularVelocity = 0;
    }

    public void ThrowBall(Basket currentBasket) {
        if (_currentSpeedMagnitude < _minSpeedMagnitude) return;

        _ballRb.bodyType = RigidbodyType2D.Dynamic;
        _ballRb.AddForce(_startSpeed, ForceMode2D.Impulse);
        _ballRb.AddTorque(0.1f, ForceMode2D.Impulse);
        _trajectoryRenderer.HideTrajectory();
        currentBasket.BasketState = BasketState.Static;
    }

    public float Speed {
        get {
            float sqrMagnitude = _ballRb.velocity.sqrMagnitude;
            return sqrMagnitude * sqrMagnitude;
        }
    }

    public float MaxSpeedMagnitude { get => _maxSpeedMagnitude; }
}