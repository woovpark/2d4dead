using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerV2Script : MonoBehaviour
{
    public GameObject BulletPrefab;

    private bool mOKToFire = true;
    private bool mIsInReloading = false;
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
        /*/ 무기 초기화
        foreach (var eachWeap in mData.Weapons)
        {
            eachWeap.GameMagCap = eachWeap.MagazineCap;
            eachWeap.GameInvenCap = eachWeap.InventoryCap;
        }*/

        mData.Weapons[0].GameMagCap = mData.Weapons[0].MagazineCap;
        mData.Weapons[0].GameInvenCap = -1; // 권총의 경우 inspector쪽에서 invencap을 지정하는 게 무의미 해짐
        mData.Weapons[1].GameMagCap = mData.Weapons[1].MagazineCap;
        mData.Weapons[1].GameInvenCap = 0;

    }

    public void FireOnce()
    {
        var aCurWeap = mData.Weapons[mJuingong.WeaponIndex];
        var bulletVel = mControl.AimVector.normalized;
        var aBullet = Instantiate(BulletPrefab);
        aBullet.transform.position = mData.GetCurrentWeaponMuzzlePoint().position;
        aBullet.GetComponent<BulletScript>().SetVelocity(bulletVel);
        var totPow = aCurWeap.Power * GDataScript.instance.GetJuingongAtk(mData.Lv);
        aBullet.GetComponent<BulletDataScript>().SetPower(totPow);
        print("bullet power " + totPow);

        aCurWeap.GameMagCap--;
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
            // 소지 총알 갯수가 다 떨어진 경우 - 권총의 경우 GameInvenCap 이 -1 이기에 패스
            if (aCurWeap.GameInvenCap == 0)
            {
                yield break;
            }

            mIsInReloading = true;
            float aTime = aCurWeap.ReloadTime;
            while (aTime > 0)
            {
                aTime -= Time.deltaTime;
                float aPercent = aTime / aCurWeap.ReloadTime * 100f;
                mUI.SetReloadGauge(aPercent);

                yield return null;
            }
            mUI.SetReloadGauge(100);

            FillMagazine();
            mIsInReloading = false;
        }
        mOKToFire = true;

        if (mIsPressed) StartCoroutine(TriggerRoutine());
    }

    // 웨폰 인덱스가 꼭 바뀐 후에 호출할것
    public void OnChangeWeapon()
    {
        StopAllCoroutines();
        mUI.SetReloadGauge(100);

        var curWeapon = mData.Weapons[mJuingong.WeaponIndex];
        mOKToFire = (curWeapon.GameMagCap > 0);

        // 무기 바꾸었는데 탄창에 총알이 없음 (가방에 총알 갖고있음)
        if ((curWeapon.GameMagCap <= 0) && (curWeapon.GameInvenCap != 0))
        {
            StartCoroutine(OnlyReloadRoutine());
        }

        if (mIsPressed) StartCoroutine(TriggerRoutine());
    }

    public void ManualReload()
    {
        if (mIsInReloading) return;

        var curWeapon = mData.Weapons[mJuingong.WeaponIndex];
        if (curWeapon.GameMagCap >= curWeapon.MagazineCap) return;
        if (curWeapon.GameInvenCap == 0) return;

        StopAllCoroutines();
        mOKToFire = false;
        mIsInReloading = true;
        StartCoroutine(OnlyReloadRoutine());
    }

    IEnumerator OnlyReloadRoutine()
    {
        var aCurWeap = mData.Weapons[mJuingong.WeaponIndex];

        float aTime = aCurWeap.ReloadTime;
        while (aTime > 0)
        {
            aTime -= Time.deltaTime;
            float aPercent = aTime / aCurWeap.ReloadTime * 100f;
            mUI.SetReloadGauge(aPercent);

            yield return null;
        }
        mUI.SetReloadGauge(100);

        FillMagazine();

        mOKToFire = true;
        mIsInReloading = false;
        if (mIsPressed) StartCoroutine(TriggerRoutine());
    }

    // 탄창에 남아 있는경우, 무한 총알 등등 모두 고려된 메소드
    void FillMagazine()
    {
        var aCurWeap = mData.Weapons[mJuingong.WeaponIndex];
        
        int bulletWanted = aCurWeap.MagazineCap - aCurWeap.GameMagCap;
        if (aCurWeap.InventoryCap != -1)
        {
            if (bulletWanted > aCurWeap.GameInvenCap)
            {
                bulletWanted = aCurWeap.GameInvenCap;
            }
            aCurWeap.GameInvenCap -= bulletWanted;
        }
        aCurWeap.GameMagCap += bulletWanted;
    }
}
