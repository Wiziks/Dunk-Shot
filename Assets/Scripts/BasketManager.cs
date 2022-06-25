using System;
using UnityEngine;
using TMPro;

enum SpawnVariantsByRotation {
    None,
    Angle15,
    Angle30,
    Angle45,
}

enum SpawnVariantsByModificators {
    None,
    Star,
    Wall,
    Bouncer
}

public class BasketManager : MonoBehaviour {
    [Header("Spawn Parameters")]
    [SerializeField] private Basket _basketPrefab;
    [SerializeField] private float _minXBound;
    [SerializeField] private float _maxXBound;
    [SerializeField] private float _minYBound;
    [SerializeField] private float _maxYBound;

    [Header("First Baskets")]
    [SerializeField] private Vector3 _firstBasketPosition;
    [SerializeField] private Vector3 _secondBasketPosition;
    private bool _isBasketStarting;

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    private int _score = 0;

    [Header("Modificators")]
    [SerializeField] private Transform _bouncer;
    [SerializeField] private Transform _wall;
    [SerializeField] private Star _star;

    private Basket[] _basketPool;
    private int currentPoolIndex = 0;

    public static BasketManager Instance;

    private void Awake() {
        if (Instance == null)
            Instance = this;
    }

    private void Start() {
        _basketPool = new Basket[2];
        for (int i = 0; i < _basketPool.Length; i++)
            _basketPool[i] = Instantiate(_basketPrefab, transform);

        _basketPool[0].transform.position = _firstBasketPosition;
        _basketPool[1].transform.position = _secondBasketPosition;

        _isBasketStarting = true;
    }

    public void Spawn(int throwScore) {
        if (_isBasketStarting) {
            _isBasketStarting = false;
            return;
        }

        float x = UnityEngine.Random.Range(_minXBound, _maxXBound);
        float y = UnityEngine.Random.Range(_minYBound, _maxYBound);
        if (Ball.Instance.transform.position.x > 0) x *= -1f;

        Vector3 spawnPosition = new Vector3(x, y, 0);

        _basketPool[currentPoolIndex].transform.position = spawnPosition;
        _basketPool[currentPoolIndex].FirstTimeInBasket = true;

        int spawnVariantsByRotationCount = Enum.GetNames(typeof(SpawnVariantsByRotation)).Length;
        int randomSpawnByRotationVariant = UnityEngine.Random.Range(0, spawnVariantsByRotationCount);

        Vector3 basketRotation = Vector3.zero;
        if ((SpawnVariantsByRotation)randomSpawnByRotationVariant == SpawnVariantsByRotation.None) {
            basketRotation = Vector3.zero;
        } else if ((SpawnVariantsByRotation)randomSpawnByRotationVariant == SpawnVariantsByRotation.Angle15) {
            basketRotation = new Vector3(0f, 0f, 15f);
        } else if ((SpawnVariantsByRotation)randomSpawnByRotationVariant == SpawnVariantsByRotation.Angle30) {
            basketRotation = new Vector3(0f, 0f, 30f);
        } else if ((SpawnVariantsByRotation)randomSpawnByRotationVariant == SpawnVariantsByRotation.Angle45) {
            basketRotation = new Vector3(0f, 0f, 45f);
        }

        if (Ball.Instance.transform.position.x > 0) basketRotation *= -1f;
        _basketPool[currentPoolIndex].transform.rotation = Quaternion.Euler(basketRotation);

        _star.gameObject.SetActive(false);
        _wall.gameObject.SetActive(false);
        _bouncer.gameObject.SetActive(false);

        int spawnVariantsByModificatorsCount = Enum.GetNames(typeof(SpawnVariantsByModificators)).Length;
        int randomSpawnByModificatorsVariant = UnityEngine.Random.Range(0, spawnVariantsByModificatorsCount);

        if ((SpawnVariantsByModificators)randomSpawnByModificatorsVariant == SpawnVariantsByModificators.Star) {
            _star.transform.position = _basketPool[currentPoolIndex].StarPosition;
            _star.gameObject.SetActive(true);
            _star.UpdateStartPosition();
        } else if ((SpawnVariantsByModificators)randomSpawnByModificatorsVariant == SpawnVariantsByModificators.Wall) {
            Vector3 wallPosition;

            if (Ball.Instance.transform.position.x > 0) wallPosition = _basketPool[currentPoolIndex].RightWallPosition;
            else wallPosition = _basketPool[currentPoolIndex].LeftWallPosition;
            _wall.position = wallPosition;

            _wall.gameObject.SetActive(true);
        } else if ((SpawnVariantsByModificators)randomSpawnByModificatorsVariant == SpawnVariantsByModificators.Bouncer) {
            _bouncer.position = _basketPool[currentPoolIndex].BouncerPosition;
            _bouncer.gameObject.SetActive(true);
        }

        _basketPool[currentPoolIndex].BasketState = BasketState.Static;
        _basketPool[currentPoolIndex].ChangeRingColor();

        _score += throwScore;
        _scoreText.text = _score + "";

        currentPoolIndex++;
        if (currentPoolIndex == _basketPool.Length) currentPoolIndex = 0;
    }

    public void GameOver() {
        GameManager.Instance.SetHighScore(_score);
    }
}
