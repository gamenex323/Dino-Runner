using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float leftEdge;
    public bool isPlatform;
    public GameObject platform;

    private void OnEnable()
    {
        if (!isPlatform)
            return;
        int rnd = Random.Range(0, 2);
        if (rnd == 1)
        {
            platform.SetActive(true);
        }
        else
        {
            platform.SetActive(false);

        }
    }

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 2f;
    }

    private void Update()
    {
        transform.position += GameManager.Instance.gameSpeed * Time.deltaTime * Vector3.left;

        if (transform.position.x < leftEdge)
        {
            Destroy(gameObject);
        }
    }

}
