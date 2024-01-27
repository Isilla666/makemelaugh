using System;
using System.Collections;
using System.Collections.Generic;
using Backend.Events;
using Backend.Registration;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject discoPrefab;
    [SerializeField] private JokeController jokePrefab;
    [SerializeField] private GameObject dogPrefab;
    [SerializeField] private GameObject petpetPrefab;
    [SerializeField] private Transform petpetTarget;
    [SerializeField] private PlayerLeftAnimationController leftAnimationController;
    [SerializeField] private PlayerRightAnimationController rightAnimationController;
    [SerializeField] private ProgressBalancer progressBalancer;
    [SerializeField] private CakeAnimator leftCakeAnimator;
    [SerializeField] private CakeAnimator rightCakeAnimator;
    [SerializeField] private VictoryState victoryState;
    [SerializeField] private PrincessController princessController;
    
    //int - actionId(skillId)
    private Dictionary<int, List<PlayerAction>> _allActions;

    private List<IPlayerAnimationController> _playerAnimationControllers;
    private ActionEvent _stateChangeListener;

    //int - teamId
    private readonly Dictionary<int, List<ActionType>> _runningPlayerActions = new()
    {
        { 0, new List<ActionType>() },
        { 1, new List<ActionType>() },
    };

    private readonly List<ActionType> _runningCommonActions = new();

    private void Awake()
    {
        progressBalancer.TimerCompleted += ProgressBalancerOnTimerCompleted;
        progressBalancer.ScoreUpdated += ProgressBalancerOnScoreUpdated;
        progressBalancer.TimerUpdated += ProgressBalancerOnTimerUpdated;
        
        _stateChangeListener = SignalRegistration<ActionEvent>.Resolve();
        _stateChangeListener.OnValueChanged += HandleAction;
    }

    private void OnDestroy()
    {
        progressBalancer.TimerCompleted -= ProgressBalancerOnTimerCompleted;
        progressBalancer.ScoreUpdated -= ProgressBalancerOnScoreUpdated;
        progressBalancer.TimerUpdated -= ProgressBalancerOnTimerUpdated;
        
        _stateChangeListener.OnValueChanged -= HandleAction;
    }
    
    private void Start()
    {
        _playerAnimationControllers = new List<IPlayerAnimationController>
        {
            leftAnimationController,
            rightAnimationController
        };
        
        _allActions = new Dictionary<int, List<PlayerAction>>
        {
            { 0, new List<PlayerAction> { new(ActionPlaceType.Player, ActionType.Click, false, HandleClick) } },
            { 1, new List<PlayerAction> { new(ActionPlaceType.Player, ActionType.Cake, false, HandleCake) } },
            {
                2, new List<PlayerAction>
                {
                    new(ActionPlaceType.Common, ActionType.Dog, false, HandleDog),
                    new(ActionPlaceType.Common, ActionType.PetPet, true, HandlePetpet),
                    new(ActionPlaceType.Player, ActionType.Banana, true, HandleBanana),
                    new(ActionPlaceType.Player, ActionType.Hlop, true, HandleHlop),
                    new(ActionPlaceType.Player, ActionType.Dance, true, HandleDance),
                }
            },
            { 3, new List<PlayerAction> { new(ActionPlaceType.Common, ActionType.Disco, true, HandleDisco) } },
            { 4, new List<PlayerAction> { new(ActionPlaceType.Common, ActionType.Joke, true, HandleJoke) } }
        };
        
        progressBalancer.StartTimer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("WaitUsersScene");
    }

    private void ProgressBalancerOnTimerUpdated(float time)
    {
        var percent = time / progressBalancer.MaxTime;

        const int idle = 25;
        const int  smile = 50;
        const int  laugh = 75;
        
        var state = percent switch
        {
            <= idle => PrincessController.PrincesseType.Idle,
            > idle and <= smile => PrincessController.PrincesseType.Smile,
            > smile and <= laugh => PrincessController.PrincesseType.Laugh,
            _ => PrincessController.PrincesseType.OmegaLUL
        };

        princessController.ChangeType(state);
    }

    private void ProgressBalancerOnScoreUpdated(float teamScore1, float teamScore2)
    {
        SetPlayerState(leftAnimationController, teamScore1);
        SetPlayerState(rightAnimationController, teamScore2);
    }

    private void SetPlayerState(IPlayerAnimationController animationController, float score)
    {
        var state = score switch
        {
            <= 33 => PlayerTypeAnimation.Sad,
            <= 66 and > 33 => PlayerTypeAnimation.Idle,
            _ => PlayerTypeAnimation.Fanny
        };

        if (animationController.IsBusy)
            animationController.LastLooped = state;
        else
            animationController.ChangeType(state);
    }
    
    private void ProgressBalancerOnTimerCompleted(int teamId)
    {
        victoryState.VictoryTeam(teamId);
    }

    private void HandleAction(ActionEvent.ActionData actionData)
    {
        progressBalancer.SetScore(actionData.team, actionData.damage);
        
        var actions = _allActions[actionData.id];

        var playerAction = actions.Count switch
        {
            > 1 => actions[Random.Range(0, actions.Count)],
            > 0 => actions[0],
            _ => null
        };

        if (playerAction == null) return;

        if (playerAction.IsOnce)
        {
            if (playerAction.ActionPlaceType == ActionPlaceType.Common)
            {
                if (_runningCommonActions.Contains(playerAction.ActionType)) return;

                _runningCommonActions.Add(playerAction.ActionType);
                playerAction.Action?.Invoke(actionData.team, () => OnComplete(playerAction.ActionType, playerAction.ActionPlaceType, actionData.team));
            }
            else
            {
                if (!_runningPlayerActions.TryGetValue(actionData.team, out var actionTypes)) return;

                if (!actionTypes.Contains(playerAction.ActionType))
                    actionTypes.Add(playerAction.ActionType);
            }
        }
        else
            playerAction.Action?.Invoke(actionData.team, null);

        Debug.Log(playerAction.ActionType);
    }

    private void OnComplete(ActionType actionType, ActionPlaceType actionPlaceType, int teamId)
    {
        if (actionPlaceType == ActionPlaceType.Common)
            _runningCommonActions.Remove(actionType);
        else
            _runningPlayerActions[teamId].Remove(actionType);
    }

    private void HandleClick(int teamId, Action onComplete)
    {
    }

    private void HandleJoke(int teamId, Action onComplete) => StartCoroutine(DoJoke(onComplete));

    private void HandleDisco(int teamId, Action onComplete) => StartCoroutine(DoDisco(onComplete));

    private void HandleDog(int teamId, Action onComplete) => DoDog();

    private void HandlePetpet(int teamId, Action onComplete) => StartCoroutine(DoPetPet(onComplete));

    private void HandleDance(int teamId, Action onComplete) 
        => _playerAnimationControllers[teamId].ChangeType(PlayerTypeAnimation.Dance, onComplete);

    private void HandleCake(int teamId, Action onComplete)
    {
        if(teamId == 0)
            leftCakeAnimator.StartAnimation();
        else
            rightCakeAnimator.StartAnimation();
    }
    private void HandleBanana(int teamId, Action onComplete)
        => _playerAnimationControllers[teamId].ChangeType(PlayerTypeAnimation.Banan, onComplete);
    
    private void HandleHlop(int teamId, Action onComplete)
        => _playerAnimationControllers[teamId].ChangeType(PlayerTypeAnimation.Hlop, onComplete);
    
    IEnumerator DoDisco( Action onCompleted)
    {
        var disco = Instantiate(discoPrefab, transform, false);
        yield return new WaitForSeconds(10f);
        Destroy(disco.gameObject);
        onCompleted?.Invoke();
    }
    

    IEnumerator DoJoke(Action onComplete)
    {
        var joke = Instantiate(jokePrefab, transform, false);
        yield return new WaitForSeconds(joke.EndTime);
        Destroy(joke.gameObject);
        onComplete?.Invoke();
    }

    [Button]
    private void DoDog()
    {
        Instantiate(dogPrefab, transform, false);
    }
    
    IEnumerator DoPetPet(Action onComplete)
    {
        var petpet = Instantiate(petpetPrefab, transform, false);
        petpet.transform.position = petpetTarget.position;
        yield return new WaitForSeconds(4f);
        Destroy(petpet.gameObject);
        onComplete?.Invoke();
    }


    private class PlayerAction
    {
        public ActionPlaceType ActionPlaceType { get; }
        public ActionType ActionType { get; }
        public Action<int, Action> Action { get; }
        public bool IsOnce { get; }

        public PlayerAction(ActionPlaceType actionPlaceType, ActionType actionType, bool isOnce,
            Action<int, Action> action)
        {
            ActionPlaceType = actionPlaceType;
            ActionType = actionType;
            Action = action;
            IsOnce = isOnce;
        }
    }
}