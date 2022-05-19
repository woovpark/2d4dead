using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageBlinkScript : MonoBehaviour
{
    public SpriteRenderer CharImage;
    public float BlinkSec = 0.1f;

    private Color mOrgColor;

    private void Start()
    {
        mOrgColor = CharImage.color;
    }

    public void DoBlink()
    {
        CharImage.color = Color.white;
        StartCoroutine(WaitAndSetOrgColor());
    }

    IEnumerator WaitAndSetOrgColor()
    {
        yield return new WaitForSeconds(BlinkSec);
        CharImage.color = mOrgColor;
    }
}
