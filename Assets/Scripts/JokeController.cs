using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokeController : MonoBehaviour
{
    [SerializeField] private AudioClip vnimanie;
    [SerializeField] private List<AudioClip> jokes;
    [SerializeField] private AudioSource audioSource;
    public float EndTime => vnimanie.length + jokes[_currentJoke].length + 1f;
    private static int _currentJoke;

    private void Start()
    {
        StartCoroutine(DoIt());
    }

    private IEnumerator DoIt()
    {
        audioSource.clip = vnimanie;
        audioSource.Play();
        yield return new WaitForSeconds(vnimanie.length);
        audioSource.clip = jokes[_currentJoke];
        audioSource.Play();
        _currentJoke++;
        _currentJoke %= jokes.Count;
    }
}
