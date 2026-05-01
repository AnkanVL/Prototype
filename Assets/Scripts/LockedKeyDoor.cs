using UnityEngine;
using static PickUp;

public class LockedKeyDoor : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public GameObject lockedDoor;
    public GameObject door;

    public enum CorrectKey //choose what which item to pick up to update
    {
        None,
        level1Key,

    }
    public CorrectKey KeyToDoor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    // Update is called once per frame
    void Update()
    {
        switch (KeyToDoor)
        {
            case CorrectKey.None:
                break;

            case CorrectKey.level1Key:
                if(playerInventory.level1Key == true)
                {
                    lockedDoor.SetActive(false);
                    door.SetActive(true);
                }
                break;
        }
    }
}
