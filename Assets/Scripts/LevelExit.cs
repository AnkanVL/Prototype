using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public AttatchedDialog attatchedDialog;
    public PlayerInventory inventory;
    public int sceneToLoad;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        attatchedDialog = GetComponent<AttatchedDialog>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(inventory.food == 5)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            attatchedDialog.RunDialog();
        }
    }
}
