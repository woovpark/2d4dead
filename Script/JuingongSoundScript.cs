using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuingongSoundScript : MonoBehaviour
{
    public AudioClip FootStepClip;

    private AudioSource mAudio;
    private Rigidbody2D mBody;

    bool mIsMoving = false;
    bool mPrevMoving = false;

    private void Awake()
    {
        mAudio = GetComponent<AudioSource>();
        mBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        mIsMoving = (mBody.velocity != Vector2.zero);
        if (mIsMoving != mPrevMoving)
        {
            if (mIsMoving)
            {
                StopAllCoroutines();
                StartFootStep();
            }
            else
            {
                StopAllCoroutines();
            }
            mPrevMoving = mIsMoving;
        }
    }

    public void StartFootStep()
    {
        StartCoroutine(FootStepRoutine());
    }

    public void StopFootStep()
    {
        StopAllCoroutines();
    }

    IEnumerator FootStepRoutine()
    {
        while (true)
        {
            mAudio.pitch = Random.Range(0.9f, 1.05f);
            mAudio.clip = FootStepClip;
            mAudio.volume = 0.5f;
            mAudio.Play();
            yield return new WaitForSeconds(0.3f);
        }
    }
}
