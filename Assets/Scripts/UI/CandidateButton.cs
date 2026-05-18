// Created By: Ryan Lupoli
// Helper script used to allow for custom buttons based on the Chimera Parts
using UnityEngine;
using UnityEngine.UI;

public class CandidateButton : MonoBehaviour
{
    private GameObject candidate;
    private ChimeraPartGenerator generator;
    private string type;

    // Sets up the button with the proper data
    // Called by ChimeraPartGenerator
    public void Setup(GameObject candidate, ChimeraPartGenerator generator, string type)
    {
        this.candidate = candidate;
        this.generator = generator;
        this.type = type;
    }

    public void OnClick()
    {
        generator.SelectPart(candidate, type);
    }
}
