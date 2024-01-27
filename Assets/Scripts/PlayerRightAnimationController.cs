using System;
using System.Collections;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

public class PlayerRightAnimationController : MonoBehaviour, IPlayerAnimationController
{
    [SerializeField] private SkeletonAnimation animation;
    [SerializeField] private GameObject bananPrefab;
    [SerializeField] private GameObject hlopPrefab;
    [SerializeField] private Transform hlopTarget;

    private PlayerTypeAnimation lastLooped;
    private Coroutine _coChangeAnimationBack;
    public bool isBusy;

    public bool IsBusy => isBusy;

    public PlayerTypeAnimation LastLooped
    {
        get => lastLooped;
        set => lastLooped = value;
    }

    [Button]
    public void ChangeType(PlayerTypeAnimation type, Action onComplete = null)
    {
        switch (type)
        {
            case  PlayerTypeAnimation.Idle:
                animation.timeScale = 0.5f;
                animation.loop = true;
                animation.AnimationName = "idle";
                lastLooped = PlayerTypeAnimation.Idle;
                isBusy = false;
                break;
            case PlayerTypeAnimation.Fanny:
                animation.timeScale = 0.5f;
                animation.loop = true;
                animation.AnimationName = "win";
                lastLooped = PlayerTypeAnimation.Fanny;
                isBusy = false;
                break;
            case PlayerTypeAnimation.Sad:
                animation.timeScale = 0.5f;
                animation.loop = true;
                animation.AnimationName = "lose";
                lastLooped = PlayerTypeAnimation.Sad;
                isBusy = false;
                break;
            case PlayerTypeAnimation.Banan:
                animation.timeScale = 0.5f;
                animation.loop = false;
                animation.AnimationName = "slap";
                this.RestartCoroutine(ref _coChangeAnimationBack, Banan(onComplete));
                isBusy = true;
                break;
            case PlayerTypeAnimation.Hlop:
                animation.timeScale = 1f;
                animation.loop = false;
                animation.AnimationName = "cannon";
                this.RestartCoroutine(ref _coChangeAnimationBack, Hlop(onComplete));
                isBusy = true;
                break;
            case PlayerTypeAnimation.Dance:
                animation.timeScale = 0.5f;
                animation.loop = true;
                animation.AnimationName = "dance";
                this.RestartCoroutine(ref _coChangeAnimationBack, ChangeBack(5f, onComplete));
                isBusy = true;
                break;
        }
    }

    private IEnumerator ChangeBack(float time, Action onComplete)
    {
        yield return new WaitForSeconds(time);
        onComplete?.Invoke();
        ChangeType(lastLooped);
    }
    
    private IEnumerator Banan(Action onComplete)
    {
        yield return new WaitForSeconds(0.2f);
        bananPrefab.SetActive(true);
        yield return new WaitForSeconds(1.4f);
        bananPrefab.SetActive(false);
        onComplete?.Invoke();
        ChangeType(lastLooped);
    }
    
    private IEnumerator Hlop(Action onComplete)
    {
        yield return new WaitForSeconds(0.6f);
        var obj = Instantiate(hlopPrefab, hlopTarget, false);
        
        StartCoroutine(DeleteObjects(2.2f, obj.gameObject));
        yield return new WaitForSeconds(0.4f);
        onComplete?.Invoke();
        ChangeType(lastLooped);
    }

    private IEnumerator DeleteObjects(float time, GameObject obj)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj);
    }
}