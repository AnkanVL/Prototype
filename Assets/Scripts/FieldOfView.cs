
using System.Collections;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius = 8f;
    [Range(0, 360)]
    public float viewAngle = 130f;
    public Transform target; //Referens till spelaren
    public bool canSeeMonster; //Bool som blir sann om NPC kan se spelaren
    public LayerMask targetMask; //Layermasks för spelare och blockerande hinder
    public LayerMask obstructionMask;
    [HideInInspector]
    public Collider[] objectsInView; //Array där alla objekt som befinner sig inom overlapsfären samlas


    void Start()
    {
        StartCoroutine(FOVtimer()); //Starta timern
    }


    private IEnumerator FOVtimer()
    {
        FieldOfViewCheck(); //Kalla på FOV-funktionen
        yield return new WaitForSeconds(0.5f); //Vänta en halv sekund
        StartCoroutine(FOVtimer()); //Återstarta timern
    }

    private void  FieldOfViewCheck()
    {
        //Skapa en overlapsfär vid spelarens pivotpunkt och låt den endast undersöka targetmask
        objectsInView = Physics.OverlapSphere(transform.position, viewRadius,targetMask);

        if (objectsInView.Length != 0) //Kolla om någon collider finns i arrayen
        {
            //Räkna ut riktningen mot målet
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            //Kolla om målet ligger framför eller bakom NPC
            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                //Räkna ut avståndet till målet
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                //Skjut en ray från NPC mot målet. Titta i masken om hinder finns
                if (Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeeMonster = false; //Inga hinder,spelaren är synlig
                }
                else
                {
                    canSeeMonster = true; //Spelaren är inte synlig
                }
            }
            else
            {
                canSeeMonster = false; //Målet befinner sig utanför synfältet
            }
        }
        else
            {
                canSeeMonster = false;
            }
    }

    public bool CanSeePoint(Vector3 point)
{
    Vector3 directionToPoint =
        (point - transform.position).normalized;

    // Angle check
    if(Vector3.Angle(
        transform.forward,
        directionToPoint) < viewAngle / 2)
    {
        float distanceToPoint =
            Vector3.Distance(transform.position, point);

        // Obstruction check
        if(!Physics.Raycast(
            transform.position,
            directionToPoint,
            distanceToPoint,
            obstructionMask))
        {
            return true;
        }
    }

    return false;
}
}
