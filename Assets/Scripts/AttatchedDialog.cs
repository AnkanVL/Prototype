using UnityEngine;
using UnityEngine.UI;

//This is a secondary script that needs to be called from another script. Still a bit buggy, needs fixin'
public class AttatchedDialog : MonoBehaviour
{
    public bool dialogInProgress;
    public bool dialogFinished;
    [Header("Last string is not visable during gameplay")]
    public string[] strings;
    private int stringNmr = 0;
    public Text dialogText;
    
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
                dialogText.text = this.strings[stringNmr +1].ToString(); //update DialogText to the correct string
                
                if (stringNmr < strings.Length-2) //increase stringNmr by 1 if stringNmr is less than the lenght of the strings array
                {
                    stringNmr++;
                }

                else if (stringNmr == strings.Length-2)
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
        dialogText.text = this.strings[0].ToString();
    }

    public void StopDialog()
    {

    }

}
