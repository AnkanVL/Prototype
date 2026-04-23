using System.Collections;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    
    public AttatchedDialog attatchedDialog;
    //public MonoBehaviour interaction; //
    public ObjectiveManager objectives;

    public enum ObjectiveType //choose what switch case will activate after dialog
    {
        None,
        TalkedToNeighbor,
        ExternalScriptTest
    }

    public ObjectiveType objectiveToSet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attatchedDialog = GetComponent<AttatchedDialog>();
    }

    // Update is called once per frame
    void Update()
    {
        

        //closes door if dialog is finished
        if (attatchedDialog.dialogFinished)
        {
            PostDialog();
        }
    }

    public void StartSequence() //is called from playerScript when player interacts with the door
    {
        if (!attatchedDialog.dialogInProgress)
        {
            attatchedDialog.RunDialog();
        }

    }

    public void PostDialog() //what will happen after dialog is over
    {
        switch (objectiveToSet)
        {
            case ObjectiveType.None:
                break;

            case ObjectiveType.TalkedToNeighbor:
                objectives.talkedToNeighbor = true;
                break;

            case ObjectiveType.ExternalScriptTest:
                objectives.talkedToNeighbor2 = true;
                break;
        }
    }
}