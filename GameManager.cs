using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Dongle lastdongle;
    public GameObject donglePrefab;

    public Transform dongleGroup;

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
        GameObject instant = Instantiate(donglePrefab, dongleGroup);
        Dongle instantDongle = instant.GetComponent<Dongle>();
        return instantDongle;   // 2.#2[b]
    }

    void NextDongle()
    {
        Dongle newDongle = GetDongle();
        lastdongle = newDongle; // 2.#2[a]

        lastdongle.level = Random.Range(0, 8);
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
