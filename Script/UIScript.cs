using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public GameObject StartGameUI;
    public GameObject GameOverUI;

    public Slider ReloadGauge;
    public Text BulletCount;

    private GMScript gGM;

    private void Awake()
    {
        gGM = GDataScript.instance.GetGM();
        ReloadGauge.value = 100;
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

    public void SetReloadGauge(float pPercent)
    {
        ReloadGauge.value = pPercent;
    }
}
