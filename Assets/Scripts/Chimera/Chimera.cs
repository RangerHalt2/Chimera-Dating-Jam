// Created By: Ryan Lupoli
// Used to store and interpret data on the player's Chimera
using UnityEngine;

public class Chimera : MonoBehaviour
{
    public ChimeraPart head = null;
    public ChimeraPart body = null;
    public ChimeraPart legs = null;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearParts()
    {
        head = null;
        body = null;
        legs = null;
    }
}
