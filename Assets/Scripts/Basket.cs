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
    private bool _firstTimeInBasket = true;

    private void Start() {
        _basketState = BasketState.Static;
        ChangeRingColor();

        _startMeshYScale = _mesh.localScale.y;
        _startBallTargetYPosition = _ballTargetPoint.localPosition.y;
    }

    public void Configure(float zAngle, float distance) {
        if (_basketState == BasketState.Static) return;

        transform.rotation = Quaternion.Euler(0, 0, -zAngle - 180f);

        float meshScaleY = _startMeshYScale + distance / Ball.Instance.MaxSpeedMagnitude;
        if (meshScaleY > _maxScaleOfMesh) meshScaleY = _maxScaleOfMesh;

        _mesh.localScale = new Vector3(_mesh.localScale.x, meshScaleY, _mesh.localScale.z);

        Vector3 ballTargetOffset = new Vector3(_ballTargetPoint.localPosition.x, _startBallTargetYPosition - meshScaleY + _startMeshYScale, 0);
        _ballTargetPoint.localPosition = ballTargetOffset;

    }

    private void OnTriggerEnter2D(Collider2D other) {
        ChangeRingColor();
        TouchArea.Instance.CurrentBasket = this;
        _basketState = BasketState.Dynamic;

        _ballSpeed = Ball.Instance.Speed;

        Ball.Instance.SetKinematic();
        StartCoroutine(AlignBasket());

        if (_firstTimeInBasket) {
            float cameraHeigthOffset = -2f - transform.position.y;
            CameraController.Instance.transform.position = new Vector3(CameraController.Instance.transform.position.x, cameraHeigthOffset, CameraController.Instance.transform.position.z);
            transform.position = new Vector3(transform.position.x, -2f, transform.position.z);
            Ball.Instance.transform.position = _ballTargetPoint.position;
            BasketManager.Instance.Spawn();
            CameraController.Instance.TargetHeight = transform.position.y + 2f;
            _firstTimeInBasket = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (_basketState == BasketState.Static) return;

        other.transform.position = Vector3.MoveTowards(other.transform.position, _ballTargetPoint.position, _ballSpeed * Time.deltaTime);
    }

    private void OnTriggerExit2D(Collider2D other) {
        StartCoroutine(AlignMesh());
    }

    public void ChangeRingColor() {
        Color ringColor = (_basketState == BasketState.Static) ? _staticRingColor : _dynamicRingColor;

        _upperRing.color = ringColor;
        _lowerRing.color = ringColor;
    }

    IEnumerator AlignBasket() {
        for (float t = 0; t < 1f; t += Time.deltaTime * 2f) {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, t);
            yield return null;
        }
        transform.rotation = Quaternion.identity;
        Ball.Instance.transform.position = _ballTargetPoint.position;
    }

    IEnumerator AlignMesh() {
        Vector3 targetScale = new Vector3(_mesh.localScale.x, _startMeshYScale, _mesh.localScale.z);
        for (float t = 0; t < 1f; t += Time.deltaTime * 2f) {
            _mesh.localScale = Vector3.Lerp(_mesh.localScale, targetScale, t);
            yield return null;
        }
        _mesh.localScale = targetScale;

        Vector3 ballTargetOffset = new Vector3(_ballTargetPoint.localPosition.x, _startBallTargetYPosition, 0);
        _ballTargetPoint.localPosition = ballTargetOffset;
    }

    public BasketState BasketState { get => _basketState; set => _basketState = value; }

    public bool FirstTimeInBasket { set => _firstTimeInBasket = value; }
}