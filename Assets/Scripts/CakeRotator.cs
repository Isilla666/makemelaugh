using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class CakeRotator : MonoBehaviour
{
    [SerializeField] private float startRotatePos;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> sprites;

    private void Awake()
    {
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Count)];
    }

    void Start()
    {
        transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-startRotatePos, startRotatePos));
        transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, Random.Range(-1,1)*startRotatePos), 1).Play();
    }
}
