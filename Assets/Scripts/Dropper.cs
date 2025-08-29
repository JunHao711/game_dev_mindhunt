using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    public GameObject rockPrefab;
    public Transform spawnPoint;
    public float interval = 2.5f;
    public Vector2 randomJitter = new Vector2(-0.4f, 0.4f);

    IEnumerator Start()
    {
        yield return null;
        while (true)
        {
            Instantiate(rockPrefab, spawnPoint ? spawnPoint.position : transform.position, Quaternion.identity);
            float t = interval + Random.Range(randomJitter.x, randomJitter.y);
            yield return new WaitForSeconds(Mathf.Max(0.1f, t));
        }
    }
}
