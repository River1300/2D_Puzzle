using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("-----Core")]
    public bool isOver;
    public int score;
    public int maxLevel;
    [Header("-----ObjectPooling")]
    public GameObject donglePrefab;
    public Transform dongleGroup;
    public List<Dongle> donglePool;
    public GameObject effectPrefab;
    public Transform effectGroup;
    public List<ParticleSystem> effectPool;
    [Range(1, 30)]
    public int poolSize;
    public int poolCursor;
    public Dongle lastdongle;
    [Header("-----Sound")]
    public AudioSource bgmPlayer;
    public AudioSource[] sfxPlayer;
    public AudioClip[] sfxClip;
    public enum Sfx { LevelUp, Next, Attach, Button, Over };
    int sfxCursor;
    [Header("-----UI")]
    public GameObject startGroup;
    public GameObject endGroup;
    public Text scoreTxt;
    public Text maxScoreTxt;
    public Text subScoreTxt;
    [Header("-----ETC")]
    public GameObject line;
    public GameObject bottom;

    void Awake()
    {
        Application.targetFrameRate = 60;

        donglePool = new List<Dongle>();
        effectPool = new List<ParticleSystem>();

        for(int i = 0; i < poolSize; i++)
        {
            MakeDongle();
        }

        if(!PlayerPrefs.HasKey("MaxScore"))
            PlayerPrefs.SetInt("MaxScore", 0);
        maxScoreTxt.text = PlayerPrefs.GetInt("MaxScore").ToString();
    }

    public void GameStart()
    {
        line.SetActive(true);
        bottom.SetActive(true);
        scoreTxt.gameObject.SetActive(true);
        maxScoreTxt.gameObject.SetActive(true);
        startGroup.SetActive(false);

        bgmPlayer.Play();
        SfxPlay(Sfx.Button);

        Invoke("NextDongle", 1.5f);
    }

    Dongle MakeDongle()
    {
        GameObject instantE = Instantiate(effectPrefab, effectGroup);
        instantE.name = "Effect " + effectPool.Count;
        ParticleSystem instantEffect = instantE.GetComponent<ParticleSystem>();
        effectPool.Add(instantEffect);

        GameObject instant = Instantiate(donglePrefab, dongleGroup);
        instant.name = "Dongle " + donglePool.Count;
        Dongle instantDongle = instant.GetComponent<Dongle>();
        instantDongle.manager = this;
        instantDongle.effect = instantEffect;
        donglePool.Add(instantDongle);

        return instantDongle;
    }

    Dongle GetDongle()
    {
        for(int i = 0 ; i < donglePool.Count; i++)
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

        lastdongle = GetDongle();

        lastdongle.level = Random.Range(0, maxLevel);
        lastdongle.gameObject.SetActive(true);

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

        int maxScore = Mathf.Max(score, PlayerPrefs.GetInt("MaxScore"));
        PlayerPrefs.SetInt("MaxScore", maxScore);

        subScoreTxt.text = "점수 : " + scoreTxt.text;
        endGroup.SetActive(true);

        bgmPlayer.Stop();
        SfxPlay(Sfx.Over);
    }

    public void Restart()
    {
        SfxPlay(Sfx.Button);
        StartCoroutine(ResetCoroutine());
    }

    IEnumerator ResetCoroutine()
    {
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(0);
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

    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
            Application.Quit();
    }

    void LateUpdate()
    {
        scoreTxt.text = score.ToString();
    }
}
