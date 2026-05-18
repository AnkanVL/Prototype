using UnityEngine;

public class GiveFoodScript : MonoBehaviour
{
    public GameObject lockedDoor;
    public GameObject foodDoor;
    public GameObject foodObject;

    public enum NPCType //choose what switch case will activate after dialog
    {
        None,
        NPC1,
        NPC2,
        NPC3
    }

    public NPCType npcToGiveFoodTo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerInventory.foodLeftToGive == 0) //replace fooddoor with prop door if you don't have any food left to give
        {
            ChangeDoor();
        }
    }

    public void GiveFood()
    {
        switch (npcToGiveFoodTo)
        {
            case NPCType.None:
                break;

            case NPCType.NPC1:
                ObjectiveManager.neighbor1Life++; //add 1 life to the neighbor
                PlayerInventory.foodLeftToGive--; //subtract 1 food 
                ChangeDoor();
                //play place sound here
                foodObject.gameObject.SetActive(true); //place foodCan object
                break;

            case NPCType.NPC2:
                ObjectiveManager.neighbor2Life++;
                PlayerInventory.foodLeftToGive--;
                ChangeDoor();
                //play place sound here
                foodObject.gameObject.SetActive(true);
                break;
        }
    }

    private void ChangeDoor()
    {
        foodDoor.gameObject.SetActive(false);
        lockedDoor.gameObject.SetActive(true);
        
    }
}
