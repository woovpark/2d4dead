using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDataScript : MonoBehaviour
{
    [HideInInspector]
    public float Power;

    [Space]
    public AudioClip DieSound;
    public AudioClip HitSound;

    private MapManScript gMap;

    private float mOrgHP;
    private float mHP;
    private ZombieScript mZScript;
    private ImageBlinkScript mBlink;

    private void Awake()
    {
        var gGD = GDataScript.instance;
        gMap = gGD.GetMap();
        mZScript = GetComponent<ZombieScript>();
        mBlink = GetComponentInChildren<ImageBlinkScript>();

        mOrgHP = gGD.GetZombieHP(gMap.MapLv);
        Power = gGD.GetZombieAtk(gMap.MapLv);
        //print("z hp " + mOrgHP + " pow " + Power);
        mHP = mOrgHP;
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

        var aPercent = mHP / mOrgHP * 100f;
        mZScript.SetHPBar(aPercent);
    }

}
