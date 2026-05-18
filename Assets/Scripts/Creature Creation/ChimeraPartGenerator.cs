// Created By: Ryan Lupoli
// Selects parts to be used by the player's chimera from a predetermined list
// Can also assign those parts to a referneced Chimera Game Object
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class ChimeraPartGenerator : MonoBehaviour
{
    #region Variables
    [Header("General Settings")]
    [Tooltip("How many part candidates should be selected from the master list.")]
    [SerializeField] private int partCandidates = 3;
    [SerializeField] private Chimera chimera;

    [Header("Predetermined Part Lists")]
    [Tooltip("A master list of all potential heads a chimera could have.")]
    [SerializeField] private List<GameObject> headMasterList;
    [Tooltip("A master list of all potential bodies a chimera could have.")]
    [SerializeField] private List<GameObject> bodyMasterList;
    [Tooltip("A master list of all potential legs a chimera could have.")]
    [SerializeField] private List<GameObject> legsMasterList;

    // The list of selected head candidates
    private List<GameObject> headCandidates = new List<GameObject>();
    // The list of seleceted body candidates
    private List<GameObject> bodyCandidates = new List<GameObject>();
    // The list of selected leg candidates
    private List<GameObject> legsCandidates = new List<GameObject>();

    [Header("Selected Parts")]
    [Tooltip("A list of selected heads the player can use to create their Chimera.")]
    public List<GameObject> HeadCandidates => headCandidates;
    [Tooltip("A list of selected bodies the player can use to create their Chimera.")]
    public List<GameObject> BodyCandidates => bodyCandidates;
    [Tooltip("A list of selected legs the player can use to create their Chimera.")]
    public List<GameObject> LegsCandidates => legsCandidates;

    [Header("Candidate UI References")]
    [SerializeField] private Transform headRow;
    [SerializeField] private Transform bodyRow;
    [SerializeField] private Transform legRow;

    [SerializeField] private GameObject candidateUIPrefab;

    [Header("Selected Part UI References")]
    [SerializeField] private Transform headSelected;
    [SerializeField] private Transform bodySelected;
    [SerializeField] private Transform legSelected;

    
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateChimeraParts();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Generator Methods
    // Generates Chimera Parts for every list at once
    private void GenerateChimeraParts()
    {
        // Generate Parts for all candidate lists
        GenerateCandidates(headMasterList, headCandidates);
        GenerateCandidates(bodyMasterList, bodyCandidates);
        GenerateCandidates(legsMasterList, legsCandidates);
    }

    // Generates a list of selected candidates taken from a masterList and assings the to a provided candidateList
    private void GenerateCandidates(List<GameObject> masterList, List<GameObject> candidateList)
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
        List<GameObject> availableParts = new List<GameObject>(masterList);

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
        DisplayCandidates(headCandidates, headRow, "Head");
        DisplayCandidates(bodyCandidates, bodyRow, "Body");
        DisplayCandidates(legsCandidates, legRow, "Legs");
    }
    #endregion

    #region Display Methods
    private void DisplayCandidates(List<GameObject> candidates, Transform rowParent, string type)
    {
        // Clear old UI
        foreach (Transform child in rowParent)
        {
            Destroy(child.gameObject);
        }

        // Create new UI elements
        foreach(GameObject candidate in candidates)
        {
            // Spawn UI prefab
            GameObject uiObject = Instantiate(candidateUIPrefab, rowParent);

            // Find Sprite renderer on child
            SpriteRenderer spriteRenderer = candidate.GetComponentInChildren<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                UnityEngine.UI.Image image = uiObject.GetComponentInChildren<UnityEngine.UI.Image>();

                image.sprite = spriteRenderer.sprite;
            }

            CandidateButton button = uiObject.GetComponent<CandidateButton>();
            button.Setup(candidate, this, type);
        }
    }

    // Display all of the selected parts on the Chimera
    private void DisplaySelected()
    {
        DisplaySingle(chimera.head, headSelected);
        DisplaySingle(chimera.body, bodySelected);
        DisplaySingle(chimera.legs, legSelected);
    }

    private void DisplaySingle(GameObject part, Transform loc)
    {
        // Destroy all current children in the selection box
        foreach (Transform child in loc)
            Destroy(child.gameObject);

        // If there is no part, do nothing
        if (part == null) return;

        GameObject uiObject = Instantiate(candidateUIPrefab, loc);

        SpriteRenderer spriteRenderer = part.GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            UnityEngine.UI.Image image = uiObject.GetComponentInChildren<UnityEngine.UI.Image>();

            image.sprite = spriteRenderer.sprite;
        }
    }
    #endregion

    // Used to select a part and add it to the Chimera
    public void SelectPart(GameObject part, string type)
    {
        switch (type)
        {
            case "Head":
                chimera.head = part;
                break;
            case "Body":
                chimera.body = part;
                break;
            case "Legs":
                chimera.legs = part;
                break;
        }
        Debug.Log($"ChimeraPartGenerator: Selected {part.name} as {type}");

        DisplaySelected();
    }
}