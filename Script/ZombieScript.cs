using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieScript : MonoBehaviour
{
    public GameObject LootPrefab;
    public Slider UIHPBar;
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
        /*
        if ((0 < aRandom)&& (aRandom < 50))
        {

        }
        else if ((50 < aRandom) && (aRandom < 80))
        {
            aType = LootTypeEnum.Bullet;
        }
        else if ((80 < aRandom) && (aRandom < 95))
        {
            aType = LootTypeEnum.HPItem;
        }
        else if (95 < aRandom)
        {
            aType = LootTypeEnum.Gun;
        }*/

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
        if (collision.tag != "Player") return;

        var playerData = collision.gameObject.GetComponent<JuingongDataScript>();
        playerData.GetHit(mData.Power);
    }
}
