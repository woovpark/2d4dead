using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManScript : MonoBehaviour
{
    public GameObject ZombiePrefab;
    public int ZombieCount = 10;

    private Transform[] mCurrentMapSpawnPoints;

    public void SetSpawnPointArray(Transform[] pArray)
    {
        mCurrentMapSpawnPoints = pArray;
    }

    public void ClearAllZombies()
    {
        var zombies = FindObjectsOfType<ZombieScript>();
        foreach (var eachZombie in zombies)
        {
            Destroy(eachZombie.gameObject);
        }
    }

    public void RunZombieLoop()
    {
        StartCoroutine(ZombieLoop());
    }

    public void StopZombieLoop()
    {
        StopAllCoroutines();
    }

    IEnumerator ZombieLoop()
    {
        while (true)
        {
            var zombies = FindObjectsOfType<ZombieScript>();
            if (zombies.Length < ZombieCount)
            {
                var aZombie = Instantiate(ZombiePrefab);
                var spawnIndx = Random.Range(0, mCurrentMapSpawnPoints.Length);
                aZombie.transform.position = mCurrentMapSpawnPoints[spawnIndx].position;
            }
            yield return new WaitForSeconds(1f);
        }
    }

}
