using UnityEngine;
using UnityEngine.UI;

//This is a secondary script that needs to be called from another script. Still a bit buggy, needs fixin'
public class AttatchedDialog : MonoBehaviour
{
    public bool dialogInProgress;
    public static bool globalDialogInProgress; //static var keeping track if you are in dialog or not, needed for PressToTalk UI check in playerScript
    public static float globalDialogCooldown = 0f; //cooldown to make sure that you don't accidentaly start repeating dialog after finishing it.
    public bool dialogFinished;
    private float inputDelay = 0.2f;
    private float inputTimer = 0f;

    public string[] strings;
    private int stringNmr = 0;
    public Text dialogText;
    
    //experimental
    public GameObject player;
    public PlayerScript playerScript;

    void Awake()
    {
        dialogFinished = false;
        dialogInProgress = false;

        //experimental
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponentInParent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (globalDialogCooldown > 0f)
        {
            globalDialogCooldown -= Time.deltaTime;
        }

        if (!dialogInProgress) return;

        inputTimer -= Time.deltaTime;

        if (inputTimer > 0f) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            stringNmr++;

            if (stringNmr < strings.Length)
            {
                dialogText.text = strings[stringNmr];
            }
            else //When dialog is over
            {
                dialogInProgress = false;
                dialogFinished = true;
                dialogText.gameObject.SetActive(false);
                playerScript.canMove = true;
                globalDialogInProgress = false;
                globalDialogCooldown = 0.2f;
            }
        }

    }

    public void RunDialog()
    {
        dialogInProgress = true;
        globalDialogInProgress = true;
        inputTimer = inputDelay;
        dialogFinished = false ;
        //this.stringNmr = 0;
        dialogText.gameObject.SetActive(true);
        //dialogText.text = this.strings[0].ToString();

        stringNmr = 0;
        dialogText.text = strings[0];
        playerScript.canMove = false;
    }

    public void StopDialog()
    {

    }

}
