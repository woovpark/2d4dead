using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManScript : MonoBehaviour
{
    public GameObject ZombiePrefab;
    public int ZombieCount = 10;

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
                var aX = Random.Range(-10f, 10f);
                var aY = Random.Range(-10f, 10f);
                aZombie.transform.position = new Vector3(aX, aY, 0);
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
