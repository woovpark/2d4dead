using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 2디4 데드 - 마무리 단계

// 밸런스 초반에 더 쉽도록 수정
// 휴게소 맵은 좀비 소량 생성, 탐색테마 살린다 / 좀비 생성 루틴은 무한히 돌아간다
// 전투 맵은 작고 탁 트인 공간에서 좀비 웨이브를 5분간 견디는 방식 / 5분이 지나면 좀비 모두 사라지고 더이상 생성안됨 / 좀비 모두 사라질때는 드랍 없음

// 마지막 전투 맵의 헬기 구현 (5분이 지나면 좀비 사라지며 헬기가 도착)
// 휴1(첫 맵)은 특수 맵으로 튜토리얼 맵 역할을 한다

// HP 풀인 상태에서 체력회복 먹어지는 문제 수정
// 맵 이동할때 카메라 움직임이 부자연스러움 체크
// 상 하 방향 총 쏠때 조준 문제

// 모바일로 출시할 경우
// 터치 컨트롤로 수정


public class GDataScript : MonoBehaviour
{
    static public GDataScript instance;

    public float ZombieEXP = 3f;
    public int LootBulletCount = 5;
    public float LootHPBoxHeal = 10;
    [Space]
    public AudioClip GameStartClip;
    public AudioClip GameOverClip;

    [HideInInspector]
    public bool Cheat = false;

    private GMScript mGM;
    private MapManScript mMap;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public GMScript GetGM()
    {
        if (mGM == null)
        {
            mGM = GetComponent<GMScript>();
        }

        return mGM;
    }

    public MapManScript GetMap()
    {
        if (mMap == null)
        {
            mMap = GetComponent<MapManScript>();
        }
        return mMap;
    }

    public float GetEXPToUp(int pLv)
    {
        return pLv * pLv * 150f;
    }

    public float GetZombieAtk(int pLv)
    {
        return (pLv + 1) * 5f;
    }

    public float GetZombieHP(int pLv)
    {
        return pLv * 50f;
    }

    public float GetJuingongAtk(int pLv)
    {
        return 3f * (pLv - 1) * (pLv - 1) + 2f;
    }
    
    public float GetJuingongHP(int pLv)
    {
        return pLv * 50f;
    }

    //==========

    public LootTypeEnum GetRandomLootType(float pEXPRate, float pBulletRate)
    {
        var aRandom = Random.Range(0f, 100f);
        var retType = LootTypeEnum.EXP;

        if ((0 < aRandom) && (aRandom < pEXPRate))
        {

        }
        else if ((pEXPRate < aRandom) && (aRandom < pBulletRate))
        {
            retType = LootTypeEnum.Bullet;
        }
        else if (pBulletRate < aRandom)
        {
            retType = LootTypeEnum.HPItem;
        }

        return retType;
    }
}
