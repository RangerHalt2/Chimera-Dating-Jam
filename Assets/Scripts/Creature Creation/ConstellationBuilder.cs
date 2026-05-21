using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ConstellationBuilder : MonoBehaviour
{
    [SerializeField] private Constellation constellationRef;

    [SerializeField] private List<ConstellationScriptableObjects> constellationMasterList;

    [Header("UI References")]
    [SerializeField] private Transform topDisplay;
    [SerializeField] private Transform midDisplay;
    [SerializeField] private Transform bottomDisplay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DisplayConstellation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectConstellation()
    {
        constellationRef.constellation = PickRandomConstellation();
        DisplayConstellation();
    }

    public ConstellationScriptableObjects PickRandomConstellation()
    {
        if(constellationMasterList == null || constellationMasterList.Count == 0)
        {
            Debug.LogWarning("ConstelationBuilder: Provided Master list is empty or null! Unable to select constellation!");
            return null;
        }

        int index = Random.Range(0, constellationMasterList.Count); 

        return constellationMasterList[index];
    }

    public void DisplayConstellation()
    {
        Image topImage = topDisplay.GetComponent<Image>();
        Image midImage = midDisplay.GetComponent<Image>();
        Image bottomImage = bottomDisplay.GetComponent<Image>();

        // No active constellation
        if (constellationRef.constellation == null)
        {
            if (topImage != null)
                topImage.enabled = false;

            if (midImage != null)
                midImage.enabled = false;

            if (bottomImage != null)
                bottomImage.enabled = false;

            return;
        }

        // Enable images
        if (topImage != null)
        {
            topImage.enabled = true;
            topImage.sprite = constellationRef.constellation.headSprite;
        }

        if (midImage != null)
        {
            midImage.enabled = true;
            midImage.sprite = constellationRef.constellation.bodySprite;
        }

        if (bottomImage != null)
        {
            bottomImage.enabled = true;
            bottomImage.sprite = constellationRef.constellation.tailSprite;
        }
    }
}
