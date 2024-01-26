using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CakeRotator : MonoBehaviour
{
    [SerializeField] private float startRotatePos;

    void Start()
    {
        transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-startRotatePos, startRotatePos));
        transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, Random.Range(-1,1)*startRotatePos), 1).Play();
    }
}
