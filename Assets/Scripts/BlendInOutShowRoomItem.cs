using System;
using System.Collections;
using UnityEngine;

public class BlendInOutShowRoomItem : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjectsToShowUnshow;
    [SerializeField] private float timeBetweenItmes;
    private int counter;
    [SerializeField] private bool loop;


    private void Start()
    {
        loop = true;
        counter = 1;

        StartCoroutine(ShowItems());
    }

    private IEnumerator ShowItems()
    {
        while(loop)
        {
            foreach (GameObject index in gameObjectsToShowUnshow)
            {
                index.SetActive(true);
                yield return new WaitForSeconds(timeBetweenItmes);
                index.SetActive(false);
            }

        } 
    }

}
