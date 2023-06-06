using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Copy : MonoBehaviour
{
    public GameObject donglePrefab;
    public Transform dongleGroup;
    public List<Dongle> donglePool;
    public GameObject effectPrefab;
    public Transform effectGroup;
    public List<ParticleSystem> effectPool;
    [Range(1, 30)]
    public int poolSize;
    public int poolCursor;
    public Dongle lastDongle;

    public AudioSource bgmPlayer;
    public AudioSource[] sfxPlayer;
    public AudioClip[] sfxClip;
    public enum Sfx { LevelUp, Next, Attach, Button, Over };
    int sfxCursor;

    public int score;
    public int maxLevel;
    public bool isOver;

    void Awake()
    {
        Application.targetFrameRate = 60;

        donglePool = new List<Dongle>();
        effectPool = new List<ParticleSystem>();

        for(int i = 0; i < poolSize; i++)
        {
            MakeDongle();
        }
    }

    void Start()
    {
        bgmPlayer.Play();
        NextDongle();
    }

    Dongle MakeDongle()
    {
        GameObject instantE = Instantiate(effectPrefab, effectGroup);
        instantE.name = "Effect " + effectPool.Count;
        ParticleSystem tempE = instantE.GetComponent<ParticleSystem>();
        effectPool.Add(tempE);

        GameObject instant = Instantiate(donglePrefab, dongleGroup);
        instant.name = "Dongle " + donglePool.Count;
        Dongle tempDongle = instant.GetComponent<Dongle>();
        //tempDongle.manager = this;
        tempDongle.effect = tempE;
        donglePool.Add(tempDongle);
        return tempDongle;
    }

    Dongle GetDongle()
    {
        for(int i = 0; i < poolSize; i++)
        {
            poolCursor = (poolCursor + 1) % donglePool.Count;

            if(!donglePool[poolCursor].gameObject.activeSelf)
            {
                return donglePool[poolCursor];
            }
        }
        return MakeDongle();
    }

    void NextDongle()
    {
        if(isOver)
            return;

        lastDongle = GetDongle();

        lastDongle.level = Random.Range(0, maxLevel);
        lastDongle.gameObject.SetActive(true);
        SfxPlay(Sfx.Next);

        StartCoroutine(WaitNext());
    }

    IEnumerator WaitNext()
    {
        while(lastDongle != null) {
            yield return null;
        }

        yield return new WaitForSeconds(2.5f);

        NextDongle();
    }

    public void TouchDown()
    {
        if(lastDongle == null)
            return;

        lastDongle.Drag();
    }

    public void TouchUp()
    {
        if(lastDongle == null)
            return;

        lastDongle.Drop();
        lastDongle = null;
    }

    public void GameOver()
    {
        if(isOver)
            return;
        isOver = true;

        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        Dongle[] dongles = FindObjectsOfType<Dongle>();

        for(int i = 0; i < dongles.Length; i++)
        {
            dongles[i].rigid.simulated = false;
        }
        for(int i = 0; i < dongles.Length; i++)
        {
            dongles[i].Hide(Vector3.up * 100);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);
        SfxPlay(Sfx.Over);
    }

    public void SfxPlay(Sfx type)
    {
        switch(type)
        {
            case Sfx.LevelUp:
                sfxPlayer[sfxCursor].clip = sfxClip[Random.Range(0, 3)];
                break;
            case Sfx.Next:
                sfxPlayer[sfxCursor].clip = sfxClip[3];
                break;
            case Sfx.Attach:
                sfxPlayer[sfxCursor].clip = sfxClip[4];
                break;
            case Sfx.Button:
                sfxPlayer[sfxCursor].clip = sfxClip[5];
                break;
            case Sfx.Over:
                sfxPlayer[sfxCursor].clip = sfxClip[6];
                break;
        }

        sfxPlayer[sfxCursor].Play();
        sfxCursor = (sfxCursor + 1) % sfxPlayer.Length;
    }
}
