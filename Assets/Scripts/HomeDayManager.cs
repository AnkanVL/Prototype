using UnityEngine;

public class HomeDayManager : MonoBehaviour
{

    public ObjectiveManager manager;

    public GameObject day1;
    public GameObject day2;
    public GameObject day3;
    public GameObject day4;

    public GameObject orphanDay2, orphanDay2Alt, orphanDay3, orphanDay3Alt, orphanDay3Alt2;
    public GameObject motherDay2, motherDay2Alt, motherDay3, motherDay3Alt;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(manager.day != 1)
        {
            day1.gameObject.SetActive(false);
        }

        if(manager.day == 2)
        {
            day2.gameObject.SetActive(true);
            
            if (manager.orphanDay1)
            {
                orphanDay2.gameObject.SetActive(false);
                orphanDay2Alt.gameObject.SetActive(true);
            }

            if (manager.talkedToMotherDay1)
            {
                motherDay2.gameObject.SetActive(false);
                motherDay2Alt.gameObject.SetActive(true);
            }
        }

      else if (manager.day == 3)
        {
            day3.gameObject.SetActive(true);

            if(manager.hasPortrait)
            {
                orphanDay3.gameObject.SetActive(false);
                orphanDay3Alt.gameObject.SetActive(true);

            }
            else if(manager.orphanDay2 && !manager.hasPortrait)
            {
                orphanDay3.gameObject.SetActive(false);
                orphanDay3Alt2.gameObject.SetActive(true);
            }

            if(manager.talkedToMotherDay1 && manager.talkedToMotherDay2)
            {
                motherDay3.gameObject.SetActive(false);
                motherDay3Alt.gameObject.SetActive(true);
            }

            //Lady will give you key if you talked to her on day1&2


        }

      else if(manager.day == 4)
        {
            day4.gameObject.SetActive(true);

            if (manager.saidNoToOrphan)
            {
                // set active door where he won't respond
            }
            else if (manager.saidNoToOrphan == false)
            {
                // set active door where he is sad
            }

            if (manager.gaveMedicineToMother)
            {
                //set active where she is sad but thankful
            }
            else if (manager.gaveMedicineToMother == false)
            {
                //set active door where she is crazy
            }
        }
    }

    
}
