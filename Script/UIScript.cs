using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public GameObject StartGameUI;
    public GameObject GameOverUI;

    public Text BulletCount;

    private GMScript gGM;

    private void Awake()
    {
        gGM = GDataScript.instance.GetGM();
    }
    public void CallbackStartGame()
    {
        StartGameUI.SetActive(false);
        gGM.StartGame();
    }

    public void CallbackRestart()
    {
        GameOverUI.SetActive(false);
        gGM.StartGame();
    }

    public void CallbackOnCheatToggle(bool pIsOn)
    {
        GDataScript.instance.Cheat = pIsOn;
    }

    /*
    public void DevSetRot(float pFloat)
    {
        gGM.DevSetRot(pFloat);
    }*/
}
