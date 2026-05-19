using UnityEngine;

[CreateAssetMenu(fileName = "ChimeraPart", menuName = "Scriptable Objects/ChimeraPart")]
public class ChimeraPart : ScriptableObject
{
    [Tooltip("The name for the chimera part.")]
    public string partName;
    [Tooltip("What type of part this will be on/in the chimera.")]
    public Type partType;
    [Tooltip("The visual asset used for the chimera part.")]
    public Sprite partSprite;
    [Tooltip("Game Object prefab this chimera part will use when instantiated as a UI asset.")]
    public GameObject partUIPrefab;
    [Tooltip("The amount of points associated with this chimera part.")]
    public float partPoints;

    public enum Type
    {
        Head,
        Body,
        Legs
    }
}
