using UnityEngine;

// This script holds all objectives

// Note: Every scene should have a LevelManager object/script that decides what gameObjects should be active, based on the bool values in this script

// future Improvement: All variables in this script could get made static for easier access
public class ObjectiveManager : MonoBehaviour
{
    public int day = 1;
    public bool talkedToNeighbor;
    public bool talkedToNeighbor2;
    public bool talkedToNPC;

    //Orphan objectives
    public bool orphanDay1;
    public bool orphanDay2;
    public bool hasPortrait;
    public bool saidNoToOrphan;

    //Mother objectives
    public bool talkedToMotherDay1;
    public bool talkedToMotherDay2;
    public bool gaveMedicineToMother;

    

    //Life of NPC's
    public static int neighbor1Life = 2;
    public static int neighbor2Life = 2;
    public static int neighbor3Life = 2;
    public static int neighbor4Life = 2;
    public static int neighbor5Life = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

}
