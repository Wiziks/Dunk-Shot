using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour {
    [SerializeField] private Image _ballImage;
    [SerializeField] private GameObject _choosenBacklight;
    [SerializeField] private GameObject _lockMode;
    private int _sequenceNumber;

    private void Start() {
        GetComponent<Button>().onClick.AddListener(OnClickAction);
    }

    public void OnClickAction() {
        Shop.Instance.OnClickAction(_sequenceNumber);
    }

    public Image BallImage { get => _ballImage; set => _ballImage = value; }
    public GameObject ChoosenBacklight { get => _choosenBacklight; }
    public GameObject LockMode { get => _lockMode; }
    public int SequenceNumber { get => _sequenceNumber; set => _sequenceNumber = value; }
}
