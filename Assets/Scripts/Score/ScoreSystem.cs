using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private float winMargin = 0.75f; // % stuff

    private Chimera chimeraRef;

    private float cummulativePoints;

    private void Start()
    {
        chimeraRef = GameObject.FindAnyObjectByType<Chimera>();
    }



    public void SubmitChimera()
    {
        Debug.Log("Submit");
    }

}
