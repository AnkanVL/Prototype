using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class NewMonoBehaviourScript : MonoBehaviour
{
     readonly string[] GhostTypes = new string[5] { "Banshee", "Poltergeist", "Phantom", "Specter", "Wraith" };
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      Randomizer GhostTypeRandomizer = new Randomizer();
    }

    void GhostType(string ghostType)
    {
        switch (ghostType)
        {
            case "Banshee":
                Debug.Log("The Banshee is a wailing spirit that is often associated with death and mourning. It is said to be able to predict the death of a loved one by emitting a mournful cry.");
                break;
            case "Poltergeist":
                Debug.Log("The Poltergeist is a mischievous spirit that is known for causing disturbances and making noise. It is said to be able to move objects and create chaos in its wake.");
                break;
            case "Phantom":
                Debug.Log("The Phantom is a ghostly apparition that is often associated with haunting and supernatural occurrences. It is said to be able to appear and disappear at will, and can sometimes be seen as a shadowy figure.");
                break;
            case "Specter":
                Debug.Log("The Specter is a ghostly entity that is often associated with fear and terror. It is said to be able to instill fear in those who encounter it, and can sometimes be seen as a dark, shadowy figure.");
                break;
            case "Wraith":
                Debug.Log("The Wraith is a ghostly being that is often associated with death and the afterlife. It is said to be able to drain the life force from its victims, leaving them weak and vulnerable.");
                break;
            case "The Shadow People":
                Debug.Log("The Shadow People are a mysterious and elusive group of entities that are often associated with fear and the unknown. They are said to be able to move through walls and disappear without a trace, making them difficult to detect.");
                break;
            case "The Stalker":
                Debug.Log("The Stalker is a ghostly entity that is often associated with obsession and stalking. It is said to be able to follow its victims and haunt them relentlessly but only be able to move when seen, causing fear and anxiety.");
                break;

        }
    }
}