using UnityEngine;

public class CountdownTimer
{
    public float remainingTime { get; set; }
    public bool isRunning { get; private set; }
    private readonly float startingTime;

    public CountdownTimer(float startingTime)
    {
        this.startingTime = startingTime;
    }

    public void Start()
    {
        isRunning = true;
        remainingTime = startingTime;
    }

    public void Tick()
    {
        if (!isRunning) return;
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else
        {
            remainingTime = 0;
            isRunning = false;
        }
    }
}