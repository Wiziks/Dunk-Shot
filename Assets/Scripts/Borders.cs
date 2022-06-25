using UnityEngine;

public class Borders : MonoBehaviour {
    [SerializeField] private PhysicsMaterial2D _borderPhysicsMaterial2D;

    private float _borderPosition;

    public static Borders Instance;

    private void Awake() {
        if (Instance == null)
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

        leftCollider.sharedMaterial = _borderPhysicsMaterial2D;
        rightCollider.sharedMaterial = _borderPhysicsMaterial2D;
    }

    public float BorderPosition { get => _borderPosition; }
}