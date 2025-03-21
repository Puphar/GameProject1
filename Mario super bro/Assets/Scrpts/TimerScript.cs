using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class TimerScript : MonoBehaviour
{
    [Tooltip("Duration of the timer in seconds")]
    public float duration = 300f;

    [Tooltip("Whether the timer should start automatically when the scene loads")]
    public bool startAutomatically = true;

    [Tooltip("Whether the timer should repeat once it reaches zero")]
    public bool repeat = false;

    public UnityEvent onTimerStart;
    public UnityEvent onTimerUpdate;
    public UnityEvent onTimerEnd;

    private float currentTime;
    private bool isRunning;

    public TextMeshProUGUI timerText;

    private void Start()
    {
        if (startAutomatically)
        {
            StartTimer();
        }
    }

    private void Update()
    {
        if (isRunning)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                EndTimer();
            }
            else
            {
                UpdateTimer();
            }
        }
    }

    public void StartTimer()
    {
        currentTime = duration;
        isRunning = true;
        onTimerStart.Invoke();
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        currentTime = duration;
    }

    private void UpdateTimer()
    {
        onTimerUpdate.Invoke();

        if (timerText != null)
        {
            timerText.text = FormatTime(currentTime);
        }
    }

    private void EndTimer()
    {
        isRunning = false;
        onTimerEnd.Invoke();
        if (repeat)
        {
            ResetTimer();
            StartTimer();
        }
    }

    private string FormatTime(float time)
    {
        int seconds = Mathf.FloorToInt(time);
        return string.Format("{0}", seconds);
    }

}
