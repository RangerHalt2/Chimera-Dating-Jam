using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Constellations", menuName = "Scriptable Objects/Constellation")]
public class ConstellationScriptableObjects : ScriptableObject
{
    //The respective points worth for each part of the constellation
    public float headPoints;
    public float bodyPoints;
    public float tailPoints;

    public Sprite headSprite;
    public Sprite bodySprite;
    public Sprite tailSprite;

    [Tooltip("A list of any and all tags the part should have.")]
    public List<string> headTags;
    public List<string> bodyTags;
    public List<string> tailTags;
}
