using UnityEngine;
using UnityEngine.UI;

//This is a secondary script that needs to be called from another script
public class AttatchedDialog : MonoBehaviour
{
    public bool dialogInProgress;
    public bool dialogFinished;
    public string[] strings;
    private int stringNmr = 1;
    public Text dialogText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogInProgress)
        {
            if (Input.GetKeyDown(KeyCode.E) && dialogInProgress)
            {
                dialogText.text = this.strings[stringNmr].ToString(); //update DialogText to the correct string
                
                if (stringNmr < strings.Length-1)
                {
                    stringNmr++;
                }

                else if (stringNmr == strings.Length-1)
                {
                    dialogInProgress = false;
                    dialogFinished = true;
                    dialogText.gameObject.SetActive(false);
                }
            }
        }
    }

    public void RunDialog()
    {
        dialogInProgress = true;
        dialogFinished = false ;
        this.stringNmr = 0;
        dialogText.gameObject.SetActive(true);
    }

}
