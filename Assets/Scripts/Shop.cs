using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
    [Header("Shop Settings")]
    [SerializeField] private RectTransform _buttonParent;
    private ShopButton[] _shopButtons;
    private bool[] _isBoughtSkin;

    [Header("Shop Elements")]
    [SerializeField] private ShopButton _buttonPrefab;
    [SerializeField] private Sprite[] _ballSkins;
    [SerializeField] private int _priseOfSkin = 100;

    private string _ballBuySave { get; } = "BallBuySave";

    public static Shop Instance;

    private void Awake() {
        if (Instance == null)
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
                _isBoughtSkin[i] = i == 0;
                PlayerPrefs.SetInt(_ballBuySave + i, _isBoughtSkin[i] ? 1 : 0);
                PlayerPrefs.Save();
            }

            _shopButtons[i].LockMode.SetActive(!_isBoughtSkin[i]);
            _shopButtons[i].ChoosenBacklight.SetActive(GameManager.Instance.ChoosenBallSkinSequenceNumber == i);
            _shopButtons[i].BallImage.sprite = _ballSkins[i];
        }

        _buttonParent.sizeDelta = new Vector2(_buttonParent.rect.width, Mathf.Ceil(_ballSkins.Length * (_buttonParent.rect.width + 10f) / 9f));
    }

    public void OnClickAction(int SequenceNumber) {
        if (!_isBoughtSkin[SequenceNumber]) {
            if (GameManager.Instance.StarCount >= _priseOfSkin) {
                GameManager.Instance.ChangeStarCount(-_priseOfSkin);
                _shopButtons[SequenceNumber].LockMode.SetActive(false);
                _isBoughtSkin[SequenceNumber] = true;
                PlayerPrefs.SetInt(_ballBuySave + SequenceNumber, 1);
                PlayerPrefs.Save();
                AudioManager.Instance.PlayShopBuy();
            } else {
                AudioManager.Instance.PlayShopLocked();
                return;
            }
        }

        _shopButtons[GameManager.Instance.ChoosenBallSkinSequenceNumber].ChoosenBacklight.SetActive(false);
        GameManager.Instance.ChoosenBallSkinSequenceNumber = SequenceNumber;
        _shopButtons[SequenceNumber].ChoosenBacklight.SetActive(true);
        Ball.Instance.ChangeSkin(_ballSkins[SequenceNumber]);
        AudioManager.Instance.PlayShopSelect();
    }

    public Sprite GetBallSkin(int _sequenceNumber) {
        return _ballSkins[_sequenceNumber];
    }
}
