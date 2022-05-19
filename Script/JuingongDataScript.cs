using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponInfo
{
    public Sprite WeaponImage;
    public Transform MuzzlePoint;
    public float RPS; // delay : 1sec/RPS
    public int MagazineCap;
    public int InventoryCap;
    public float ReloadTime;

    public int GameMagCap { get; set; }
}

public class JuingongDataScript : MonoBehaviour
{
    public float CurEXP = 0;
    public int Lv = 1;

    public float HP = 10;
    public float AtkInitial = 2.5f;
    //public float Def = 1;

    public WeaponInfo[] Weapons;

    [Space]
    public AudioClip FootStepClip;

    private float mHP;
    private JuingongScript mParentScript;
    private GDataScript gGD;

    private void Awake()
    {
        mParentScript = GetComponent<JuingongScript>();
        mHP = HP;
        gGD = GDataScript.instance;
    }

    void RefreshHPBar()
    {
        var aPercent = mHP / HP * 100f;
        mParentScript.SetHPBar(aPercent);
    }

    public void GetHit(float pDamage)
    {
        mHP -= pDamage;
        if (mHP <= 0)
        {
            mParentScript.PlayerDie();
        }

        RefreshHPBar();
    }

    public void Heal(float pAmount)
    {
        mHP += pAmount;
        if (mHP > HP)
        {
            mHP = HP;
        }

        RefreshHPBar();
    }

    public void GetEXP(float pExp)
    {
        CurEXP += pExp;
        var aEXPToUp = gGD.GetEXPToUp(Lv);
        if (CurEXP >= aEXPToUp)
        {
            CurEXP -= aEXPToUp;
            Lv++;
        }

        Debug.Log("Lv " + Lv + "CurEXP " + CurEXP + "EXPToUP " + aEXPToUp);
    }

    public Transform GetCurrentWeaponMuzzlePoint()
    {
        return Weapons[mParentScript.WeaponIndex].MuzzlePoint;
    }
    public float GetCurrentWeaponDelay()
    {
        return 1f / Weapons[mParentScript.WeaponIndex].RPS;
    }
}
