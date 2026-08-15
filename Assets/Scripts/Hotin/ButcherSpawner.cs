using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ButcherSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject butcherPrefab;
    [SerializeField]
    private List<Transform> spawnpoints;
    [SerializeField]
    private AudioSource butcherSpawnAudioSource;

    private bool butcherIsAlive = false;
    private ButcherController butcher;

    public void SpawnButcher()
    {
        if (butcherIsAlive)
        {
            return;
        }

        butcher = Instantiate(butcherPrefab, GetSpawnPosition(), Quaternion.identity).GetComponent<ButcherController>();
        butcherSpawnAudioSource.Play();
    }

    public void DespawnButcher()
    {
        Destroy(butcher.gameObject);
        butcherIsAlive = false;
    }

    private Vector3 GetSpawnPosition()
    {
        float maxDistance = 0;
        Transform candidate = spawnpoints[0];
        foreach (Transform spawnpoint in spawnpoints)
        {
            float d = (spawnpoint.transform.position - PlayerController.Instance.transform.position).sqrMagnitude;
            if (d > maxDistance)
            {
                maxDistance = d;
                candidate = spawnpoint;
            }
        }
        return candidate.position;
    }
}
