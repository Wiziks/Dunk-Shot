using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    public static Ball Instance;
    [SerializeField] private float _power = 1;
    [SerializeField] private TrajectoryRenderer _trajectoryRenderer;
    [HideInInspector] public Rigidbody2D Rigidbody;
    void Start() {
        Instance = this;
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void ShowTrajectory(Vector3 speed) {
        _trajectoryRenderer.ShowTrajectory(transform.position, speed);
    }

    public float Power { get => _power; }
}
