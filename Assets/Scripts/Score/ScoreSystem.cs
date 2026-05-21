using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private float winMargin = 75f; // out of 100 stuff
    [SerializeField] private float[] tagBonus = { 5f, 10f, 15f };

    private Chimera chimeraRef;
    private Constellation constellationRef;

    private float cummulativePoints;
    private float partPoints;
    private float tagBonusPoints;

    private UIManager uIManager;

    private bool locked = false;

    private void Start()
    {
        chimeraRef = GameObject.FindAnyObjectByType<Chimera>();
        constellationRef = GameObject.FindAnyObjectByType<Constellation>();
        uIManager = GameObject.FindAnyObjectByType<UIManager>();
    }

    private float CalculatePartPoints(float maxPointsPossible, ChimeraPart chimeraPart, float suitorScore)
    {
        return ((1 - Mathf.Abs(suitorScore - chimeraPart.partPoints)/10) * maxPointsPossible);
    }

    private float CalculateTagBonusPoints()
    {
        HashSet<string> chimeraAllTags = new HashSet<string>(chimeraRef.head.partTags);
        chimeraAllTags.UnionWith(chimeraRef.body.partTags);
        chimeraAllTags.UnionWith(chimeraRef.legs.partTags);

        HashSet<string> constellationAllTags = new HashSet<string>(constellationRef.constellation.headTags);
        constellationAllTags.UnionWith(constellationRef.constellation.bodyTags);
        constellationAllTags.UnionWith(constellationRef.constellation.tailTags);

        int count = chimeraAllTags.Count(tag => constellationAllTags.Contains(tag));

        if (count > 3) return tagBonus[2];
        switch(count){
            case 3:
                return tagBonus[2];
            case 2:
                return tagBonus[1];
            case 1:
                return tagBonus[0];
            default:
                return 0;
        }
    }

    public void Update()
    {
        if(locked) return;
        if (chimeraRef != null && (chimeraRef.head == null && chimeraRef.body == null && chimeraRef.legs == null) && constellationRef != null)
        {
            partPoints = 0;
            tagBonusPoints = 0;
            cummulativePoints = 0;
            partPoints += CalculatePartPoints(33, chimeraRef.head, constellationRef.constellation.headPoints); //Head points.
            partPoints += CalculatePartPoints(34, chimeraRef.body, constellationRef.constellation.bodyPoints); //Body points.
            partPoints += CalculatePartPoints(33, chimeraRef.legs, constellationRef.constellation.tailPoints); //Leg points.
            tagBonusPoints = CalculateTagBonusPoints();
            cummulativePoints = tagBonusPoints + partPoints;
            UpdateScoreUI();
        }
        else
        {
            chimeraRef = GameObject.FindAnyObjectByType<Chimera>();
            constellationRef = GameObject.FindAnyObjectByType<Constellation>();
        }
    }

    private void UpdateScoreUI()
    {

    }

    public void SubmitChimera()
    {
        locked = true;
        if (cummulativePoints >= winMargin)
        {
            uIManager.GoToPage(1);
        }
        else
        {
            uIManager.GoToPage(2);
        }

        Invoke(nameof(ResetGame), 3f);

    }

    public void ResetGame()
    {
        locked = false;
        GameManager gm = GameObject.FindAnyObjectByType<GameManager>();
        gm.RestartGame();
    }

}
