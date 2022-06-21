using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BasketState {
    Static,
    Dynamic
}

public class Basket : MonoBehaviour {
    [SerializeField] private Color _dynamicRingColor;
    [SerializeField] private Color _staticRingColor;
    [SerializeField] private Transform _mesh;
    [SerializeField] private float _maxScaleOfMesh = 0.5f;
    [SerializeField] private Transform _ballTargetPoint;
    [SerializeField] private SpriteRenderer _upperRing;
    [SerializeField] private SpriteRenderer _lowerRing;
    private BasketState _basketState;
    private float _startMeshYScale;
    private float _startBallTargetYPosition;
    private float _ballSpeed;

    private void Start() {
        _basketState = BasketState.Static;
        ChangeRingColor();

        _startMeshYScale = _mesh.localScale.y;
        _startBallTargetYPosition = _ballTargetPoint.localPosition.y;
    }

    public void Configure(float zAngle, float distance) {
        if (_basketState == BasketState.Static) return;

        transform.rotation = Quaternion.Euler(0, 0, -zAngle - 180f);

        float meshScaleY = _startMeshYScale + distance / 1000f;
        if (meshScaleY > _maxScaleOfMesh) meshScaleY = _maxScaleOfMesh;

        _mesh.localScale = new Vector3(_mesh.localScale.x, meshScaleY, _mesh.localScale.z);

        Vector3 ballTargetOffset = new Vector3(_ballTargetPoint.localPosition.x, _startBallTargetYPosition - meshScaleY + _startMeshYScale, 0);
        _ballTargetPoint.localPosition = ballTargetOffset;

    }

    private void OnTriggerEnter2D(Collider2D other) {
        _basketState = BasketState.Dynamic;
        ChangeRingColor();
        TouchArea.Instance.CurrentBasket = this;

        _ballSpeed = Ball.Instance.Speed;

        Ball.Instance.SetKinematic();
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (_basketState == BasketState.Static) return;

        other.transform.position = Vector3.MoveTowards(other.transform.position, _ballTargetPoint.position, _ballSpeed * Time.deltaTime);
    }

    private void OnTriggerExit2D(Collider2D other) {
        _mesh.localScale = Vector3.Lerp(_mesh.localScale, new Vector3(_mesh.localScale.x, _startMeshYScale, _mesh.localScale.z), Time.deltaTime);
    }

    private void ChangeRingColor() {
        Color ringColor = (_basketState == BasketState.Static) ? _staticRingColor : _dynamicRingColor;

        _upperRing.color = ringColor;
        _lowerRing.color = ringColor;
    }

    public void SetBasketState(BasketState basketState) => _basketState = basketState;
}