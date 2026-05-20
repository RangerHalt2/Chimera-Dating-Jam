// Created By: Ryan Lupoli
// Allows for a scroll rect to infinitely cycle through a series of objects, similar to a carousel
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScrollRect : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to the ScrollRect controlling the drag movement.")]
    public ScrollRect scrollRect;
    [Tooltip("Reference to the viewport the content will be seen through.")]
    public RectTransform viewport;
    [Tooltip("Reference to the parent game object of the content.")]
    public RectTransform content;
    [Tooltip("Reference to the item prefab used by the content.")]
    public GameObject itemPrefab;

    [Header("Settings")]
    [Min(3)]
    [Tooltip("The number of items which should be visible at any given time.")]
    public int visibleItemCount = 3;
    [Tooltip("The width of each content item. This value should match the itemPrefab EXACTLY!")]
    public float itemWidth = 150f;

    [Header("Data")]
    [Tooltip("List of all sprites the InfiniteScrollRect should use")]
    //public List<Sprite> assets = new();
    public List<ChimeraPart> assets = new();

    // Cached list of active recycled UI items
    private readonly List<RectTransform> items = new();

    // Tracks which data index currently exists as the far left of the carousel
    private int leftDataIndex = 0;


    void OnValidate()
    {
        if (visibleItemCount < 3)
            visibleItemCount = 3;

        if (visibleItemCount % 2 == 0)
            visibleItemCount += 1;
    }

    void Start()
    {

    }

    void Update()
    {
        if (items == null || items.Count == 0)
            return;

        HandleLooping();
    }

    #region Initialization
    void Build()
    {
        if (assets == null)
        {
            Debug.LogWarning("InfiniteScrollRect: Cannot build! Assets List is null!");
            return;
        }

        if (assets.Count == 0)
        {
            Debug.LogWarning("InfiniteScrollRect: Cannot build! Assets List is empty!");
            return;
        }

        Debug.Log($"InfiniteScrollRect: Building carousel with {assets.Count} assets");

        // Remove all exising children (allows for rebuilding)
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        items.Clear();

        // Create polled visible items
        for (int i = 0; i < visibleItemCount; i++)
        {
            // Instantiate itemPrefab
            GameObject obj = Instantiate(itemPrefab, content);

            RectTransform rt = obj.GetComponent<RectTransform>();

            // Position horizontally
            rt.anchoredPosition = new Vector2(i * itemWidth, 0f);

            items.Add(rt);

            // Determine which data should appear
            int dataIndex = Mod(i, assets.Count);

            // Bind sprite/data to item
            SetupItem(obj, dataIndex);
        }

        // Center content initially
        float startX = -(visibleItemCount / 2f) * itemWidth;

        content.anchoredPosition = new Vector2(startX, 0f);
    }
    #endregion

    #region Infinite Recycle Logic
    // Detects when an item leaves the viewport and moves it to the opposite side
    void HandleLooping()
    {
        // Find half the width of a single item
        float halfWidth = itemWidth * 0.5f;

        // Find half the width of the viewport
        float viewportHalfWidth = viewport.rect.width * 0.5f;

        for (int i = 0; i < items.Count; i++)
        {
            RectTransform item = items[i];

            // Convert item center into viewport local space
            // Assists in determining when an item is visible
            Vector3 localPos = viewport.InverseTransformPoint(item.position);

            // Calculate item edges
            float itemLeft = localPos.x - halfWidth;
            float itemRight = localPos.x + halfWidth;

            // Fully outside LEFT side
            if (itemRight < -viewportHalfWidth)
            {
                MoveItemToRight(item);
            }

            // Fully outside RIGHT side
            else if (itemLeft > viewportHalfWidth)
            {
                MoveItemToLeft(item);
            }
        }
    }
    #region Recycling
    // Recylces an item fromm Left to Right
    void MoveItemToRight(RectTransform item)
    {
        // Find current rightmost item
        RectTransform rightMost = GetRightMost();

        // Position recycled item directly after the rightmost item
        float newX = rightMost.anchoredPosition.x + itemWidth;

        item.anchoredPosition = new Vector2(newX, 0f);

        // Advance the data index
        leftDataIndex = Mod(leftDataIndex + 1, assets.Count);

        // Determine new data for recycled item
        int newDataIndex = Mod(leftDataIndex + visibleItemCount - 1, assets.Count);

        // Apply new data
        SetupItem(item.gameObject, newDataIndex);
    }

    // Recylces an item fromm Right to Left
    void MoveItemToLeft(RectTransform item)
    {
        // Find current leftmost item
        RectTransform leftMost = GetLeftMost();

        // Postition recylced item directly before leftmost item
        float newX = leftMost.anchoredPosition.x - itemWidth;

        item.anchoredPosition = new Vector2(newX, 0f);

        // Move data index backwards
        leftDataIndex = Mod(leftDataIndex - 1, assets.Count);

        // New left most item is current leftDataIndex
        int newDataIndex = leftDataIndex;

        // Apply new data
        SetupItem(item.gameObject, newDataIndex);
    }
    #endregion
    #endregion

    #region Utility
    public void Rebuild()
    {
        Build();
    }

    // Return leftmost visible item
    RectTransform GetLeftMost()
    {
        RectTransform left = items[0];

        foreach (RectTransform item in items)
        {
            if (item.anchoredPosition.x < left.anchoredPosition.x)
            {
                left = item;
            }
        }

        return left;
    }

    // Return rightmost visible item
    RectTransform GetRightMost()
    {
        RectTransform right = items[0];

        foreach (RectTransform item in items)
        {
            if (item.anchoredPosition.x > right.anchoredPosition.x)
            {
                right = item;
            }
        }

        return right;
    }
    
    // Assigns data to a carousel item
    void SetupItem(GameObject obj, int assetIndex)
    {
        ChimeraPart part = assets[assetIndex];

        Image image = obj.GetComponent<Image>();

        if (image != null && assets.Count > 0)
        {
            image.sprite = assets[assetIndex].partSprite;
        }

        CarouselItem item = obj.GetComponent<CarouselItem>();

        if (item != null )
        {
            item.AssignedPart = part;
        }
    }

    // Modulo which supports negative numbers
    int Mod(int x, int m)
    {
        return (x % m + m) % m;
    }

    // Returns the RectTransform of the currently closest object to the center
    public RectTransform GetCenteredItem()
    {
        //Debug.Log($"ITEM COUNT: {items.Count}");
        //Debug.Log($"VIEWPORT NULL? {viewport == null}");

        if (items == null || items.Count == 0)
            return null;

        RectTransform closest = null;
        float closestDistance = float.MaxValue;

        Vector3 viewportCenterWorld = viewport.position;

        foreach (RectTransform item in items)
        {
            if (item == null)
                continue;

            float distance = Mathf.Abs(item.position.x - viewportCenterWorld.x);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = item;
            }
            //Debug.Log($"Item {item}: {item.name} pos = {item.position}");
        }

        return closest;
    }

    public ChimeraPart GetCenteredPart()
    {
        RectTransform centered = GetCenteredItem();

        if (centered == null)
        {
            return null;
        }

        CarouselItem item = centered.GetComponent<CarouselItem>();

        if (item == null) 
        {
            return null;
        }

        if (item.AssignedPart == null)
        {
            Debug.LogWarning($"CarouselItem on {centered.name} has NULL AssignedPart");
            return null;
        }

        return item.AssignedPart;
    }
    #endregion
}