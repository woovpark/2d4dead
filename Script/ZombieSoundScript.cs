using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSoundScript : MonoBehaviour
{
    private AudioSource mAudio;
    private ZombieDataScript mData;

    private void Awake()
    {
        mAudio = GetComponent<AudioSource>();
        mData = GetComponent<ZombieDataScript>();
    }

    private void Start()
    {
        mAudio.PlayOneShot(mData.AppearSound);
    }
    
    public void HitSound()
    {
        mAudio.PlayOneShot(mData.HitSound);
    }
}
