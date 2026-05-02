using System.Collections;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    
    public AttatchedDialog attatchedDialog;
    //public MonoBehaviour interaction; //
    public ObjectiveManager objectives;
    private bool hasRunPostDialog = false;

    public enum ObjectiveType //choose what switch case will activate after dialog
    {
        None,
        TalkedToNeighbor,
        ExternalScriptTest
    }

    public ObjectiveType objectiveToSet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (attatchedDialog == null)
            attatchedDialog = GetComponent<AttatchedDialog>();
        //attatchedDialog.dialogInProgress = false;
    }

    // Update is called once per frame
    void Update()
    {
        

        //closes door if dialog is finished
        if (attatchedDialog.dialogFinished && !hasRunPostDialog)
        {
            PostDialog();
            hasRunPostDialog = true;
        }
    }

    public void StartSequence() //is called from playerScript when player interacts with the door
    {
        if (!attatchedDialog.dialogInProgress)
        {
            hasRunPostDialog= false;
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