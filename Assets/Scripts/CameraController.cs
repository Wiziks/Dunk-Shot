using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] private float _offsetSpeed;
    private float _targetHeight;
    public static CameraController Instance;

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, _targetHeight, transform.position.z), _offsetSpeed * Time.deltaTime);
    }

    public float TargetHeight { set => _targetHeight = value; }
}
