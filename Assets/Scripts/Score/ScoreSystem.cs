using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private float winMargin = 0.75f; // % stuff

    private Chimera chimeraRef;

    private float cummulativePoints;
    private float partPoints;
    private float tagBonusPoints;

    private void Start()
    {
        chimeraRef = GameObject.FindAnyObjectByType<Chimera>();
    }

    //private float CalculatePartPoints()
    //{
        //return;
    //}

    public void Update()
    {
        if (chimeraRef != null)
        {
            
        }
        else
        {
            chimeraRef = GameObject.FindAnyObjectByType<Chimera>();
        }
    }

    public void SubmitChimera()
    {
        Debug.Log("Submit");
    }

}
