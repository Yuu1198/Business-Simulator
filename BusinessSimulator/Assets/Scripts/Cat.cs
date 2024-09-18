using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] meowClips;

    private void OnMouseDown()
    {
        AudioClip randomMeowClip = meowClips[Random.Range(0, meowClips.Length)];
        audioSource.PlayOneShot(randomMeowClip);
    }
}
