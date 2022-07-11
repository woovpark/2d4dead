using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour
{
    private GMScript gGM;
    private MapManScript gMap;
    private List<GameObject> mBarricades = new List<GameObject>();

    private void Awake()
    {
        gGM = GDataScript.instance.GetGM();
        gMap = GDataScript.instance.GetMap();

        if (gMap.IsBattleField)
        {
            var aPortals = GetComponentsInChildren<PortalScript>();
            foreach (var eachPortal in aPortals)
            {
                var aBarri = Instantiate(gMap.PortalBarricade);
                aBarri.transform.position = eachPortal.transform.position;
                mBarricades.Add(aBarri);
            }
        }
    }

    private void Start()
    {
        if (gMap.IsBattleField) StartCoroutine(MapTimerRoutine());
        else gGM.GameUI.MapTimer.gameObject.SetActive(false);
    }

    IEnumerator MapTimerRoutine()
    {
        gGM.GameUI.MapTimer.gameObject.SetActive(true);
        float aTime = gMap.MapTimeSec;

        while (aTime > 0)
        {
            aTime -= Time.deltaTime;
            var aMin = (int)(aTime / 60);
            var aSec = (int)aTime % 60;

            gGM.GameUI.MapTimer.text = aMin.ToString("00") + ":" + aSec.ToString("00");

            yield return null;
        }
        gGM.GameUI.MapTimer.gameObject.SetActive(false);

        {
            var zombies = FindObjectsOfType<ZombieScript>();
            foreach (var zombie in zombies)
            {
                Destroy(zombie.gameObject);
            }
        }

        GDataScript.instance.GetComponent<ZombieManScript>().StopZombieLoop();

        foreach (var eachBari in mBarricades)
        {
            Destroy(eachBari);
        }
    }
}
