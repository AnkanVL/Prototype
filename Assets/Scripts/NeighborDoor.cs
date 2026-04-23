using System.Collections;
using UnityEngine;

public class NeighborDoor : MonoBehaviour
{
    private bool isOpen = false;

    public Transform doorPivot;
    private float openAngle = 45f;
    public float speed = 2f;
    public AttatchedDialog attatchedDialog;
    //public MonoBehaviour interaction; //
    public ObjectiveManager objectives;

    private Quaternion closedRot;
    private Quaternion openRot;

    public enum ObjectiveType //choose what objective to update
    {
        None,
        TalkedToNeighbor,
        TalkedToNeighbor2
    }

    public ObjectiveType objectiveToSet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        closedRot = doorPivot.rotation;
        openRot = Quaternion.Euler(doorPivot.eulerAngles + new Vector3(0, openAngle, 0));
        attatchedDialog = GetComponent<AttatchedDialog>();
    }

    // Update is called once per frame
    void Update()
    {
        //keeps track of the door rotation
        Quaternion targetRot = isOpen ? openRot : closedRot;

        doorPivot.rotation = Quaternion.Lerp(
            doorPivot.rotation,
            targetRot,
            Time.deltaTime * speed
        );

        //closes door if dialog is finished
        if (attatchedDialog.dialogFinished)
        {
            isOpen = false;
            PostDialog();
        }
    }

    public void StartKnockCoroutine() //is called from playerScript when player interacts with the door
    {
        if (!attatchedDialog.dialogInProgress)
        {
            StartCoroutine(Knock());
        }
        
    }

    public void PlaySequence()
    {
        attatchedDialog.RunDialog(); //runs dialog from the AttatchedDialog script
    }

    public IEnumerator Knock()
    {
        //play knock sound
        yield return new WaitForSeconds(1.5f);
        attatchedDialog.dialogInProgress = true;
        attatchedDialog.dialogFinished = false;
        isOpen = true;
        yield return new WaitForSeconds(0.5f);
        PlaySequence();
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

            case ObjectiveType.TalkedToNeighbor2:
                objectives.talkedToNeighbor2 = true;
                break;
        }
    }
}

/* Play knock sound, wait 1 second
 * open the door
 * Dialog 
 * close door
 * 
 * 
 */