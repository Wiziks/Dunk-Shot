using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextAnnouncers : MonoBehaviour {
    [Header("Text Announcers Transform")]
    [SerializeField] private RectTransform _textAnnouncersRect;

    [Header("Text Of Announcers")]
    [SerializeField] private TextMeshProUGUI _perfectText;
    [SerializeField] private TextMeshProUGUI _bounceText;
    [SerializeField] private TextMeshProUGUI _throwScoreText;

    [Header("Visual Parameters")]
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
            _perfectText.text = "PERFECT" + (perfectScore > 1 ? $" x{perfectScore}" : "");
            textAnnouncersQueue.Enqueue(_perfectText);
        }

        if (bounceScore > 0) {
            _bounceText.text = "BOUNCE" + (bounceScore > 1 ? $" x{bounceScore}" : "");
            textAnnouncersQueue.Enqueue(_bounceText);
        }

        _throwScoreText.text = $"+{throwScore}";
        textAnnouncersQueue.Enqueue(_throwScoreText);

        _textAnnouncersRect.anchoredPosition = _mainCamera.WorldToScreenPoint(ballWorldCoordinates + Vector3.up * 0.2f);

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
        currentRect.enabled = true;

        Vector2 startPosition = Vector2.zero;
        Vector2 finishPosition = startPosition + Vector2.up * _flowDistance;

        for (float t = 0; t < 1f; t += Time.deltaTime / _flowTime) {
            currentRect.rectTransform.anchoredPosition = Vector2.Lerp(startPosition, finishPosition, t);
            yield return null;
        }

        currentRect.enabled = false;
    }
}
