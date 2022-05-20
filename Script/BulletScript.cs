using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Rigidbody2D mBody;
    public float LifeTime = 1;
    public float BulletSpeed = 50;

    private BulletDataScript mData;
    private AudioSource mAudio;

    private void Awake()
    {
        mData = GetComponent<BulletDataScript>();
        mAudio = GetComponent<AudioSource>();
    }
    private void Start()
    {
        mAudio.PlayOneShot(mData.ShotSound);
        StartCoroutine(LifeRoutine());
    }

    public void SetVelocity(Vector2 pVel)
    {
        mBody.velocity = pVel * BulletSpeed;
    }

    IEnumerator LifeRoutine()
    {
        yield return new WaitForSeconds(LifeTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            Destroy(gameObject);
            return;
        }

        if (collision.tag != "Enemy") return;
        
        var zombieData = collision.gameObject.GetComponent<ZombieDataScript>();
        zombieData.GetHit(mData.Power);
        Destroy(gameObject);
    }
}
