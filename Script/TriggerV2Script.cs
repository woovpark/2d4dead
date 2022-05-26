using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerV2Script : MonoBehaviour
{
    public GameObject BulletPrefab;

    private bool mOKToFire = true;
    public bool OKToFire { get { return mOKToFire; } set { mOKToFire = value; } }
    private bool mIsPressed = false;

    private GMScript gGM;
    private JuingongDataScript mData;
    private JuingongScript mJuingong;
    private JuingongControlScript mControl;
    private UIScript mUI;

    private void Awake()
    {
        mJuingong = GetComponent<JuingongScript>();
        mData = GetComponent<JuingongDataScript>();
        mControl = GetComponent<JuingongControlScript>();
        gGM = GDataScript.instance.GetGM();
        mUI = FindObjectOfType<UIScript>();
    }

    private void Start()
    {
        // 무기 초기화
        foreach (var eachWeap in mData.Weapons)
        {
            eachWeap.GameMagCap = eachWeap.MagazineCap;
            eachWeap.GameInvenCap = eachWeap.InventoryCap;
        }
    }

    public void FireOnce()
    {
        var bulletVel = mControl.AimVector.normalized;
        var aBullet = Instantiate(BulletPrefab);
        aBullet.transform.position = mData.GetCurrentWeaponMuzzlePoint().position;
        aBullet.GetComponent<BulletScript>().SetVelocity(bulletVel);

        mData.Weapons[mJuingong.WeaponIndex].GameMagCap--;
    }

    public void ButtonDown()
    {
        mIsPressed = true;
        if (!mOKToFire) return;

        StartCoroutine(TriggerRoutine());
    }

    public void ButtonUp()
    {
        mIsPressed = false;
    }

    IEnumerator TriggerRoutine()
    {
        mOKToFire = false;
        var aCurWeap = mData.Weapons[mJuingong.WeaponIndex];
        var aDelay = mData.GetCurrentWeaponDelay();

        FireOnce();
        yield return new WaitForSeconds(aDelay);

        // reload check
        if (mData.Weapons[mJuingong.WeaponIndex].GameMagCap <= 0)
        {
            // 소지 총알 갯수가 다 떨어진 경우
            if (aCurWeap.GameInvenCap == 0)
            {
                yield break;
            }

            float aTime = aCurWeap.ReloadTime;
            while (aTime > 0)
            {
                aTime -= Time.deltaTime;
                float aPercent = aTime / aCurWeap.ReloadTime * 100f;
                mUI.SetReloadGauge(aPercent);

                yield return null;
            }
            mUI.SetReloadGauge(100);

            int availableCount = aCurWeap.MagazineCap;
            if (aCurWeap.InventoryCap != -1)
            {
                if (aCurWeap.MagazineCap > aCurWeap.GameInvenCap)
                {
                    availableCount = aCurWeap.GameInvenCap;
                }
                aCurWeap.GameInvenCap -= availableCount;
            }
            aCurWeap.GameMagCap = availableCount;
            
        }
        mOKToFire = true;

        if (mIsPressed) StartCoroutine(TriggerRoutine());
    }
}
