using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borders : MonoBehaviour {
    private float _borderPosition;
    public static Borders Instance;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        Vector2 instantiatePoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2));
        _borderPosition = instantiatePoint.x;
        InstantiateBorders(_borderPosition + 0.5f);
    }

    private void InstantiateBorders(float x) {
        GameObject leftBorder = Instantiate(new GameObject(), new Vector2(-x, 0), Quaternion.identity, transform);
        GameObject rigthBorder = Instantiate(new GameObject(), new Vector2(x, 0), Quaternion.identity, transform);

        BoxCollider2D leftCollider = leftBorder.AddComponent<BoxCollider2D>();
        BoxCollider2D rightCollider = rigthBorder.AddComponent<BoxCollider2D>();

        leftCollider.size = new Vector2(1f, 20f);
        rightCollider.size = new Vector2(1f, 20f);
    }

    public float BorderPosition { get => _borderPosition; }
}