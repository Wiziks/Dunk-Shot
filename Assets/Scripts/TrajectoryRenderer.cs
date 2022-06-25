using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour {
    [SerializeField] private SpriteRenderer _dotPrefab;
    [SerializeField] private int _dotCount = 10;

    private SpriteRenderer[] dotsArray;

    private void Start() {
        dotsArray = new SpriteRenderer[_dotCount];
        for (int i = 0; i < dotsArray.Length; i++) {
            dotsArray[i] = Instantiate(_dotPrefab, transform);

            Vector3 prefabScale = _dotPrefab.transform.localScale;
            dotsArray[i].transform.localScale = new Vector3(prefabScale.x - 0.02f * i, prefabScale.y - 0.02f * i, prefabScale.z);

            dotsArray[i].gameObject.SetActive(false);
        }
    }

    public void ShowTrajectory(Vector3 origin, Vector3 speed, float alpha) {
        bool isSwitched = false;
        bool isLeft = false;
        for (int i = 0; i < dotsArray.Length; i++) {
            float time = i * 0.1f;

            Vector2 dotPosition = origin + speed * time + Physics.gravity * time * time / 2f;

            if (Mathf.Abs(dotPosition.x) > Borders.Instance.BorderPosition && !isSwitched) {
                speed.x *= -1f;
                isLeft = dotPosition.x > 0;
                Debug.Log(dotPosition.x);
                dotPosition = origin + speed * time + Physics.gravity * time * time / 2f;
                isSwitched = true;
            }

            if (isSwitched)
                dotPosition.x += (Borders.Instance.BorderPosition + 0.5f) * (isLeft ? 1f : -1f);

            dotsArray[i].transform.position = dotPosition;

            dotsArray[i].color = new Color(dotsArray[i].color.r, dotsArray[i].color.g, dotsArray[i].color.b, alpha);
            dotsArray[i].gameObject.SetActive(true);
        }
    }

    public void HideTrajectory() {
        for (int i = 0; i < dotsArray.Length; i++)
            dotsArray[i].gameObject.SetActive(false);
    }
}