using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 2디4 데드

// Unity Input System asset 적용 OK
// 1 주인공을 움직인다 OK
// 2 좀비를 동적으로 생성시킨다 (게임루프 필요) OK
// 3 주인공과 좀비의 draw order 정리한다 OK
// 3.5 좀비의 동적생성 갯수가 제한되도록 (일명 산아제한) OK
// 4 좀비가 주인공을 향해 이동한다 OK
// 4.5 좀비가 안뭉치게 만든다 OK
// 4.6 마우스 키보드 컨트롤 구현 OK
// 4.6.5 이미지 적용 OK
// 4.6.6 바닥 이미지 적용 OK
// 4.6.6 좀비만 진행 방향을 바라보게 만든다 OK
// 4.7 키보드로 움직이게 만든다 OK
// 4.8 마우스로 조준한다 OK
// 4.8.5 주인공은 마우스를 바라본다 OK
// 5 주인공이 총을 쏜다 OK
// 5.1 총알 프리팹 (rigidbody, collider) OK
// 5.2 마우스로 발사 방향 지정 (AimVec) OK
// 5.3 클릭시 발사되게 만든다 OK
// 6 좀비가 총을 맞는다 OK
// 6.1 좀비에게 체력 시스템을 달아 준다 OK
// 6.2 총알과 좀비가 만나면 체력을 깎는다. OK
// 6.3 좀비의 체력이 0이 되면 사라진다 OK
// 6.4 좀비에게 체력 게이지를 달아준다 OK
// 6.5 좀비가 맞았을 때 깜박이게 만든다 OK
// 7 주인공 성장 시스템 만들기 OK
// 7.1 체력시스템 OK
// 7.1.5 체력회복 아이템을 먹어서 회복함 OK
// 7.1.6 사망 구현 (GameOver) OK
// 7.1.6.5 게임의 시작(Start Game)과 끝(Game Over)을 구현 OK
// 7.2 경험치 및 레벨 / 공격력 OK
// 7.2.1 레벨에 맞춘 경험치 공식 OK, 드랍경험치 공식, HP, 공격력 공식 (주인공과 좀비 따로) 작성 OK
// 7.2.2 대미지 공식 작성 (총 종류, 플레이어 공격력, 좀비의 HP) OK
// 7.2.5 경험치 아이템을 먹어서 성장함 OK
// 7.2.9 총 종류 바꿀 수 있도록 작업 - OK
// 7.3 총알이 소모되도록 하고 총알 아이템을 먹어서 보충하도록 한다 OK
// 7.4 총알 갯수 UI 를 주인공에게 붙여 준다

// ?.4 리로드를 구현한다 OK
// ?.4.1 리로딩 비주얼 실제로 구현 OK

// 8 좀비의 리스폰 패턴을 개선한다 - 글로벌 좀비 관리 스크립트 생성
// 8.1 좀비가 카메라 밖에서 생성되어 다가 오도록 한다 - 반만 OK - 맵 디자인 수정필요
// 8.2 맵의 충돌영역에 갖히지 않도록 한다 OK

// 9 맵 기본 메카닉 제작
// 9.1 입/출구는 바리케이드에 막혀있음
// 9.2 맵에 들어 왔을때 타이머가 작동하여 5분후에 바리케이드가 폭발하여 출구가 노출된다.
// 9.3 휴게소/전투 공용 맵 다수 만들어서 게임 플레이 할 때 마다 랜덤하게 맵을 로드한다 - 맵관리자가 프리팹을 관리하도록
// 9.4 휴 - 전 - 휴 - 전 의 패턴을 구현 OK / 마지막 스테이지를 결정(전10에서 탈출? 휴11에서 탈출?)
// 9.5 공용 맵에 아이템 스폰 포인트를 여러개 만들어 골고루 뿌려 놓는다 OK

// 10 아이템 루팅 시스템 OK
// 10.0.1 루팅 아이템 프리팹 만들기 - 경험치 아이템, 소총 총알 아이템(총기), 체력회복 아이템 OK

// 10.1 다양한 총기 - 권총과 소총
// 10.2 소총 탄환 수집 / 소지 갯수 제한 / 탄창(리로드) 구현 OK
// 10.3 체력회복 아이템 - 습득시 즉시 회복 OK



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
