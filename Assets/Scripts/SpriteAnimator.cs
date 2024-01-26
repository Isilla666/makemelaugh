using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private float delay;

    private int _currentSpriteIndex;
    private Coroutine _coroutine;
    
    
    [Button]
    void Start()
    {
        this.RestartCoroutine(ref _coroutine, DoIt());
    }

    private IEnumerator DoIt()
    {
        _currentSpriteIndex = 0;
        if (sprites.Count == 0 || spriteRenderer == null)
        {
            yield break;
        }

        if (delay <= 0)
        {
            delay = 0.01f;
        }
        var timeYield = new WaitForSeconds(delay);
        while (true)
        {
            spriteRenderer.sprite = sprites[_currentSpriteIndex];
            _currentSpriteIndex++;
            _currentSpriteIndex %= sprites.Count;
            yield return timeYield;
        }
    }
}
