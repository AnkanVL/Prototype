using UnityEngine;
using UnityEngine.UI;

//WARNING! this DialogSystem is kinda bad and might cause problems in the future. It's not very versitile
public class DialogSystem : MonoBehaviour
{
    public bool canTalk;
    public bool isTalking;
    //public Image TextBox;
    public Text E;
    public Text Dialogtext;
    public bool isOptional;
    public bool movementDisabled;
    public bool notRepeatable;
    [Header("Last string is not visable during gameplay")]
    public string[] strings;
    private int stringNmr = 1;
    public GameObject player;
    public PlayerScript playerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponentInParent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E) && canTalk && isOptional) //Start optional dialog
        {
            //TextBox.gameObject.SetActive(true);
            Dialogtext.gameObject.SetActive(true);
            this.stringNmr = 0;
            isTalking = true;
            canTalk = false;

            if (movementDisabled)
            {
                playerScript.canMove = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.E) && isTalking) //Progress through dialog
        {
            //Debug.Log(stringNmr);
            Dialogtext.text = this.strings[stringNmr].ToString(); //update DialogText to the correct string

            if (stringNmr < strings.Length - 1)
            {
                stringNmr++;
            }

            else if (stringNmr == strings.Length - 1)
            {
                isTalking = false;
                //TextBox.gameObject.SetActive(false);
                Dialogtext.gameObject.SetActive(false);
                this.stringNmr = 0;
                canTalk = true;
                playerScript.canMove = true;

                if (notRepeatable)
                {
                    gameObject.SetActive(false);
                    E.gameObject.SetActive(false);
                }
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (isOptional) //Notify for possible dialog
        {
            E.gameObject.SetActive(true);
            canTalk = true;
        }

        else //automaticaly start dialog
        {
            isTalking = true;
            //TextBox.gameObject.SetActive(true);
            Dialogtext.gameObject.SetActive(true);
            Dialogtext.text = strings[0];

            if (movementDisabled)
            {
                playerScript.canMove = false;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        E.gameObject.SetActive(false);
        //TextBox.gameObject.SetActive(false);
        Dialogtext.gameObject.SetActive(false);
        this.stringNmr = 0;
        canTalk = false;
    }
}
