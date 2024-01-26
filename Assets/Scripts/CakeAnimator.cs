using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class CakeAnimator : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private ParticleSystem puff;

    [SerializeField] private GameObject cakePrefab;

    [SerializeField] private float timeThrow;


    private Coroutine _coCakeThrow;
    
    [Button]
    public void StartAnimation()
    {
        StartCoroutine(DoItCake());
    }

    
    private IEnumerator DoItCake()
    {
        var cake = Instantiate(cakePrefab, transform, false);
        var time = timeThrow;
        var startPos = cake.transform.position;
        while (time>=0f)
        {
            
            var dir = cake.transform.position - target.position;
            dir = new Vector3(-dir.y, dir.x, 0);
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            cake.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            cake.transform.position = Vector3.Lerp(startPos, target.position, 1f - time/timeThrow);
            time -= Time.deltaTime;
            yield return null;
        }
        var part = Instantiate(puff, transform, false);
        part.transform.position = cake.transform.position;
        Destroy(cake);
        yield return new WaitForSeconds(2f);
        Destroy(part.gameObject);
    }
}
