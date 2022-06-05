using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDataScript : MonoBehaviour
{
    [HideInInspector]
    public float Power; // 발사 순간에 총기 종류에 맞춰 대미지 값을 받는다

    public AudioClip ShotSound;
    
    public void SetPower(float pPower)
    {
        Power = pPower;
    }
}
