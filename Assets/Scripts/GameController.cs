using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject discoPrefab;

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
}
