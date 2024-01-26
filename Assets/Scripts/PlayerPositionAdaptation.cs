using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerPositionAdaptation : MonoBehaviour
{
    [SerializeField] private bool isLeft;
    [SerializeField] private float procent;
    
    
    [Button]
    private void Awake()
    {
        var width = Screen.width;
        if (isLeft)
        {
            var t = Camera.main.ScreenToWorldPoint(new Vector3(width * procent,0,0));
            var transform1 = transform;
            transform1.position = new Vector3(t.x, transform1.position.y);
        }
        else
        {
            var t = Camera.main.ScreenToWorldPoint(new Vector3(width - width * procent,0,0));
            var transform1 = transform;
            transform1.position = new Vector3(t.x, transform1.position.y);
        }
    }
}
