using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dongle : MonoBehaviour
{
    public GameManager manager;
    public ParticleSystem effect;

    public int level;
    public bool isDrag;
    public bool isMerge;

    public Rigidbody2D rigid;
    CircleCollider2D circle;
    Animator anim;
    SpriteRenderer spriteRenderer;

    float deadTime;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

    void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.tag == "Dongle")
        {
            Dongle otherD = other.gameObject.GetComponent<Dongle>();

            if(level == otherD.level && !isMerge && !otherD.isMerge && level < 7)
            {   // A는 자신의 위치에서 커져야 하고 B는 A를 향해 가다가 사라져야 한다.
                float meX = transform.position.x;
                float meY = transform.position.y;
                float otherX = other.transform.position.x;
                float otherY = other.transform.position.y;
                if(meY < otherY || (meY == otherY && meX > otherX))
                {   // A는 커지는 함수를 B는 사라지는 함수를 각각 호출
                    otherD.Hide(transform.position);
                    LevelUp();
                }
            }
        }
    }

    public void Hide(Vector3 targetPos)
    {
        isMerge = true;

        rigid.simulated = false;
        circle.enabled = false;

        if(targetPos == Vector3.up * 100)
        {
            EffectPlay();
        }

        StartCoroutine(HideRoutine(targetPos));
    }

    IEnumerator HideRoutine(Vector3 targetPos)
    {
        int frameCount = 0;

        while(frameCount < 20)
        {   // B는 매 프레임 마다 조금씩 A를 향해 다가간다.
            frameCount++;
            if(targetPos != Vector3.up * 100){
                transform.position = Vector3.Lerp(transform.position, targetPos, 0.5f);
            }
            else if(targetPos == Vector3.up * 100){
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.2f);
            }
            yield return null;
        }

        manager.score += (int)Mathf.Pow(2, level);

        isMerge = false;
        gameObject.SetActive(false);
    }

    void LevelUp()
    {
        isMerge = true;

        rigid.velocity = Vector2.zero;
        rigid.angularVelocity = 0;

        StartCoroutine(LevelUpRoutine());
    }

    IEnumerator LevelUpRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        anim.SetInteger("Level", level + 1);
        EffectPlay();

        yield return new WaitForSeconds(0.3f);

        level++;
        manager.maxLevel = Mathf.Max(level, manager.maxLevel);
        isMerge = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Line")
        {
            deadTime += Time.deltaTime;

            if(deadTime > 2){
                spriteRenderer.color = Color.red;
            }
            if(deadTime > 5){
                manager.GameOver();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Line")
        {
            deadTime = 0;
            spriteRenderer.color = Color.white;
        }
    }

    void EffectPlay()
    {
        effect.transform.position = transform.position;
        effect.transform.localScale = transform.localScale;
        effect.Play();
    }
}
