using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    #region Variables
    public static UIManager instance;
    [Header("Page Management")]
    [Tooltip("The Pages (or Panels) managed by the UI Manager.")]
    public List<UIPage> pages;
    [Tooltip("The index of the currently active page in the UI.")]
    public int currentPage = 0;
    [Tooltip("The index of the page the UI should start on when the UI Manager starts up.")]
    public int defaultPage = 0;
    private int previousPage = 0;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitilizeFirstPage();
    }

    // Sets up the first page. Ensures that only the default page is enabled on startup
    private void InitilizeFirstPage()
    {
        GoToPage(defaultPage);
    }

    public void GoToPage(int pageIndex)
    {
        // If the page index is within the bounds of pages, and a page has been assigned at that index
        if (pageIndex < pages.Count && pages[pageIndex] != null)
        {
            // Disable all pages
            SetActiveAllPages(false);
            // Activate the specified page
            previousPage = currentPage;
            pages[pageIndex].gameObject.SetActive(true);
            currentPage = pageIndex;
            Debug.Log("UI MANAGER - page history & added: ");
        }
    }

    // Turns all pages on or off according to the activated parameter
    public void SetActiveAllPages(bool activated)
    {
        // If pages has at least one page assinged
        if (pages != null)
        {
            // For every UIPage in the list
            foreach (UIPage page in pages)
            {
                if (page != null)
                {
                    // Activate or deactivate the page according to the state of activated
                    page.gameObject.SetActive(activated);
                }
            }
        }
    }
}