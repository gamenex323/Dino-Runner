using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public struct HurdleSpawnableObject
    {
        public GameObject prefab;
        [Range(0f, 1f)]
        public float spawnChance;
    }
    [System.Serializable]
    public struct CoinSpawnableObject
    {
        public GameObject prefab;
        [Range(0f, 1f)]
        public float spawnChance;
    }

    public HurdleSpawnableObject[] hurdleObjects;
    public CoinSpawnableObject[] coinsObjects;
    public float minSpawnRate = 1f;
    public float maxSpawnRate = 2f;

    private void OnEnable()
    {
        Invoke(nameof(SpawnHurdles), Random.Range(minSpawnRate, maxSpawnRate));
        Invoke(nameof(SpawnCoins), Random.Range(minSpawnRate, maxSpawnRate));
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void SpawnHurdles()
    {
        float spawnChance = Random.value;

        foreach (HurdleSpawnableObject obj in hurdleObjects)
        {
            if (spawnChance < obj.spawnChance)
            {
                if (obj.prefab.transform.childCount > 0)
                {
                    if (obj.prefab.transform.GetChild(0).GetComponent<Obstacle>())
                    {
                        if (obj.prefab.transform.GetChild(0).GetComponent<Obstacle>().isbird)
                        {
                            if (GameManager.Instance.CheckPLatformHurdle())
                            {
                                SpawnHurdles();
                                return;
                            }

                        }
                    }
                }
                GameObject obstacle = Instantiate(obj.prefab);
                GameManager.Instance.instantiatedObstacles.Add(obstacle);
                obstacle.transform.position += transform.position;
                break;
            }

            spawnChance -= obj.spawnChance;
        }

        Invoke(nameof(SpawnHurdles), Random.Range(minSpawnRate, maxSpawnRate));
    }


    private void SpawnCoins()
    {
        float spawnChance = Random.value;

        foreach (CoinSpawnableObject obj in coinsObjects)
        {
            if (spawnChance < obj.spawnChance)
            {
                GameObject obstacle = Instantiate(obj.prefab);
                obstacle.transform.position += transform.position;
                break;
            }

            spawnChance -= obj.spawnChance;
        }

        Invoke(nameof(SpawnCoins), Random.Range(minSpawnRate, maxSpawnRate));
    }


}
