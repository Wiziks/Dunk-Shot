using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
    [SerializeField] private RectTransform _buttonParent;
    [SerializeField] private ShopButton _buttonPrefab;
    [SerializeField] private Sprite[] _ballSkins;
    [SerializeField] private int _priseOfSkin = 100;
    private ShopButton[] _shopButtons;
    private string _ballBuySave { get; } = "BallBuySave";
    private bool[] _isBoughtSkin;
    public static Shop Instance;

    private void Awake() {
        Instance = this;
    }

    void Start() {
        _shopButtons = new ShopButton[_ballSkins.Length];
        _isBoughtSkin = new bool[_ballSkins.Length];

        for (int i = 0; i < _ballSkins.Length; i++) {
            _shopButtons[i] = Instantiate(_buttonPrefab, _buttonParent);
            _shopButtons[i].GetComponent<Image>().sprite = _ballSkins[i];
            _shopButtons[i].SequenceNumber = i;

            if (PlayerPrefs.HasKey(_ballBuySave + i))
                _isBoughtSkin[i] = PlayerPrefs.GetInt(_ballBuySave + i) == 1 ? true : false;
            else {
                _isBoughtSkin[i] = false;
                PlayerPrefs.SetInt(_ballBuySave + i, 0);
                PlayerPrefs.Save();
            }

            _shopButtons[i].LockMode.SetActive(!_isBoughtSkin[i]);
            _shopButtons[i].ChoosenBacklight.SetActive(GameManager.Instance.ChoosenBallSkinSequenceNumber == i);
            _shopButtons[i].BallImage.sprite = _ballSkins[i];
        }

        _buttonParent.sizeDelta = new Vector2(_buttonParent.rect.width, Mathf.Ceil(_ballSkins.Length * _buttonParent.rect.width / 3f));
    }

    public void OnClickAction(int SequenceNumber) {
        if (!_isBoughtSkin[SequenceNumber]) {
            if (GameManager.Instance.StarCount >= _priseOfSkin) {
                GameManager.Instance.ChangeStarCount(-_priseOfSkin);
                _shopButtons[SequenceNumber].LockMode.SetActive(false);
                _isBoughtSkin[SequenceNumber] = true;
                PlayerPrefs.SetInt(_ballBuySave + SequenceNumber, 1);
                PlayerPrefs.Save();
            } else return;
        }

        GameManager.Instance.ChoosenBallSkinSequenceNumber = SequenceNumber;
        _shopButtons[SequenceNumber].ChoosenBacklight.SetActive(true);
        Ball.Instance.ChangeSkin(_ballSkins[SequenceNumber]);
    }
}
