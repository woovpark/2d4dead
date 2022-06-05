using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LootTypeEnum { EXP, Bullet, HPItem }

public class LootScript : MonoBehaviour
{
    public LootTypeEnum LootType = LootTypeEnum.EXP;
    public AudioClip DropGetClip;
    public GameObject[] LootImages;

    public float LootValue = 0;

    private GDataScript gGD;
    private GMScript gGM;
    private Collider2D mCollider;
    private bool mEnabled = true;

    private void Awake()
    {
        gGD = GDataScript.instance;
        gGM = gGD.GetGM();
        mCollider = GetComponent<Collider2D>();
        foreach (var eachImg in LootImages)
        {
            eachImg.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        if (!mEnabled) return;
        mEnabled = false;

        //Debug.Log("아이템 습득! " + LootType);

        var playerData = collision.gameObject.GetComponent<JuingongDataScript>();
        gGM.PlayGlobalSound(DropGetClip);
        bool isOKToDestroy = true;
        switch (LootType)
        {
            case LootTypeEnum.EXP:
                {
                    playerData.GetEXP(LootValue);
                }
                break;
            case LootTypeEnum.Bullet:
                {
                    isOKToDestroy = playerData.AddBullet(LootValue);
                }
                break;
            case LootTypeEnum.HPItem:
                {
                    playerData.Heal(LootValue);
                }
                break;
        }
        if (isOKToDestroy) Destroy(gameObject);
        else mEnabled = true;
    }

    public void SetupLootItem(LootTypeEnum pType)
    {
        LootType = pType;

        LootImages[(int)LootType].SetActive(true);
        
        switch (LootType)
        {
            case LootTypeEnum.EXP:
                LootValue = gGD.ZombieEXP;
                break;
            case LootTypeEnum.Bullet:
                LootValue = gGD.LootBulletCount;
                break;
            case LootTypeEnum.HPItem:
                LootValue = gGD.LootHPBoxHeal;
                break;
        }
    }
}
