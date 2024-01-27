using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject discoPrefab;
    [SerializeField] private JokeController jokePrefab;
    [SerializeField] private GameObject dogPrefab;
    [SerializeField] private GameObject petpetPrefab;
    [SerializeField] private Transform petpetTarget;

    [Button]
    private void SpawnDisco()
    {
        StartCoroutine(DoDisco());
    }

    IEnumerator DoDisco()
    {
        var disco = Instantiate(discoPrefab, transform, false);
        yield return new WaitForSeconds(10f);
        Destroy(disco.gameObject);
    }
    
    [Button]
    private void SpawnJoke()
    {
        StartCoroutine(DoJoke());
    }

    IEnumerator DoJoke()
    {
        var joke = Instantiate(jokePrefab, transform, false);
        yield return new WaitForSeconds(joke.EndTime);
        Destroy(joke.gameObject);
    }
    
    [Button]
    private void SpawnDog()
    {
        Instantiate(dogPrefab, transform, false);
    }
    
    [Button]
    private void SpawnPetpet()
    {
        StartCoroutine(DoPetPet());
    }
    
    IEnumerator DoPetPet()
    {
        var petpet = Instantiate(petpetPrefab, transform, false);
        petpet.transform.position = petpetTarget.position;
        yield return new WaitForSeconds(4f);
        Destroy(petpet.gameObject);
    }
}
