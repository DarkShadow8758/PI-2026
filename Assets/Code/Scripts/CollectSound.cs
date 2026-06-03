using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectSound : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_AudioSource;
    [SerializeField]
    private AudioClip m_AudioClip;

    private void PlaySound ()
    {
        AudioSource audioSource = m_AudioSource;
        audioSource.clip = m_AudioClip;
        audioSource.Play();
    }
}
