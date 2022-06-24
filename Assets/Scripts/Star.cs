using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {
    [SerializeField] private float _speed;
    [SerializeField] private float _distanceToSwitchDirection;
    Vector3 _startPosition;
    private bool _up;

    private void Start() {
        _startPosition = transform.position;
    }

    void Update() {
        transform.Translate(Vector3.up * (_up ? 1f : -1f) * _speed * Time.deltaTime);

        if (Mathf.Abs(transform.position.y - _startPosition.y) > _distanceToSwitchDirection)
            _up = !_up;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        GameManager.Instance.ChangeStarCount(1);
        gameObject.SetActive(false);
        AudioManager.Instance.PlayCoin();
    }
}
