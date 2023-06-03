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

    public int maxLevel;

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
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
        Dongle newDongle = GetDongle();
        lastdongle = newDongle; // 2.#2[a]

        lastdongle.manager = this;

        lastdongle.level = Random.Range(0, maxLevel);
        lastdongle.gameObject.SetActive(true);  // 2.#4[g]

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
}
