using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LootTypeEnum { EXP, Bullet, HPItem, Gun }

public class LootScript : MonoBehaviour
{
    public LootTypeEnum LootType = LootTypeEnum.EXP;
    public AudioClip DropGetClip;
    public GameObject[] LootImages;

    public float LootValue = 0;

    private GDataScript gGD;
    private GMScript gGM;
    private Collider2D mCollider;

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

        //Debug.Log("æ∆¿Ã≈€ Ω¿µÊ! " + LootType);

        var playerData = collision.gameObject.GetComponent<JuingongDataScript>();
        gGM.PlayGlobalSound(DropGetClip);
        switch (LootType)
        {
            case LootTypeEnum.EXP:
                {
                    playerData.GetEXP(LootValue);
                }
                break;
            case LootTypeEnum.Bullet:
                break;
            case LootTypeEnum.HPItem:
                {
                    playerData.Heal(LootValue);
                }
                break;
            case LootTypeEnum.Gun:
                break;
        }
        Destroy(gameObject);
    }

    public void SetupLootItem(LootTypeEnum pType)
    {
        LootType = pType;

        LootImages[(int)LootType].SetActive(true);

        
        switch (LootType)
        {
            case LootTypeEnum.EXP:
                LootValue = gGD.ZombieInitialEXP;
                break;
            case LootTypeEnum.Bullet:
                break;
            case LootTypeEnum.HPItem:
                LootValue = 10;
                break;
            case LootTypeEnum.Gun:
                break;
        }
    }
}
