using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawOrderScript : MonoBehaviour
{
    public SpriteRenderer CharImage;

    private void Update()
    {
        var orderY = - transform.position.y * 10;
        CharImage.sortingOrder = (int)orderY;
    }
}
