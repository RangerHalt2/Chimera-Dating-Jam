using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Constellations", menuName = "Scriptable Objects/Constellation")]
public class ConstellationScriptableObjects : ScriptableObject
{
    //The respective points worth for each part of the constellation
    [SerializeField] private float headPoints;
    [SerializeField] private float bodyPoints;
    [SerializeField] private float tailPoints;

    [SerializeField] private Image headSprite;
    [SerializeField] private Image bodySprite;
    [SerializeField] private Image tailSprite;
}
