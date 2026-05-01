using UnityEngine;

public class PickUp : MonoBehaviour
{
    public PlayerInventory playerInventory;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public enum ItemType //choose what which item to pick up
    {
        None,
        level1Key,
        food,
        
    }
    public ItemType ItemToPickUp;

    public void PickUpItem() //choose what will happen after you pick up selected enum item
    {
        gameObject.SetActive(false);

        switch (ItemToPickUp)
        {
            case ItemType.None:
                break;

            case ItemType.level1Key:
                playerInventory.level1Key = true;
                break;

            case ItemType.food:
                playerInventory.food++;
                break;
        }
        
    }
}
