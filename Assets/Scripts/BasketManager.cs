using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

enum SpawnVariantsByRotation {
    None,
    Angle15,
    Angle30,
    Angle45,
}

public class BasketManager : MonoBehaviour {
    [SerializeField] private Basket _basketPrefab;
    [SerializeField] private Vector3 _firstBasketPosition;
    [SerializeField] private Vector3 _secondBasketPosition;
    [SerializeField] private float _minXBound;
    [SerializeField] private float _maxXBound;
    [SerializeField] private float _minYBound;
    [SerializeField] private float _maxYBound;
    [SerializeField] private TextMeshProUGUI _scoreText;
    private Basket[] _basketPool;
    private int currentPoolIndex = 0;
    private bool _isBasketStarting;
    private int _score = 0;
    public static BasketManager Instance;

    private void Awake() {
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

    public void Spawn() {
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

        int spawnVariantsCount = Enum.GetNames(typeof(SpawnVariantsByRotation)).Length;
        int randomSpawnVariant = UnityEngine.Random.Range(0, spawnVariantsCount);

        Vector3 basketRotation = Vector3.zero;
        if ((SpawnVariantsByRotation)randomSpawnVariant == SpawnVariantsByRotation.None) {
            basketRotation = Vector3.zero;
        } else if ((SpawnVariantsByRotation)randomSpawnVariant == SpawnVariantsByRotation.Angle15) {
            basketRotation = new Vector3(0f, 0f, 15f);
        } else if ((SpawnVariantsByRotation)randomSpawnVariant == SpawnVariantsByRotation.Angle30) {
            basketRotation = new Vector3(0f, 0f, 30f);
        } else if ((SpawnVariantsByRotation)randomSpawnVariant == SpawnVariantsByRotation.Angle45) {
            basketRotation = new Vector3(0f, 0f, 45f);
        }

        if (Ball.Instance.transform.position.x > 0) basketRotation *= -1f;
        _basketPool[currentPoolIndex].transform.rotation = Quaternion.Euler(basketRotation);

        _basketPool[currentPoolIndex].BasketState = BasketState.Static;
        _basketPool[currentPoolIndex].ChangeRingColor();

        _score++;
        _scoreText.text = _score + "";

        currentPoolIndex++;
        if (currentPoolIndex == _basketPool.Length) currentPoolIndex = 0;
    }

    public void GameOver() {
        GameManager.Instance.SetHighScore(_score);
    }
}
