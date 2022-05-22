using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JuingongControlScript : MonoBehaviour
{
    public float MoveSpeed = 10;
    private GMScript gGM;
    private JuingongScript mJuingong;
    private Vector2 mMoveVec;

    [HideInInspector]
    public Vector2 AimVector = Vector2.zero;

    Vector3 mouseRawVector; // �����ǥ

    private void Awake()
    {
        gGM = GDataScript.instance.GetGM();
        mJuingong = GetComponent<JuingongScript>();
    }

    void OnMove(InputValue pInput)
    {
        mMoveVec = pInput.Get<Vector2>();
    }

    void OnFire(InputValue pInput)
    {
        //print("get float " + pInput.Get<float>());
        //print("ispressed " + pInput.isPressed);

        if (pInput.isPressed)
        {
            mJuingong.ButtonDown();
        }
        else
        {
            mJuingong.ButtonUp();
        }
    }

    void OnChangePistol()
    {
        mJuingong.ChangeWeapon(0);
    }
    void OnChangeRifle()
    {
        mJuingong.ChangeWeapon(1);
    }

    private void Update()
    {
        var mouseWorldVec = gGM.GetGameCamera().ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if (mouseRawVector != mouseWorldVec)
        {
            mouseRawVector = mouseWorldVec;
            AimVector = mouseRawVector - mJuingong.gameObject.transform.position;

            //Debug.Log(mouseWorldVec.x.ToString("0.0") + " " + mouseWorldVec.y.ToString("0.0"));
        }
    }

    private void FixedUpdate()
    {
        if (!gGM.IsInGame) return;

        mJuingong.SetSpeed(mMoveVec * MoveSpeed);
    }
}
