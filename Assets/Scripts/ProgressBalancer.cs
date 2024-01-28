using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBalancer : MonoBehaviour
{
    [SerializeField] private float maxTime = 30;
    [SerializeField] private float currentTime = 0;
    [SerializeField] private float score1 = 1;
    [SerializeField] private float score2 = 1;
    [SerializeField] private Slider slider1;
    [SerializeField] private Slider slider2;

    private Coroutine _coTimer;

    public event Action<int> TimerCompleted;
    public event Action<float, float> ScoreUpdated;
    public event Action<float> TimerUpdated;

    public float MaxTime => maxTime;

    public void SetScore(int teamId, int damage)
    {
        if (teamId == 0)
            score1 += damage;
        else
            score2 += damage;
        
        ScoreWasUpdated();
    }
    
    [Button]
    void ScoreWasUpdated()
    {
        var progress = score1 / (score1 + score2);  // -0.5    0.5

        var timeProgress = currentTime / maxTime;
        if (progress <= 0.5f)
        {
            progress = (1f - timeProgress) * progress;
        }
        else
        {
            progress = Mathf.Lerp(progress, 1.0f, timeProgress);
        }

        slider1.value = progress;
        slider2.value = 1f-progress;
        // Debug.Log(progress);
        
        ScoreUpdated?.Invoke(slider1.value, slider2.value);
    }

    [Button]
    public void StartTimer()
    {
        currentTime = 0;
        this.RestartCoroutine(ref _coTimer, DoItTimer());
    }

    private IEnumerator DoItTimer()
    {
        var timeYield = new WaitForSeconds(1f);
        do
        {
            ScoreWasUpdated();
            yield return timeYield;
            currentTime++;
            TimerUpdated?.Invoke(currentTime);
        } while (currentTime < maxTime);
        ScoreWasUpdated();
        TimerCompleted?.Invoke(score1 >= score2 ? 0 : 1);
    }
}
