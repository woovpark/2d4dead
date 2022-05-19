using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 2디4 데드

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
// 7 주인공 성장 시스템 만들기
// 7.1 체력시스템 OK
// 7.1.5 체력회복 아이템을 먹어서 회복함 OK
// 7.1.6 사망 구현 (GameOver) OK
// 7.1.6.5 게임의 시작(Start Game)과 끝(Game Over)을 구현 OK
// 7.2 경험치 및 레벨 / 공격력 방어력 - 반만 오케이
// 7.2.1 레벨에 맞춘 경험치 공식 OK, 드랍경험치 공식, HP, 공격력, 방어력 공식 (주인공과 좀비 따로) 작성
// 7.2.2 대미지 공식 작성 (총 종류, 플레이어 공격력, 좀비의 방어력) 총알 자체 파워는 제거 - 총을 중심으로 고려하는 걸로 수정
// 7.2.5 경험치 아이템을 먹어서 성장함 OK
// 7.2.9 총 종류 바꿀 수 있도록 작업 - 여러가지 오케이 - weaponindex 를 활용하여 array 에서 정보 참조 / 기능도 분화
// 7.3 총알이 소모되도록 하고 총알 아이템을 먹어서 보충하도록 한다
// 7.4 리로드를 구현한다
// 7.4.1 총알의 총 소지 수 지정, 및 탄창 시스템 구현
// 7.4.2 리로드 키를 누르면 바로 리로딩
// 7.4.3 리로딩 중에 무기를 바꾸면 리로딩 취소(탄창은 0)
// 7.4.4 리로딩 중에는 발사버튼의 누름 여부에 상관없이 계속 된다 / 리로딩 종료 후에 버튼 체크하여 다시 발사 여부 확인 OK
// 7.4.5 트리거 스크립트의 상태에 따라 "준비완료"중에 버튼 누름뗌 -> 발사 계속 하고 안하고 // "리로딩중" -> 버튼 누름뗌 의미 없음 OK
// 7.4.5 권총(무한 총알의 경우)먼저 구현 OK / 소총(총알 제한의 경우) 순서로 개발
// 7.4.6 리로딩 비주얼 실제로 구현

// 8 좀비의 리스폰 패턴을 개선한다
// 8.1 좀비가 카메라 밖에서 생성되어 다가 오도록 한다

// 10 아이템 루팅 시스템 OK
// 10.0.1 루팅 아이템 프리팹 만들기 - 경험치 아이템, 총알 아이템, 체력회복 아이템, 총기 아이템

// 10.1 다양한 총기 - 권총과 소총
// 10.2 소총 탄환 수집 / 소지 갯수 제한 / 탄창(리로드) 구현
// 10.3 체력회복 아이템 - 습득시 즉시 회복 OK
// 
// 20 좀비 리스폰 시스템 고민 ->맵디자인과 연계하여 고려

// ?? Unity Input System asset 적용

public class GDataScript : MonoBehaviour
{
    static public GDataScript instance;

    public float ZombieInitialEXP = 3f;
    public float PistolPower = 2.5f;
    public int PistolRPS = 3;
    [Space]
    public AudioClip GameStartClip;
    public AudioClip GameOverClip;

    private GMScript mGM;

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

    public float GetEXPToUp(int pLv)
    {
        return pLv * pLv * 150f;
    }
}
