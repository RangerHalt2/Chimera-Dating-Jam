using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Timer timer;
    [SerializeField] private ChimeraPartGenerator cpg;
    [SerializeField] private ConstellationBuilder cb;

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
    }
}
