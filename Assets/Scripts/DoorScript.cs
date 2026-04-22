using System.Collections;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private bool isOpen = false;

    public Transform doorPivot;
    public float openAngle = 90f;
    public float speed = 2f;

    private Quaternion closedRot;
    private Quaternion openRot;

    void Start()
    {
        closedRot = doorPivot.rotation; //A quaternion, the rotation of the door when it's closed, determined by the doorPivot's standard rotation
        openRot = Quaternion.Euler(doorPivot.eulerAngles + new Vector3(0, openAngle, 0)); //the rotation of the door when it's open, determined by the doorPivot's standard rotation + a vector 
    }

    void Update()
    {
        Quaternion targetRot = isOpen ? openRot : closedRot; //set the targetRot quaternion to openRot or closedRot depending on the value of the isOpen bool

        doorPivot.rotation = Quaternion.Lerp(
            doorPivot.rotation,
            targetRot,
            Time.deltaTime * speed
        );
    }

    public void Open()
    {
        isOpen = !isOpen;
        StartCoroutine(AutoClose());
    }

    private IEnumerator AutoClose() //automatically closes the door after 3 seconds
    {
        yield return new WaitForSeconds(3);
        isOpen = false;
    }
}
