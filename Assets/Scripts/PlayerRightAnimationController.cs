using System.Collections;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

public class PlayerRightAnimationController : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation animation;
    [SerializeField] private GameObject bananPrefab;
    [SerializeField] private GameObject hlopPrefab;
    [SerializeField] private Transform hlopTarget;

    private PlayerLeftType lastLooped;
    private Coroutine _coChangeAnimationBack;
    public bool isBusy;
    
    public enum PlayerLeftType
    {
        Idle,
        Fanny,
        Sad,
        Banan,
        Hlop,
        Dance
    }

    [Button]
    public void ChangeType(PlayerLeftType type)
    {
        switch (type)
        {
            case  PlayerLeftType.Idle:
                animation.timeScale = 0.5f;
                animation.loop = true;
                animation.AnimationName = "idle";
                lastLooped = PlayerLeftType.Idle;
                isBusy = false;
                break;
            case PlayerLeftType.Fanny:
                animation.timeScale = 0.5f;
                animation.loop = true;
                animation.AnimationName = "win";
                lastLooped = PlayerLeftType.Fanny;
                isBusy = false;
                break;
            case PlayerLeftType.Sad:
                animation.timeScale = 0.5f;
                animation.loop = true;
                animation.AnimationName = "lose_loop";
                lastLooped = PlayerLeftType.Sad;
                isBusy = false;
                break;
            case PlayerLeftType.Banan:
                animation.timeScale = 0.5f;
                animation.loop = false;
                animation.AnimationName = "slap";
                this.RestartCoroutine(ref _coChangeAnimationBack, Banan());
                isBusy = true;
                break;
            case PlayerLeftType.Hlop:
                animation.timeScale = 1f;
                animation.loop = false;
                animation.AnimationName = "cannon";
                this.RestartCoroutine(ref _coChangeAnimationBack, Hlop());
                isBusy = true;
                break;
            case PlayerLeftType.Dance:
                animation.timeScale = 0.5f;
                animation.loop = true;
                animation.AnimationName = "dance";
                this.RestartCoroutine(ref _coChangeAnimationBack, ChangeBack(5f));
                isBusy = true;
                break;
        }
    }

    private IEnumerator ChangeBack(float time)
    {
        yield return new WaitForSeconds(time);
        ChangeType(lastLooped);
    }
    
    private IEnumerator Banan()
    {
        yield return new WaitForSeconds(0.2f);
        bananPrefab.SetActive(true);
        yield return new WaitForSeconds(1.4f);
        bananPrefab.SetActive(false);
        ChangeType(lastLooped);
    }
    
    private IEnumerator Hlop()
    {
        yield return new WaitForSeconds(0.6f);
        var obj = Instantiate(hlopPrefab, hlopTarget, false);
        
        StartCoroutine(DeleteObjects(2.2f, obj.gameObject));
        yield return new WaitForSeconds(0.4f);
        ChangeType(lastLooped);
    }

    private IEnumerator DeleteObjects(float time, GameObject obj)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj);
    }
}