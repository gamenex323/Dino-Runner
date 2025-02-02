using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float leftEdge;
    public bool isPlatform;
    public GameObject platform;
    public bool isbird;
    public float birdSpeed;
    private bool obstacleDestroyCall = false;

    private void OnEnable()
    {
        if (!isPlatform)
            return;
        int rnd = Random.Range(0, 2);
        if (rnd == 1)
        {
            if (platform)
                platform.SetActive(true);
        }
        else
        {
            if (platform)
                platform.SetActive(false);

        }
    }

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 2f;
    }

    private void Update()
    {
        if (Player.instance)
        {
            if (Player.instance.isStop)
            {
                return;
            }
        }
        if (isbird)
        {
            transform.position += birdSpeed * Time.deltaTime * Vector3.left;
        }
        else
        {
            transform.position += GameManager.Instance.gameSpeed * Time.deltaTime * Vector3.left;
        }
        if (transform.position.x < leftEdge)
        {

            if (!obstacleDestroyCall)
            {
                obstacleDestroyCall = true;
                if (isPlatform)
                {
                    DG.Tweening.DOVirtual.DelayedCall(5f, () => Destroy(gameObject));
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Trigger With: " + other.gameObject.name);
        if (gameObject.CompareTag("Coins"))
        {
            Debug.Log("Trigger With: ", other.gameObject);

            if (other.CompareTag("Obstacle"))
            {
                Destroy(gameObject);
            }
            if (other.CompareTag("Platform"))
            {
                Destroy(gameObject);
            }
        }

    }
}
