using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingDirectionScript : MonoBehaviour
{
    public SpriteRenderer CharImage;
    public Rigidbody2D CharBody;

    private void Update()
    {
        var aSpd = CharBody.velocity;

        CharImage.flipX = (aSpd.x < 0);
    }
}
