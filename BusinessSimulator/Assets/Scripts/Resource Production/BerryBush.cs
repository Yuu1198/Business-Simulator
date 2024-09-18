using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryBush : MonoBehaviour
{
    public int berryAmount = 10;

    public AudioSource audioSource;
    public AudioClip collectClip;

    // Player Input
    private void OnMouseDown()
    {
        CollectBerries();
    }

    private void CollectBerries()
    {
        GameManager.Instance.CollectBerries(this);
    }
}
