using UnityEngine;
using UnityEngine.EventSystems;

public class TouchArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {
    [Header("Panels")]
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _inGamePanel;

    public Basket CurrentBasket { private get; set; }

    private Vector2 _startPoint;

    public static TouchArea Instance;

    private void Awake() {
        if (Instance == null)
            Instance = this;
    }

    public void OnPointerDown(PointerEventData eventData) {
        Touch touch = Input.GetTouch(0);
        _startPoint = touch.position;

        _mainMenu.SetActive(false);
        _inGamePanel.SetActive(true);

        Ball.Instance.Configure(Vector3.zero, 0);
    }

    public void OnDrag(PointerEventData eventData) {
        Touch touch = Input.GetTouch(0);

        float distance = Vector2.Distance(_startPoint, touch.position);
        Vector2 tempPoint = new Vector2(_startPoint.x, touch.position.y);
        float distanceFingerToTempPoint = Vector2.Distance(touch.position, tempPoint);
        float angle = Mathf.Asin(distanceFingerToTempPoint / distance) * 180 / Mathf.PI;

        if (_startPoint.y > touch.position.y) angle = 180f - angle;
        if (_startPoint.x > touch.position.x) angle *= -1f;

        Vector2 delta;
        delta = _startPoint - touch.position;
        if (distance > Ball.Instance.MaxSpeedMagnitude) {
            delta = delta.normalized * Ball.Instance.MaxSpeedMagnitude;
            distance = Ball.Instance.MaxSpeedMagnitude;
        }

        CurrentBasket.Configure(angle, distance);
        Ball.Instance.Configure(delta, distance);
    }

    public void OnPointerUp(PointerEventData eventData) {
        if(CurrentBasket)
            if (CurrentBasket.BasketState == BasketState.Dynamic)
                Ball.Instance.ThrowBall(CurrentBasket);
    }
}