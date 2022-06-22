using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum SliderState {
    ON,
    OFF
}

public class Slider : MonoBehaviour {
    [SerializeField] private Color _enableColor;
    [SerializeField] private Color _disableColor;
    [SerializeField] private Image _onText;
    [SerializeField] private Image _offText;
    [SerializeField] private Image _switcher;
    [SerializeField] private Vector3 _onSwitcherPosition;
    [SerializeField] private Vector3 _offSwitcherPosition;
    [SerializeField] private float _timeToSwitchState;
    float _switcherHeight;
    private SliderState _sliderState;
    private Image _imageComponent;

    private void Start() {
        _imageComponent = GetComponent<Image>();
        _switcherHeight = _switcher.rectTransform.rect.height;
        SwitchOn();
    }

    [ContextMenu("Switch State")]
    public void SwitchState() {
        if (_sliderState == SliderState.ON)
            SwitchOff();
        else
            SwitchOn();
    }

    private void SwitchOn() {
        _sliderState = SliderState.ON;

        _offText.enabled = false;
        _onText.enabled = true;

        StartCoroutine(ReplaceSwitch(_onSwitcherPosition, _enableColor));
    }

    private void SwitchOff() {
        _sliderState = SliderState.OFF;

        _offText.enabled = true;
        _onText.enabled = false;

        StartCoroutine(ReplaceSwitch(_offSwitcherPosition, _disableColor));
    }

    IEnumerator ReplaceSwitch(Vector3 target, Color imageComponentColor) {
        for (float t = 0; t < 1; t += Time.unscaledDeltaTime / _timeToSwitchState) {
            _switcher.rectTransform.localPosition = Vector3.Lerp(_switcher.rectTransform.localPosition, target, t);
            _imageComponent.color = Color.Lerp(_imageComponent.color, imageComponentColor, t);
            yield return null;
        }

        _switcher.rectTransform.localPosition = target;
        _switcher.rectTransform.sizeDelta = new Vector2(_switcher.rectTransform.rect.width, _switcherHeight);
    }
}
