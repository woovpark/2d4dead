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

        WeaponImage.sprite = mData.Weapons[WeaponIndex].WeaponImage;
    }

    public void ButtonDown()
    {
        mTrigger.ButtonDown();
    }
    public void ButtonUp()
    {
        mTrigger.ButtonUp();
    }

    
}
