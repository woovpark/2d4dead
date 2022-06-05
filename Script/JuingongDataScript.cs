using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponInfo
{
    public Sprite WeaponImage;
    public Transform MuzzlePoint;
    public float Power;
    public float RPS; // delay : 1sec/RPS
    public int MagazineCap;
    public int InventoryCap;
    public float ReloadTime;

    public event Action OnMagCapChanged;
    private int mGMC = 0;
    public int GameMagCap { get { return mGMC; } set { mGMC = value; OnMagCapChanged?.Invoke(); } }

    public event Action OnInvenCapChanged;
    private int mGIC = 0;
    public int GameInvenCap { get { return mGIC; } set { mGIC = value; OnInvenCapChanged?.Invoke(); } }
}

public class JuingongDataScript : MonoBehaviour
{
    public float CurEXP = 0;
    public int Lv = 1;

    public float HP;

    public WeaponInfo[] Weapons;

    [Space]
    public AudioClip FootStepClip;

    private float mHP;
    private JuingongScript mParentScript;
    private GDataScript gGD;

    private void Awake()
    {
        mParentScript = GetComponent<JuingongScript>();

        gGD = GDataScript.instance;

        HP = gGD.GetJuingongHP(Lv);
        mHP = HP;
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
        //TODO: 10레벨 이상 안 크게
        CurEXP += pExp;
        var aEXPToUp = gGD.GetEXPToUp(Lv);
        if (CurEXP >= aEXPToUp)
        {
            // 레벨업 프로세스
            CurEXP -= aEXPToUp;
            Lv++;
            HP = gGD.GetJuingongHP(Lv);
            mHP = HP;
            RefreshHPBar();
        }

        Debug.Log("Lv " + Lv + "CurEXP " + CurEXP + "EXPToUP " + aEXPToUp);
    }

    public bool AddBullet(float pAmount)
    {
        if (Weapons[1].GameInvenCap == Weapons[1].InventoryCap) return false;
        
        Weapons[1].GameInvenCap += (int)pAmount;

        // 소총 총알을 주웠지만 현재 권총을 쓰고 있는 경우 끝
        if (mParentScript.WeaponIndex == 0) return true;

        // 현재 소총 쓰고 있고 탄창 = 0 & 가방에 총알이 있을 경우 자동리로딩
        if (Weapons[1].GameInvenCap > Weapons[1].InventoryCap)
            Weapons[1].GameInvenCap = Weapons[1].InventoryCap;
        mParentScript.CheckReload();
        return true;
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
