
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float startingTime;

    private float remainingTime;

    [SerializeField] private ScoreSystem scoreSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        remainingTime = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Time remaining
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        // Time has run out
        else if(remainingTime < 0)
        {
            remainingTime = 0;
            scoreSystem.SubmitChimera();
        }

        int seconds = Mathf.CeilToInt(remainingTime);
        timerText.text = seconds.ToString();
    }

    public void ResetTimer()
    {
        remainingTime = startingTime;
    }
}
