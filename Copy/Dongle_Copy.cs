using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dongle_Copy : MonoBehaviour
{
    public GameManager manager;

    public int level;
    public bool isDrag;
    public bool isMerge;

    public Rigidbody2D rigid;
    CircleCollider2D circle;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();
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

    void OnCollisionStay2D(Collision2D other)
    {
        Dongle otherD = other.gameObject.GetComponent<Dongle>();

        if(level == otherD.level && !isMerge && !otherD.isMerge && level < 7)
        {
            float meX = transform.position.x;
            float meY = transform.position.y;
            float otherX = otherD.transform.position.x;
            float otherY = otherD.transform.position.y;
            if(meY < otherY || (meY == otherY && meX > otherX))
            {

            }
        }
    }

    public void Hide(Vector3 targetPos)
    {
        isMerge = true;
        rigid.simulated = false;
        circle.enabled = false;

        StartCoroutine(HideRoutine(targetPos));
    }

    IEnumerator HideRoutine(Vector3 targetPos)
    {
        int frameCount = 0;
        while(frameCount < 20)
        {
            frameCount++;
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.5f);
            yield return null;
        }
        isMerge = false;
        gameObject.SetActive(false);
    }

    public void LevelUp()
    {
        isMerge = true;
        rigid.velocity = Vector2.zero;
        rigid.angularVelocity = 0;

        StartCoroutine(LevelUpRoutine());
    }

    IEnumerator LevelUpRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        anim.SetInteger("Level", level);

        yield return new WaitForSeconds(0.3f);

        level++;
        manager.maxLevel = Mathf.Max(level, manager.maxLevel);
        isMerge = false;
    }
}
