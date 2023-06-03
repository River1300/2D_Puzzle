using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Copy : MonoBehaviour
{
    public Dongle lastDongle;
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
        ParticleSystem tempE = instantE.GetComponent<ParticleSystem>();

        GameObject instant = Instantiate(donglePrefab, dongleGroup);
        Dongle tempDongle = instant.GetComponent<Dongle>();
        //tempE.effect = this;
        return tempDongle;
    }

    void NextDongle()
    {
        Dongle newDongle = GetDongle();
        lastDongle = newDongle;

        // lastDongle.manager = this;

        lastDongle.level = Random.Range(0, maxLevel);
        lastDongle.gameObject.SetActive(true);

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
}
