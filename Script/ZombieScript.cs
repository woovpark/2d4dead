using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieScript : MonoBehaviour
{
    public GameObject LootPrefab;
    public Slider UIHPBar;
    public float EXPRateMax = 70f;
    public float BulletRateMax = 95f;

    private Rigidbody2D mBody;
    private ZombieDataScript mData;
    private ZombieSoundScript mSound;
    private GMScript gGM;

    private void Awake()
    {
        gGM = GDataScript.instance.GetGM();
        mBody = GetComponent<Rigidbody2D>();
        mData = GetComponent<ZombieDataScript>();
        mSound = GetComponent<ZombieSoundScript>();
        UIHPBar.gameObject.SetActive(false);
        UIHPBar.value = UIHPBar.maxValue;
    }

    public void SetSpeed(Vector2 pSpeed)
    {
        mBody.velocity = pSpeed;
    }

    private void Update()
    {
        var juing = FindObjectOfType<JuingongScript>();
        if (juing == null) return;

        var jPos = juing.gameObject.transform.position;
        var chaseVec = (jPos - transform.position).normalized * 2f;
        //Debug.Log(chaseVec);
        SetSpeed((Vector2)chaseVec);
    }

    public void SetHPBar(float pHPPercent)
    {
        if (pHPPercent != 100) UIHPBar.gameObject.SetActive(true);
        UIHPBar.value = pHPPercent;
    }

    public void DropLootItem()
    {
        var aLoot = Instantiate(LootPrefab);
        aLoot.transform.position = transform.position;

        var aRandom = Random.Range(0f, 100f);
        var aType = LootTypeEnum.EXP;
        
        if ((0 < aRandom)&& (aRandom < EXPRateMax))
        {

        }
        else if ((EXPRateMax < aRandom) && (aRandom < BulletRateMax))
        {
            aType = LootTypeEnum.Bullet;
        }
        else if (BulletRateMax < aRandom)
        {
            aType = LootTypeEnum.HPItem;
        }

        aLoot.GetComponent<LootScript>().SetupLootItem(aType);
    }

    public void DoDie()
    {
        DropLootItem();
        gGM.PlayGlobalSound(mData.DieSound);
    }

    public void PlayHitSound()
    {
        mSound.HitSound();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //FIXME: 대미지가 계속 플레이어에게 주어지도록 수정 / enabler 고려 작업
        if (collision.tag != "Player") return;

        var playerData = collision.gameObject.GetComponent<JuingongDataScript>();
        playerData.GetHit(mData.Power);
    }
}
