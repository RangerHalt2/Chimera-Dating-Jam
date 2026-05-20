using Unity.VisualScripting;
using UnityEngine;

public class ChimeraBuilder : MonoBehaviour
{
    [Header("Chimera Reference")]
    [Tooltip("Refrence to the Chimera Game Object which will store the part selections.")]
    [SerializeField] private Chimera chimera;

    [Header("Carousel References")]
    [Tooltip("Refrence to the carousel containing the player's head options.")]
    [SerializeField] private InfiniteScrollRect headCarousel;
    [Tooltip("Refrence to the carousel containing the player's body options.")]
    [SerializeField] private InfiniteScrollRect bodyCarousel;
    [Tooltip("Refrence to the carousel containing the player's leg options.")]
    [SerializeField] private InfiniteScrollRect legCarousel;

    private ChimeraPart lastHead;
    private ChimeraPart lastBody;
    private ChimeraPart lastLegs;

    [Header("Selected Part UI References")]
    [SerializeField] private Transform headSelected;
    [SerializeField] private Transform bodySelected;
    [SerializeField] private Transform legSelected;

    private bool ready = false;

    private void Start()
    {
       ready = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ready)
        {
            return;
        }

        if (chimera == null)
        {
            Debug.LogError("ChimeraBuilder: Missing reference to Chimera. Cannot write data/build a chimera!");
            return;
        }

        if (headCarousel == null || bodyCarousel == null || legCarousel == null)
        {
            Debug.LogWarning("ChimeraBuilder: Missing reference to Part Carousels! Cannot Build Chimera!");
            return;
        }

        UpdateChimeraSelection();
    }

    private void OnEnable()
    {
        GameEvent.PartsGenerated += HandlePartsGenerated;
    }
    private void OnDisable()
    {
        GameEvent.PartsGenerated -= HandlePartsGenerated;
    }

    void HandlePartsGenerated()
    {
        ready = true;
    }

    public void UpdateChimeraSelection()
    {
        ChimeraPart head = headCarousel.GetCenteredPart();
        ChimeraPart body = bodyCarousel.GetCenteredPart();
        ChimeraPart legs = legCarousel.GetCenteredPart();

        if (head == null)
            Debug.LogWarning("Head carousel returned NULL center item");

        if (body == null)
            Debug.LogWarning("Body carousel returned NULL center item");

        if (legs == null)
            Debug.LogWarning("Leg carousel returned NULL center item");

        bool changed = false;

        // Only update parts if they have changed
        if (head != lastHead)
        {
            chimera.head = head;
            lastHead = head;
            changed = true;
        }

        if (body != lastBody)
        {
            chimera.body = body;
            lastBody = body;
            changed = true;
        }

        if (legs != lastLegs)
        {
            chimera.legs = legs;
            lastLegs = legs;
            changed = true;
        }

        if (changed)
        {
            DisplaySelected();
        }
    }

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
}
