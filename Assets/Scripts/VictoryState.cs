using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class VictoryState : MonoBehaviour
{
    [SerializeField] private List<GameObject> gameEndObjects;
    [SerializeField] private List<GameObject> gameEndObjectsDisable;
    [SerializeField] private PlayerLeftAnimationController player1;
    [SerializeField] private PlayerRightAnimationController player2;
    [SerializeField] private PrincessController princesse;
    [SerializeField] private Transform vinnerTarget;
    [SerializeField] private Transform princesseTarget;
    
    
    [Button]
    public void VictoryTeam(int teamId)
    {
        foreach (var gameEndObject in gameEndObjects)
        {
            gameEndObject.SetActive(true);
        }
        foreach (var gameEndObject in gameEndObjectsDisable)
        {
            gameEndObject.SetActive(false);
        }
        princesse.ChangeType(PrincessController.PrincesseType.OmegaLUL);
        princesse.transform.position = princesseTarget.position;

        if (teamId == 0)
        {
            player1.transform.position = vinnerTarget.position;
            player1.ChangeType(PlayerTypeAnimation.Fanny);
            player2.gameObject.SetActive(false);
        }
        else
        {
            player2.transform.position = vinnerTarget.position;
            player2.ChangeType(PlayerTypeAnimation.Fanny);
            player1.gameObject.SetActive(false);
        }
    }
}
