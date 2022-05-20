using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDataScript : MonoBehaviour
{
    public float HP = 10;
    public float Power = 5;

    [Space]
    public AudioClip DieSound;
    public AudioClip HitSound;

    private float mHP;
    private ZombieScript mZScript;
    private ImageBlinkScript mBlink;

    private void Awake()
    {
        mZScript = GetComponent<ZombieScript>();
        mBlink = GetComponentInChildren<ImageBlinkScript>();
        mHP = HP;
    }

    public void GetHit(float pDamage)
    {
        mZScript.PlayHitSound();
        mBlink.DoBlink();

        mHP -= pDamage;
        if (mHP <= 0)
        {
            mZScript.DoDie();
            Destroy(gameObject);
        }

        var aPercent = mHP / HP * 100f;
        mZScript.SetHPBar(aPercent);
    }

}
