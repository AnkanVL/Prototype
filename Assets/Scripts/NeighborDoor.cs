using System.Collections;
using UnityEngine;

public class NeighborDoor : MonoBehaviour
{
    private bool isOpen = false;

    public Transform doorPivot;
    private float openAngle = 45f;
    public float speed = 2f;

    private Quaternion closedRot;
    private Quaternion openRot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        closedRot = doorPivot.rotation;
        openRot = Quaternion.Euler(doorPivot.eulerAngles + new Vector3(0, openAngle, 0));
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion targetRot = isOpen ? openRot : closedRot;

        doorPivot.rotation = Quaternion.Lerp(
            doorPivot.rotation,
            targetRot,
            Time.deltaTime * speed
        );
    }

    private void PlaySequence()
    {
        
    }

    private IEnumerator Knock()
    {
        //play sound
        yield return new WaitForSeconds(1.5f);
        isOpen = true;
        yield return new WaitForSeconds(0.5f);
        PlaySequence();
    }
}

/* Play knock sound, wait 1 second
 * open the door
 * Dialog 
 * close door
 * 
 * 
 */