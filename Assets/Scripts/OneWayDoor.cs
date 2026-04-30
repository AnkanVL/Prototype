using UnityEngine;

public class OneWayDoor : MonoBehaviour
{
    public GameObject lockedDoor;
    public GameObject door;
    private bool isLocked = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (isLocked)
            {
                isLocked = false;
                lockedDoor.SetActive(false);
                door.SetActive(true);
            }
        }
        
    }
}
