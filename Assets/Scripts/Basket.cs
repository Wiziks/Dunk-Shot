using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BasketState {
    Static,
    Dynamic
}

public class Basket : MonoBehaviour {
    [SerializeField] private Color _dynamicRingColor;
    [SerializeField] private Color _staticRingColor;
    [SerializeField] private Transform _mesh;
    [SerializeField] private float _maxScaleOfMesh = 0.5f;
    [SerializeField] private Transform _ballTargetPoint;
    private BasketState _basketState;
    private bool _canControll;
    private Camera _mainCamera;
    private float _startMeshYScale;
    private float _startBallTargetYPosition;

    private void Start() {
        _basketState = BasketState.Static;
        _mainCamera = Camera.main;
        _startMeshYScale = _mesh.localScale.y;
        _startBallTargetYPosition = _ballTargetPoint.localPosition.y;
    }

    public void Configure(float zAngle, float distance) {
        if (_basketState == BasketState.Static) return;
        transform.rotation = Quaternion.Euler(0, 0, -zAngle - 180f);
        float y = _startMeshYScale + distance / 1000f;
        if (y > _maxScaleOfMesh) y = _maxScaleOfMesh;
        _mesh.localScale = new Vector3(_mesh.localScale.x, y, _mesh.localScale.z);
        _ballTargetPoint.localPosition = new Vector3(_ballTargetPoint.localPosition.x, _startBallTargetYPosition - y + _startMeshYScale, _ballTargetPoint.localPosition.z);
        //Ball.Instance.ShowTrajectory(speed);
    }


    private void OnTriggerEnter2D(Collider2D other) {
        _basketState = BasketState.Dynamic;
        TouchArea.Instance.CurrentBasket = this;
        Ball.Instance.Rigidbody.bodyType = RigidbodyType2D.Kinematic;
        Ball.Instance.Rigidbody.velocity = Vector2.zero;
    }

    private void OnTriggerStay2D(Collider2D other) {
        other.transform.position = Vector3.MoveTowards(other.transform.position, _ballTargetPoint.position, Time.deltaTime);
    }
}
