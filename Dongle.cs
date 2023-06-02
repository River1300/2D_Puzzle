using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dongle : MonoBehaviour
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
        anim.SetInteger("Level", level);    // 2.#4[e]
    }

    void Update()
    {
        if(isDrag)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 1.#4[c]

            float leftBoarder = -4.2f + transform.localScale.x / 2; // 1.#4[h]
            float rightBoarder = 4.2f - transform.localScale.x / 2;

            if(mousePos.x < leftBoarder){
                mousePos.x = leftBoarder;
            }
            else if(mousePos.x > rightBoarder){
                mousePos.x = rightBoarder;
            }

            mousePos.y = 8;
            mousePos.z = 0; // 1.#4[f]
            transform.position = Vector3.Lerp(transform.position, mousePos, 0.1f); // 1.#4[g]
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
