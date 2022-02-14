using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lerp_ex1 : MonoBehaviour
{

    // Variables pour le déplacement
    private Vector3 destination;
    public float vitesse;
    public float duree;
    private bool lerpActif=false;

    // Variable pour la rotation
    public Transform Target;
    public float RotationSpeed;
    private Quaternion _lookRotation;
    private Vector3 _direction;

    void Start()
    {
        //StartCoroutine(MoveOverSpeed(gameObject, destination, vitesse));
        //StartCoroutine(MoveOverSeconds(gameObject, destination, duree));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !lerpActif)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                destination = hit.point;
                lerpActif = true;
                
                // DÉPLACEMENT
                StartCoroutine(MoveOverSpeed(gameObject, destination, vitesse));
                //StartCoroutine(MoveOverSeconds(gameObject, destination, duree));
            }
        }


        if (lerpActif)
        {
            // ROTATION
            //find the vector pointing from our position to the target
            _direction = (destination - transform.position).normalized;

            //create the rotation we need to be in to look at the target
            _lookRotation = Quaternion.LookRotation(_direction);

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
        }
    }

    // Déplace l'objet jusqu'à la destination (end) selon la vitesse (peu importe le temps)
    public IEnumerator MoveOverSpeed(GameObject objectToMove, Vector3 end, float speed)
    {
        // speed should be 1 unit per second
        while (objectToMove.transform.position != end)
        {
            objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, end, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        lerpActif = false;
    }
    // Déplace l'objet jusqu'à la destination (end) selon le temps maximum (ajustera la vitesse)
    public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
        lerpActif = false;
    }

}
