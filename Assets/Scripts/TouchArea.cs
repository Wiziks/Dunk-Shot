using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
        float ab = Vector2.Distance(_startPoint, touch.position);
        Vector2 c = new Vector2(_startPoint.x, touch.position.y);
        float ac = Vector2.Distance(touch.position, c);
        float angle = Mathf.Asin(ac / ab) * 180 / Mathf.PI;
        if (_startPoint.y > touch.position.y) angle = 90f - angle + 90f;
        if (_startPoint.x > touch.position.x) angle *= -1f;
        Debug.Log(angle);
        CurrentBasket.Configure(angle, ab);
    }

    public void OnPointerUp(PointerEventData eventData) {

    }
}
