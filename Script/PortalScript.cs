using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PortalTypeEnum { Next, Prev }

public class PortalScript : MonoBehaviour
{
    public PortalTypeEnum PortalType;
    public Transform StartPoint;

    private GMScript gGM;
    private MapManScript gMap;
    private bool mEnabled = true;

    private void Awake()
    {
        gGM = GDataScript.instance.GetGM();
        gMap = GDataScript.instance.GetMap();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        if (!mEnabled) return;
        mEnabled = false;

        gMap.EnterPortal(PortalType);
    }
}
