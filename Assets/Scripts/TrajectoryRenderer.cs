using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour {
    [SerializeField] private GameObject _dotPrefab;
    [SerializeField] private int _dotCount = 10;
    private GameObject[] dotsArray;

    private void Start() {
        dotsArray = new GameObject[_dotCount];
        for (int i = 0; i < dotsArray.Length; i++) {
            dotsArray[i] = Instantiate(_dotPrefab);

            Vector3 prefabScale = _dotPrefab.transform.localScale;
            dotsArray[i].transform.localScale = new Vector3(prefabScale.x - 0.02f * i, prefabScale.y - 0.02f * i, prefabScale.z);

            dotsArray[i].SetActive(false);
        }
    }

    public void ShowTrajectory(Vector3 origin, Vector3 speed) {
        for (int i = 0; i < dotsArray.Length; i++) {
            float time = i * 0.1f;
            Vector2 dotPosition = origin + speed * time + Physics.gravity * time * time / 2f;
            dotsArray[i].transform.position = dotPosition;
            dotsArray[i].SetActive(true);
        }
    }

    public void HideTrajectory() {
        for (int i = 0; i < dotsArray.Length; i++)
            dotsArray[i].SetActive(false);
    }
}
