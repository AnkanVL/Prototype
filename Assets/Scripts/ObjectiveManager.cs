using UnityEngine;

// This script holds all objectives

// Note: Every scene should have a LevelManager object/script that decides what gameObjects should be active, based on the bool values in this script

// future Improvement: All variables in this script could get made static for easier access
public class ObjectiveManager : MonoBehaviour
{
    public int day;
    public bool talkedToNeighbor;
    public bool talkedToNeighbor2;
    public bool talkedToNPC;
    
    public static int neighbor1Life = 2;
    public static int neighbor2Life = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

}
