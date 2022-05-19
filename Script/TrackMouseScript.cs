using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMouseScript : MonoBehaviour
{
    public SpriteRenderer CharImage;
    private GMScript gGM;
    public GameObject ImageObj;

    private void Awake()
    {
        gGM = GDataScript.instance.GetGM();
    }

    private void Update()
    {
        //CharImage.flipX = (gGM.AimVector.x > 0);
        if (gGM.AimVector.x > 0)
        {
            ImageObj.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            ImageObj.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
