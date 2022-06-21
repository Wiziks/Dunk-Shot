using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {
    public static TouchArea Instance;
    public Basket CurrentBasket { private get; set; }
    private Vector2 _startPoint;

    private void Start() {
        Instance = this;
    }

    public void OnPointerDown(PointerEventData eventData) {
        Touch touch = Input.GetTouch(0);
        _startPoint = touch.position;
    }

    public void OnDrag(PointerEventData eventData) {
        Touch touch = Input.GetTouch(0);
        float distance = Vector2.Distance(_startPoint, touch.position);
        Vector2 tempPoint = new Vector2(_startPoint.x, touch.position.y);

        float distanceFingerToTempPoint = Vector2.Distance(touch.position, tempPoint);
        float angle = Mathf.Asin(distanceFingerToTempPoint / distance) * 180 / Mathf.PI;

        if (_startPoint.y > touch.position.y) angle = 180f - angle;
        if (_startPoint.x > touch.position.x) angle *= -1f;

        CurrentBasket.Configure(angle, distance);
        Ball.Instance.ConfigureTrajectory(-touch.position + _startPoint);
    }

    public void OnPointerUp(PointerEventData eventData) {
        CurrentBasket.SetBasketState(BasketState.Static);
        Ball.Instance.ThrowBall();
    }
}
