using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextAnnouncers : MonoBehaviour {
    [SerializeField] private RectTransform _textAnnouncersRect;
    [SerializeField] private TextMeshProUGUI _perfectText;
    [SerializeField] private TextMeshProUGUI _bounceText;
    [SerializeField] private TextMeshProUGUI _throwScoreText;
    [SerializeField] private int _flowDistance;
    [SerializeField] private float _flowTime;
    [SerializeField] private float _flowTimeInterval;
    private Queue<TextMeshProUGUI> textAnnouncersQueue = new Queue<TextMeshProUGUI>();
    private Camera _mainCamera;

    private void Start() {
        _mainCamera = Camera.main;
    }

    public void StartFlow(Vector3 ballWorldCoordinates, int perfectScore, int bounceScore, int throwScore) {
        if (perfectScore > 0) {
            _perfectText.text = "PERFECT" + (perfectScore > 0 ? $" x{perfectScore}" : "");
            textAnnouncersQueue.Enqueue(_perfectText);
        }

        if (bounceScore > 0) {
            _bounceText.text = "BOUNCE" + (bounceScore > 0 ? $" x{bounceScore}" : "");
            textAnnouncersQueue.Enqueue(_bounceText);
        }

        _throwScoreText.text = $"+{throwScore}";
        textAnnouncersQueue.Enqueue(_throwScoreText);

        Vector3 ballScreenCoordinates = _mainCamera.WorldToScreenPoint(ballWorldCoordinates + Vector3.up);
        _textAnnouncersRect.rect.Set(ballScreenCoordinates.x, ballScreenCoordinates.y, _textAnnouncersRect.rect.width, _textAnnouncersRect.rect.height);

        StartCoroutine(LaunchFlow());
    }

    private IEnumerator LaunchFlow() {
        while (textAnnouncersQueue.Count > 0) {
            StartCoroutine(Flow());
            yield return new WaitForSeconds(_flowTimeInterval);
        }
    }

    private IEnumerator Flow() {
        TextMeshProUGUI currentRect = textAnnouncersQueue.Dequeue();

        Vector2 startPosition = _textAnnouncersRect.anchoredPosition;
        Vector2 finishPosition = startPosition + Vector2.up * _flowDistance;

        for (float t = 0; t < 1f; t += Time.deltaTime / _flowTime) {
            Vector2 currentPosition = Vector2.Lerp(startPosition, finishPosition, t);
            currentRect.rectTransform.rect.Set(currentPosition.x, currentPosition.y, currentRect.rectTransform.rect.width, currentRect.rectTransform.rect.height);
            yield return null;
        }

        currentRect.text = "";
    }
}
