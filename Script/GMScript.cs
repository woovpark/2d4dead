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

    [HideInInspector]
    public Vector2 AimVector = Vector2.zero;

    private Vector2 ControlVector = Vector2.zero;

    private bool mIsInGame = false;

    private JuingongScript mJuingong;
    private Camera gameCamera;
    private AudioSource mAudio;

    Vector3 mouseRawVector;

    private void Awake()
    {
        gameCamera = FindObjectOfType<Camera>();
        mAudio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        GameUI.StartGameUI.SetActive(true);
        GameUI.GameOverUI.SetActive(false);
        //StartCoroutine(GameLoop());
        mAudio.PlayOneShot(GDataScript.instance.GameStartClip);

        Instantiate(BGTile, BGRoot);
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
        GameCM.Follow = aJuingong.transform;
        StartCoroutine(GameLoop());
        mIsInGame = true;
    }

    public void DoGameOver()
    {
        mAudio.PlayOneShot(GDataScript.instance.GameOverClip);
        ControlVector = Vector2.zero;
        mIsInGame = false;
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

    private void Update()
    {
        if (!mIsInGame) return;

        if (Input.GetKeyDown(KeyCode.W))
        {
            ControlVector += new Vector2(0, 10);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            ControlVector -= new Vector2(0, 10);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ControlVector += new Vector2(0, -10);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            ControlVector -= new Vector2(0, -10);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            ControlVector += new Vector2(-10, 0);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            ControlVector -= new Vector2(-10, 0);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            ControlVector += new Vector2(10, 0);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            ControlVector -= new Vector2(10, 0);
        }

        SetJuingongSpeed(ControlVector);

        if (Input.GetKeyDown(KeyCode.E))
        {

        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            mJuingong.ChangeWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            mJuingong.ChangeWeapon(1);
        }

        // mouse
        if (mJuingong != null)
        {
            var mouseWorldVec = gameCamera.ScreenToWorldPoint(Input.mousePosition);
            if (mouseRawVector != mouseWorldVec)
            {
                mouseRawVector = mouseWorldVec;
                AimVector = mouseRawVector - mJuingong.gameObject.transform.position;

                //Debug.Log(gameCamera.ScreenToWorldPoint(mouseRawVector)); // ¼º°ø
            }

            if (Input.GetMouseButtonDown(0))
            {
                mJuingong.ButtonDown();
            }

            if (Input.GetMouseButtonUp(0))
            {
                mJuingong.ButtonUp();
            }
        }
    }

    public void PlayGlobalSound(AudioClip pClip)
    {
        mAudio.PlayOneShot(pClip);
    }
}
