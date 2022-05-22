using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMouseScript : MonoBehaviour
{
    public SpriteRenderer CharImage;
    public GameObject ImageObj;

    private JuingongControlScript mControl;

    private void Awake()
    {
        mControl = GetComponent<JuingongControlScript>();
    }

    private void Update()
    {
        if (mControl.AimVector.x > 0)
        {
            ImageObj.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            ImageObj.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
