using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GMScript : MonoBehaviour
{
    public CinemachineVirtualCamera GameCM;
    public UIScript GameUI;
    public GameObject JuingongPrefab;
    public GameObject LootPrefab;
    public int RestAreaLootCount = 2;

    public Transform BGRoot;
    public GameObject Lv1RestArea;

    [Header("휴게소 맵용 드랍 확률")]
    public float RestEXPRateMax = 30f;
    public float RestBulletRateMax = 80f;

    public bool IsInGame { set; get; }

    private ZombieManScript gZombie;
    private JuingongScript mJuingong;
    private Camera mGameCamera;
    public Camera GetGameCamera() { return mGameCamera; }
    private AudioSource mAudio;
    private GameObject mCurrentMap;

    Vector3 mouseRawVector;

    void CheckGZombie()
    {
        if (!gZombie) gZombie = GetComponent<ZombieManScript>();
    }

    private void Awake()
    {
        mGameCamera = FindObjectOfType<Camera>();
        mAudio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        GameUI.StartGameUI.SetActive(true);
        GameUI.GameOverUI.SetActive(false);

        mAudio.PlayOneShot(GDataScript.instance.GameStartClip);

        // 첫 맵 : 레벨1 휴게소 //FIXME: BGTile
        LoadMap(Lv1RestArea, PortalTypeEnum.Next, false);
    }

    public void LoadMap(GameObject pMapPrefab, PortalTypeEnum pFrom, bool isBattleField)
    {
        CheckGZombie();

        Destroy(mCurrentMap);
        gZombie.ClearAllZombies();
        gZombie.StopZombieLoop();

        mCurrentMap = Instantiate(pMapPrefab, BGRoot);
        var aMapData = mCurrentMap.GetComponent<MapDataScript>();
        // cinemachine setup
        var aConfiner = GameCM.GetComponent<CinemachineConfiner2D>();
        aConfiner.InvalidateCache();
        aConfiner.m_BoundingShape2D = aMapData.CameraZone;
        // zombie spawnpoint setup
        gZombie.SetSpawnPointArray(aMapData.ZombieSpawnPoints);
        gZombie.SetZombieCount(aMapData.ZombieCount);

        gZombie.RunZombieLoop();
        CreateMapLoot(aMapData);

        if (mJuingong)
        {
            var portals = mCurrentMap.GetComponentsInChildren<PortalScript>();
            //print("portal count " + portals.Length);
            foreach (var eachPortal in portals)
            {
                if (pFrom == PortalTypeEnum.Next)
                {
                    if (eachPortal.PortalType == PortalTypeEnum.Prev)
                    {
                        mJuingong.transform.position = eachPortal.StartPoint.position;
                        break;
                    }
                }
                else if (pFrom == PortalTypeEnum.Prev)
                {
                    if (eachPortal.PortalType == PortalTypeEnum.Next)
                    {
                        mJuingong.transform.position = eachPortal.StartPoint.position;
                        break;
                    }
                }
            }
        }
    }

    void CreateMapLoot(MapDataScript pMapData)
    {
        // 위치 중복 체크 필요
        for (int k = 0; k < pMapData.LootDropCount; k++)
        {
            var aLoot = Instantiate(LootPrefab);
            var spawnIndx = Random.Range(0, pMapData.LootSpawnPoints.Length);
            aLoot.transform.position = pMapData.LootSpawnPoints[spawnIndx].position;
            aLoot.GetComponent<LootScript>().SetupLootItem(GDataScript.instance.GetRandomLootType(RestEXPRateMax, RestBulletRateMax));
        }
    }

    public void StartGame()
    {
        CheckGZombie();
        gZombie.ClearAllZombies();

        var aJuingong = Instantiate(JuingongPrefab);
        mJuingong = aJuingong.GetComponent<JuingongScript>();
        {
            var weapons = mJuingong.GetData().Weapons;
            foreach (var eachWeap in weapons)
            {
                eachWeap.OnMagCapChanged += SetMagCapUI;
            }
            weapons[1].OnInvenCapChanged += SetMagCapUI;

        }
        GameCM.Follow = aJuingong.transform;
        IsInGame = true;
        SetMagCapUI();

        // 첫 맵이 휴게소일 경우 좀비 루프 돌리면 안된다
    }

    public void DoGameOver()
    {
        {
            var weapons = mJuingong.GetData().Weapons;
            foreach (var eachWeap in weapons)
            {
                eachWeap.OnMagCapChanged -= SetMagCapUI;
            }
            weapons[1].OnInvenCapChanged -= SetMagCapUI;
        }
        mAudio.PlayOneShot(GDataScript.instance.GameOverClip);
        IsInGame = false;
        StopAllCoroutines();
        GameUI.GameOverUI.SetActive(true);
    }

    public void SetJuingongSpeed(Vector2 pSpeed)
    {
        mJuingong.SetSpeed(pSpeed);
    }

    public Vector3 GetJuingongPos()
    {
        if (mJuingong == null) return Vector3.zero;

        return mJuingong.gameObject.transform.position;
    }

    public void PlayGlobalSound(AudioClip pClip)
    {
        mAudio.PlayOneShot(pClip);
    }

    public void SetMagCapUI()
    {
        var curWeapon = mJuingong.GetData().Weapons[mJuingong.WeaponIndex];
        var aInvenCap = curWeapon.GameInvenCap.ToString();
        if (curWeapon.InventoryCap == -1) aInvenCap = "--";
        mJuingong.BulletCount.text = curWeapon.GameMagCap.ToString() + "/" + curWeapon.MagazineCap.ToString() + "/" + aInvenCap;
    }

}
