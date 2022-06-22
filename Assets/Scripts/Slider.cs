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
    [SerializeField] private Image _imageComponent;
    private SliderState _sliderState;


    [ContextMenu("Switch State")]
    public void SwitchState() {
        if (_sliderState == SliderState.ON)
            SwitchOff();
        else
            SwitchOn();
    }

    public void SwitchOn() {
        _sliderState = SliderState.ON;

        _offText.enabled = false;
        _onText.enabled = true;

        if (gameObject.activeInHierarchy)
            StartCoroutine(ReplaceSwitch(_onSwitcherPosition, _enableColor));
        else {
            _switcher.rectTransform.localPosition = _onSwitcherPosition;
            _imageComponent.color = _enableColor;
        }
    }

    public void SwitchOff() {
        _sliderState = SliderState.OFF;

        _offText.enabled = true;
        _onText.enabled = false;

        if (gameObject.activeInHierarchy)
            StartCoroutine(ReplaceSwitch(_offSwitcherPosition, _disableColor));
        else {
            _switcher.rectTransform.localPosition = _offSwitcherPosition;
            _imageComponent.color = _disableColor;
        }
    }

    IEnumerator ReplaceSwitch(Vector3 target, Color imageComponentColor) {
        for (float t = 0; t < 1; t += Time.unscaledDeltaTime / _timeToSwitchState) {
            _switcher.rectTransform.localPosition = Vector3.Lerp(_switcher.rectTransform.localPosition, target, t);
            _imageComponent.color = Color.Lerp(_imageComponent.color, imageComponentColor, t);
            yield return null;
        }

        _switcher.rectTransform.localPosition = target;
        _imageComponent.color = imageComponentColor;
    }
}
