using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Dongle lastdongle;
    public GameObject donglePrefab;
    public Transform dongleGroup;
    public GameObject effectPrefab;
    public Transform effectGroup;

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
    }

    void Start()
    {
        bgmPlayer.Play();
        NextDongle();
    }

    Dongle GetDongle()
    {
        GameObject instantE = Instantiate(effectPrefab, effectGroup);
        ParticleSystem instantEffect = instantE.GetComponent<ParticleSystem>();

        GameObject instant = Instantiate(donglePrefab, dongleGroup);
        Dongle instantDongle = instant.GetComponent<Dongle>();
        instantDongle.effect = instantEffect;
        return instantDongle;   // 2.#2[b]
    }

    void NextDongle()
    {
        if(isOver)
            return;

        Dongle newDongle = GetDongle();
        lastdongle = newDongle; // 2.#2[a]

        lastdongle.manager = this;

        lastdongle.level = Random.Range(0, maxLevel);
        lastdongle.gameObject.SetActive(true);  // 2.#4[g]

        SfxPlay(Sfx.Next);
        StartCoroutine(WaitNext());
    }

    IEnumerator WaitNext()
    {
        while(lastdongle != null) {
            yield return null;
        }

        yield return new WaitForSeconds(2.5f);

        NextDongle();   // 2.#3[b]
    }

    public void TouchDown()
    {
        if(lastdongle == null)  // 2.#2[g]
            return;

        lastdongle.Drag();
    }

    public void TouchUp()
    {
        if(lastdongle == null)
            return;

        lastdongle.Drop();
        lastdongle = null;
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
