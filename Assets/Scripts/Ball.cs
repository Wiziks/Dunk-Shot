using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    public static Ball Instance;
    [SerializeField] private float _power = 1;
    [SerializeField] private TrajectoryRenderer _trajectoryRenderer;
    private Rigidbody2D _ballRb;
    private Vector3 _startSpeed;

    void Start() {
        Instance = this;
        _ballRb = GetComponent<Rigidbody2D>();
    }

    public void ConfigureTrajectory(Vector3 speed) {
        _startSpeed = speed * _power;
        _trajectoryRenderer.ShowTrajectory(transform.position, _startSpeed);
    }

    public void SetKinematic() {
        _ballRb.bodyType = RigidbodyType2D.Kinematic;
        _ballRb.velocity = Vector2.zero;
        _ballRb.angularVelocity = 0;
    }

    public void ThrowBall() {
        _ballRb.bodyType = RigidbodyType2D.Dynamic;
        _ballRb.AddForce(_startSpeed, ForceMode2D.Impulse);
        _ballRb.AddTorque(1, ForceMode2D.Impulse);
    }

    public float Speed {
        get {
            float sqrMagnitude = _ballRb.velocity.sqrMagnitude;
            return sqrMagnitude * sqrMagnitude;
        }
    }
}
