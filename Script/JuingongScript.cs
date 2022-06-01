using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JuingongScript : MonoBehaviour
{
    public Slider UIHPBar;

    public SpriteRenderer WeaponImage;

    private GMScript gGM;
    private JuingongDataScript mData;
    private Rigidbody2D mBody;
    private AudioSource mAudio;
    private TriggerV2Script mTrigger;

    public int WeaponIndex { get; set; }

    private void Awake()
    {
        mData = GetComponent<JuingongDataScript>();
        mTrigger = GetComponent<TriggerV2Script>();
        mBody = GetComponent<Rigidbody2D>();
        mAudio = GetComponent<AudioSource>();
        UIHPBar.value = UIHPBar.maxValue;
        gGM = GDataScript.instance.GetGM();
    }

    private void Start()
    {

    }

    public JuingongDataScript GetData() { return mData; }

    public void SetSpeed(Vector2 pSpeed)
    {
        mBody.velocity = pSpeed;
    }

    public void SetHPBar(float pHPPercent)
    {
        UIHPBar.value = pHPPercent;
    }

    public void PlayerDie()
    {
        gGM.DoGameOver();
        Destroy(gameObject);
    }

    public void ChangeWeapon(int pIndex)
    {
        WeaponIndex = pIndex;
        var curWeapon = mData.Weapons[WeaponIndex];
        WeaponImage.sprite = curWeapon.WeaponImage;

        mTrigger.OnChangeWeapon();
    }

    public void ButtonDown()
    {
        mTrigger.ButtonDown();
    }
    public void ButtonUp()
    {
        mTrigger.ButtonUp();
    }

    public void Reload()
    {
        mTrigger.ManualReload();
    }

    public void CheckReload()
    {
        if ((mData.Weapons[1].GameMagCap == 0) && // 탄창의 용량이 0
            (mData.Weapons[1].GameInvenCap > 0)) // 가방에 총알이 있으면
            Reload();
    }


}
