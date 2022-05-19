using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowJuingong : MonoBehaviour
{
    private GMScript gGM;
    private void Awake()
    {
        gGM = GDataScript.instance.GetGM();
    }
    void Update()
    {
        var juingongPos = gGM.GetJuingongPos();
        transform.position = new Vector3(juingongPos.x, juingongPos.y, transform.position.z);
    }
}
