using System.Collections;
using UnityEngine;

public enum BasketState {
    Static,
    Dynamic
}

public class Basket : MonoBehaviour {
    [Header("Visual Parts")]
    [SerializeField] private Color _dynamicRingColor;
    [SerializeField] private Color _staticRingColor;
    [SerializeField] private SpriteRenderer _upperRing;
    [SerializeField] private SpriteRenderer _lowerRing;

    [Header("Mesh")]
    [SerializeField] private Transform _mesh;
    [SerializeField] private float _maxScaleOfMesh = 0.5f;
    private float _startMeshYScale;

    [Header("Visual Effects")]
    [SerializeField] private Animator _effectAnimator;
    [SerializeField] private AnimationClip _throwAnimation;
    [SerializeField] private SpriteRenderer _inRingEffect;

    [Header("Points")]
    [SerializeField] private Transform _ballTargetPoint;
    [SerializeField] private Transform _starPositionPoint;
    [SerializeField] private Transform _leftWallPositionPoint;
    [SerializeField] private Transform _rightWallPositionPoint;
    [SerializeField] private Transform _bouncerPositionPoint;

    private BasketState _basketState;
    private float _startBallTargetYPosition;
    private float _ballSpeed;
    private bool _firstTimeInBasket = true;

    private void Start() {
        _basketState = BasketState.Static;
        ChangeRingColor();

        _effectAnimator.gameObject.SetActive(false);

        _startMeshYScale = _mesh.localScale.y;
        _startBallTargetYPosition = _ballTargetPoint.localPosition.y;
        _inRingEffect.color = _staticRingColor;
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
        TouchArea.Instance.CurrentBasket = this;
        _basketState = BasketState.Dynamic;
        ChangeRingColor();

        _ballSpeed = Ball.Instance.Speed;

        Ball.Instance.OnRing(_firstTimeInBasket, out int throwScore);
        StartCoroutine(AlignBasket());

        if (_firstTimeInBasket) {
            Ball.Instance.TurnOffEffects();
            float cameraHeigthOffset = -2f - transform.position.y;
            CameraController.Instance.transform.position = new Vector3(CameraController.Instance.transform.position.x, cameraHeigthOffset, CameraController.Instance.transform.position.z);
            transform.position = new Vector3(transform.position.x, -2f, transform.position.z);
            Ball.Instance.transform.position = _ballTargetPoint.position;
            BasketManager.Instance.Spawn(throwScore);
            CameraController.Instance.TargetHeight = transform.position.y + 2f;

            StartCoroutine(InRingEffect());

            _firstTimeInBasket = false;
        }

        Ball.Instance.TurnOnEffects();
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

    IEnumerator InRingEffect() {
        _inRingEffect.enabled = true;

        Vector3 startScale = _inRingEffect.transform.localScale;
        Vector3 targetVector = new Vector3(_upperRing.transform.localScale.x, _upperRing.transform.localScale.y, 1f);
        Color targetColor = new Color(_inRingEffect.color.r, _inRingEffect.color.g, _inRingEffect.color.b, 0);

        for (float t = 0; t < 1f; t += Time.deltaTime * 3f) {
            _inRingEffect.transform.localScale = Vector3.Lerp(startScale, targetVector, t);
            _inRingEffect.color = Color.Lerp(_inRingEffect.color, targetColor, t / 3f);
            yield return null;
        }
        _inRingEffect.enabled = false;

        _inRingEffect.color = new Color(_inRingEffect.color.r, _inRingEffect.color.g, _inRingEffect.color.b, 1);
        _inRingEffect.transform.localScale = startScale;
    }

    public void PlayThrowEffect() {
        _effectAnimator.gameObject.SetActive(true);
        _effectAnimator.Play(_throwAnimation.name);
        Invoke(nameof(TurnOffAnimator), _throwAnimation.length);
    }

    private void TurnOffAnimator() {
        _effectAnimator.gameObject.SetActive(false);
    }

    public Vector3 StarPosition { get => _starPositionPoint.position; }

    public Vector3 LeftWallPosition { get => _leftWallPositionPoint.position; }

    public Vector3 RightWallPosition { get => _rightWallPositionPoint.position; }

    public Transform BallTargetPoint { get => _ballTargetPoint; }

    public Vector3 BouncerPosition { get => _bouncerPositionPoint.position; }

    public BasketState BasketState { get => _basketState; set => _basketState = value; }

    public bool FirstTimeInBasket { set => _firstTimeInBasket = value; }
}