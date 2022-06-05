using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManScript : MonoBehaviour
{
    public GameObject BGTile;

    public int MapLv { get; set; }

    public bool IsBattleField { get; set; }

    private GMScript gGM;

    void CheckGM()
    {
        if (!gGM) gGM = GDataScript.instance.GetGM();
    }

    private void Start()
    {
        // 초기화
        MapLv = 1;
        IsBattleField = false;
    }

    // 휴1 - 전1 - 휴2 - 전2 - 휴3 - 전3 -... - 휴10 - 전10 - 헬기(휴11)?

    public void EnterPortal(PortalTypeEnum pType)
    {
        switch (pType)
        {
            case PortalTypeEnum.Next:
                {
                    if (IsBattleField)
                    {
                        MapLv++;
                    }
                }
                break;
            case PortalTypeEnum.Prev:
                {
                    if (!IsBattleField)
                    {
                        if (MapLv == 1) return;
                        MapLv--;
                    }
                }
                break;
        }
        IsBattleField = !IsBattleField;

        CheckGM();
        //print("map lv " + MapLv + " / is battle field " + IsBattleField);
        gGM.LoadMap(BGTile, pType, IsBattleField);
    }
}
