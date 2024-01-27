using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class DogAnimator : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private ParticleSystem part;
    
    private void Awake()
    {
        var main = part.main;
        main.startColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        StartCoroutine(DoIt());
    }

    IEnumerator DoIt()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
