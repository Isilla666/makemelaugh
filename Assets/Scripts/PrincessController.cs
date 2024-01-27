using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

public class PrincessController : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation animation;
    
    public enum PrincesseType
    {
        Idle,
        Smile,
        Laugh,
        OmegaLUL
    }

    [Button]
    public void ChangeType(PrincesseType type)
    {
        switch (type)
        {
            case PrincesseType.Idle:
                animation.AnimationName = "idle";
                break;
            case PrincesseType.Smile:
                animation.AnimationName = "smile";
                break;
            case PrincesseType.Laugh:
                animation.AnimationName = "laugh";
                break;
            case PrincesseType.OmegaLUL:
                animation.AnimationName = "OmegaLUL";
                break;
        }
    }
}
