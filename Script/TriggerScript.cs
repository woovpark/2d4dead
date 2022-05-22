using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerStateType { TSReady, TSReloading, }

public class TriggerScript : MonoBehaviour
{
    public GameObject BulletPrefab;

    private TriggerStateType mState;
    private bool mIsPressed;
    
    private GMScript gGM;
    private JuingongDataScript mData;
    private JuingongScript mJuingong;
    private JuingongControlScript mControl;


    private void Awake()
    {
        mJuingong = GetComponent<JuingongScript>();
        mData = GetComponent<JuingongDataScript>();
        mControl = GetComponent<JuingongControlScript>();
        gGM = GDataScript.instance.GetGM();
    }

    private void Start()
    {
        // 무기 초기화
        foreach (var eachWeap in mData.Weapons)
        {
            eachWeap.GameMagCap = eachWeap.MagazineCap;
        }
        mState = TriggerStateType.TSReady;
    }

    public void FireOnce()
    {
        var bulletVel = mControl.AimVector.normalized;
        var aBullet = Instantiate(BulletPrefab);
        aBullet.transform.position = mData.GetCurrentWeaponMuzzlePoint().position;
        aBullet.GetComponent<BulletScript>().SetVelocity(bulletVel);

        mData.Weapons[mJuingong.WeaponIndex].GameMagCap--;

        if (mData.Weapons[mJuingong.WeaponIndex].GameMagCap <= 0)
        {
            StopAllCoroutines();
            StartReload();
            return;
        }
    }

    IEnumerator AutoFire()
    {
        var aDelay = mData.GetCurrentWeaponDelay();
        while (true)
        {
            FireOnce();
            yield return new WaitForSeconds(aDelay);
        }
    }

    public void ButtonDown()
    {
        mIsPressed = true;

        if (mState == TriggerStateType.TSReady)
        {
            StartFire();
        }
        //else if (mState == TriggerStateType.TSReloading) // 무시함
    }

    public void ButtonUp()
    {
        mIsPressed = false;

        if (mState == TriggerStateType.TSReady)
        {
            StopFire();
        }
        //else if (mState == TriggerStateType.TSReloading) // 무시함
    }

    public void StartFire()
    {
        StopAllCoroutines();
        if (mData.Weapons[mJuingong.WeaponIndex].GameMagCap <= 0)
        {
            StartReload();
            return;
        }

        StartCoroutine(AutoFire());
    }

    public void StopFire()
    {
        StopAllCoroutines();
    }

    public void StartReload()
    {
        mState = TriggerStateType.TSReloading;

        //Debug.Log("== StartReload ");
        // 리로딩 딜레이
        StopAllCoroutines();
        StartCoroutine(SubReload());
    }

    IEnumerator SubReload()
    {
        var aUIScr = FindObjectOfType<UIScript>();

        var aCurWeap = mData.Weapons[mJuingong.WeaponIndex];

        // 리로딩 비주얼
        float aTime = aCurWeap.ReloadTime;
        while (aTime > 0)
        {
            aTime -= Time.deltaTime;
            float aPercent = aTime / aCurWeap.ReloadTime * 100f;
            aUIScr.SetReloadGauge(aPercent);

            yield return null;
        }

        aUIScr.SetReloadGauge(100);

        aCurWeap.GameMagCap = aCurWeap.MagazineCap;
        mState = TriggerStateType.TSReady;
        //Debug.Log("            End Reload ======");
        if (mIsPressed)
        {
            StartFire();
        }
    }
}
