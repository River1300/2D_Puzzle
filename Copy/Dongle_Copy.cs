using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dongle_Copy : MonoBehaviour
{
    public int level;
    public bool isDrag;

    public Rigidbody2D rigid;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        anim.SetInteger("Level", level);
    }

    void Update()
    {
        if(isDrag)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float lSide = -4.2f + transform.localScale.x / 2;
            float rSide = 4.2f - transform.localScale.x / 2;
            if(mousePos.x < lSide)
                mousePos.x = lSide;
            else if(mousePos.x > rSide)
                mousePos.x = rSide;

            mousePos.z = 0;
            mousePos.y = 8;

            transform.position = Vector3.Lerp(transform.position, mousePos, 0.1f);
        }
    }

    public void Drag()
    {
        isDrag = true;
    }

    public void Drop()
    {
        isDrag = false;
        rigid.simulated = true;
    }
}
