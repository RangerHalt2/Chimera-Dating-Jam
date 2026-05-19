// Created By: Ryan Lupoli
// Helper script used to allow for custom buttons based on the Chimera Parts
using UnityEngine;
using UnityEngine.UI;

public class CandidateButton : MonoBehaviour
{
    private ChimeraPart candidate;
    private ChimeraPartGenerator generator;
    private string type;

    // Sets up the button with the proper data
    // Called by ChimeraPartGenerator
    public void Setup(ChimeraPart candidate, ChimeraPartGenerator generator)
    {
        this.candidate = candidate;
        this.generator = generator;
    }

    public void OnClick()
    {
        generator.SelectPart(candidate);
    }
}
