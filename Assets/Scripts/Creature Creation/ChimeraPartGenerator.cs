// Created By: Ryan Lupoli
// Selects parts to be used by the player's chimera from a predetermined list
// Can also assign those parts to a referneced Chimera Game Object
using System.Collections.Generic;
using UnityEngine;

public class ChimeraPartGenerator : MonoBehaviour
{
    #region Variables
    [Header("General Settings")]
    [Tooltip("How many part candidates should be selected from the master list.")]
    [SerializeField] private int partCandidates = 3;
    [SerializeField] private Chimera chimera;

    [Header("Predetermined Part Lists")]
    [Tooltip("A master list of all potential heads a chimera could have.")]
    [SerializeField] private List<ChimeraPart> headMasterList;
    [Tooltip("A master list of all potential bodies a chimera could have.")]
    [SerializeField] private List<ChimeraPart> bodyMasterList;
    [Tooltip("A master list of all potential legs a chimera could have.")]
    [SerializeField] private List<ChimeraPart> legsMasterList;

    // The list of selected head candidates
    private List<ChimeraPart> headCandidates = new List<ChimeraPart>();
    // The list of seleceted body candidates
    private List<ChimeraPart> bodyCandidates = new List<ChimeraPart>();
    // The list of selected leg candidates
    private List<ChimeraPart> legsCandidates = new List<ChimeraPart>();

    [Header("Selected Parts")]
    [Tooltip("A list of selected heads the player can use to create their Chimera.")]
    public List<ChimeraPart> HeadCandidates => headCandidates;
    [Tooltip("A list of selected bodies the player can use to create their Chimera.")]
    public List<ChimeraPart> BodyCandidates => bodyCandidates;
    [Tooltip("A list of selected legs the player can use to create their Chimera.")]
    public List<ChimeraPart> LegsCandidates => legsCandidates;

    [Header("Candidate UI References")]
    [SerializeField] private Transform headRow;
    [SerializeField] private Transform bodyRow;
    [SerializeField] private Transform legRow;
    [Space]
    [SerializeField] private InfiniteScrollRect headCarousel;
    [SerializeField] private InfiniteScrollRect bodyCarousel;
    [SerializeField] private InfiniteScrollRect legCarousel;

    [Header("Selected Part UI References")]
    [SerializeField] private Transform headSelected;
    [SerializeField] private Transform bodySelected;
    [SerializeField] private Transform legSelected;

    
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateAndDisplayCandidates();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Generator Methods
    // Generates Chimera Parts for every list at once
    private void GenerateChimeraParts()
    {
        Debug.Log("ChimeraPartGenerator: Generating Chimera Parts.");
        // Generate Parts for all candidate lists
        GenerateCandidates(headMasterList, headCandidates);
        GenerateCandidates(bodyMasterList, bodyCandidates);
        GenerateCandidates(legsMasterList, legsCandidates);

        GameEvent.PartsGenerated?.Invoke();
    }

    // Generates a list of selected candidates taken from a masterList and assings the to a provided candidateList
    private void GenerateCandidates(List<ChimeraPart> masterList, List<ChimeraPart> candidateList)
    {
        // Empty the headCandiates List to ensure the correct amount of candidates as specified by partCandidates
        candidateList.Clear();

        // Ensure that there is a list of potential head candidates
        if (masterList == null || masterList.Count == 0)
        {
            Debug.LogWarning("ChimeraPartGenerator: Provided Master list is empty or null! Unable to generate candidate list!");
            return;
        }

        // Warn if there are fewer parts available than what was requested
        if (masterList.Count < partCandidates)
        {
            Debug.LogWarning("ChimeraPartGenerator: Requested number of part candidates is greater than the amount of options in the provided master list. The list will be generated with the entire masterList, but may be smaller than expected.");
        }

        // Prevent requesting more candidates thatn exist.
        int candidatesToGenerate = Mathf.Min(partCandidates, masterList.Count);

        // Create a temporary copy of the master list, so that selected parts may be removed
        List<ChimeraPart> availableParts = new List<ChimeraPart>(masterList);

        // Generate the pre-determined amount of parts specified by partCandidates
        for (int i = 0; i < candidatesToGenerate; i++)
        {
            // Select random index
            int index = Random.Range(0, availableParts.Count);

            // Add selected head to the candidate list
            candidateList.Add(availableParts[index]);

            // Remove the part from the temp list so it can't be selected again
            availableParts.RemoveAt(index);
        }
    }

    public void GenerateAndDisplayCandidates()
    {
        // Reset the Chimera
        chimera.ClearParts();
        DisplaySelected();

        // Generate new Parts
        GenerateChimeraParts();

        // Display Part Candidates
        //For row UI
        if (headRow != null)
        {
            DisplayCandidates(headCandidates, headRow);
        }
        if (bodyRow != null)
        {
            DisplayCandidates(headCandidates, bodyRow);
        }
        if (legRow != null)
        {
            DisplayCandidates(headCandidates, legRow);
        }

        // For Carousel UI
        if(headCarousel != null)
        {
            headCarousel.assets = headCandidates;
            headCarousel.Rebuild();
        }
        if (bodyCarousel != null)
        {
            bodyCarousel.assets = bodyCandidates;
            bodyCarousel.Rebuild();
        }
        if (legCarousel != null)
        {
            legCarousel.assets = legsCandidates;
            legCarousel.Rebuild();
        }
    }
    #endregion

    #region Display Methods
    private void DisplayCandidates(List<ChimeraPart> candidates, Transform rowParent)
    {
        // Clear old UI
        foreach (Transform child in rowParent)
        {
            Destroy(child.gameObject);
        }

        // Create new UI elements
        foreach(ChimeraPart candidate in candidates)
        {
            // Spawn UI prefab
            GameObject uiObject = Instantiate(candidate.partUIPrefab, rowParent);

            // Find Sprite renderer on child
            UnityEngine.UI.Image image = uiObject.GetComponentInChildren<UnityEngine.UI.Image>();
            image.sprite = candidate.partSprite;

            CandidateButton button = uiObject.GetComponent<CandidateButton>();

            button.Setup(candidate, this);
        }
    }

    // Display all of the selected parts on the Chimera
    private void DisplaySelected()
    {
        DisplaySingle(chimera.head, headSelected);
        DisplaySingle(chimera.body, bodySelected);
        DisplaySingle(chimera.legs, legSelected);
    }

    private void DisplaySingle(ChimeraPart part, Transform loc)
    {
        // Destroy all current children in the selection box
        foreach (Transform child in loc)
            Destroy(child.gameObject);

        // If there is no part, do nothing
        if (part == null) return;

        GameObject uiObject = Instantiate(part.partUIPrefab, loc);

        UnityEngine.UI.Image image = uiObject.GetComponentInChildren<UnityEngine.UI.Image>();
        image.sprite = part.partSprite;
    }
    #endregion

    // Used to select a part and add it to the Chimera
    public void SelectPart(ChimeraPart part)
    {
        switch (part.partType)
        {
            case ChimeraPart.Type.Head:
                chimera.head = part;
                Debug.Log($"ChimeraPartGenerator: Selected {part.name} as head.");
                break;
            case ChimeraPart.Type.Body:
                chimera.body = part;
                Debug.Log($"ChimeraPartGenerator: Selected {part.name} as body.");
                break;
            case ChimeraPart.Type.Legs:
                chimera.legs = part;
                Debug.Log($"ChimeraPartGenerator: Selected {part.name} as legs.");
                break;
        }

        DisplaySelected();
    }
}