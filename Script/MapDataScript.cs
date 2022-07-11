using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDataScript : MonoBehaviour
{
    public int ZombieCount;
    public int LootDropCount;
    public Collider2D CameraZone;
    public Transform[] ZombieSpawnPoints;
    public Transform[] LootSpawnPoints;
    public Transform EnterPoint;
}
