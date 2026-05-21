using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Timer timer;
    [SerializeField] private ChimeraPartGenerator cpg;
    [SerializeField] private ConstellationBuilder cb;
    [SerializeField] private ScoreSystem scoreSystem;

    private void Start()
    {
        RestartGame();
    }

    public void RestartGame()
    {
        uiManager.GoToPage(0);
        timer.ResetTimer();
        cpg.GenerateAndDisplayCandidates();
        cb.SelectConstellation();
        StartCoroutine(ResetGameNextFrame());
    }

    private IEnumerator ResetGameNextFrame()
    {
        yield return new WaitForSeconds(0.2f);
        scoreSystem.ResetGame();
    }

}
