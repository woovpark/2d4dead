using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GMScript : MonoBehaviour
{
    public CinemachineVirtualCamera GameCM;
    public UIScript GameUI;
    public GameObject ZombiePrefab;
    public GameObject JuingongPrefab;

    public Transform BGRoot;
    public GameObject BGTile;

    public int ZombieCount = 10;

    public bool IsInGame { set; get; }

    private JuingongScript mJuingong;
    private Camera mGameCamera;
    public Camera GetGameCamera() { return mGameCamera; }
    private AudioSource mAudio;

    Vector3 mouseRawVector;

    private void Awake()
    {
        mGameCamera = FindObjectOfType<Camera>();
        mAudio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        GameUI.StartGameUI.SetActive(true);
        GameUI.GameOverUI.SetActive(false);
        //StartCoroutine(GameLoop());
        mAudio.PlayOneShot(GDataScript.instance.GameStartClip);

        LoadMap(BGTile);
    }

    void LoadMap(GameObject pMapPrefab)
    {
        var aMap = Instantiate(pMapPrefab, BGRoot);
        var aColl = aMap.GetComponent<Collider2D>();
        var aConfiner = GameCM.GetComponent<CinemachineConfiner2D>();
        aConfiner.InvalidateCache();
        aConfiner.m_BoundingShape2D = aColl;
    }

    public void StartGame()
    {
        var zombies = FindObjectsOfType<ZombieScript>();
        foreach (var eachZombie in zombies)
        {
            Destroy(eachZombie.gameObject);
        }

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
        StartCoroutine(GameLoop());
        IsInGame = true;
        SetMagCapUI();
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

    IEnumerator GameLoop()
    {
        while (true)
        {
            var zombies = FindObjectsOfType<ZombieScript>();
            if (zombies.Length < ZombieCount)
            {
                var aZombie = Instantiate(ZombiePrefab);
                var aX = Random.Range(-10f, 10f);
                var aY = Random.Range(-10f, 10f);
                aZombie.transform.position = new Vector3(aX, aY, 0);
            }
            yield return new WaitForSeconds(1f);
        }
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
        GameUI.BulletCount.text = curWeapon.GameMagCap.ToString() + "/" + curWeapon.MagazineCap.ToString() + "/" + aInvenCap;
    }
}
