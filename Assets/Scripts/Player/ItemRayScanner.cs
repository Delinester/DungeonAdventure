using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemRayScanner : MonoBehaviour
{
    public GameObject point;

    private RaycastHit currentObject;

    private float spotDistance = 23f;
   
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.8f));

        point.transform.position = Camera.main.WorldToScreenPoint(ray.origin);        
       

        if (Physics.Raycast(ray, out hit, spotDistance))
        {
            if (hit.collider.gameObject.CompareTag("Weapon") && hit.collider.GetComponent<WeaponDataStorage>().attachment == WeaponDataStorage.WeaponAttachment.Nobody)
            {
                hit.collider.gameObject.GetComponent<WeaponUI>().ShowUI();
                currentObject = hit;  
                if (Input.GetKeyDown(KeyCode.E))
                {
                    gameObject.GetComponent<WeaponManagerPlayer>().PickUp(hit.collider.gameObject);
                }
            }
            else if (hit.collider.CompareTag("Shield"))
            {
                hit.collider.gameObject.GetComponent<WeaponUI>().ShowUI();
                currentObject = hit;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    gameObject.GetComponent<WeaponManagerPlayer>().PickUp(hit.collider.gameObject);
                }
            }
        }
        else
        {
            if (currentObject.collider != null)
            {
                if (currentObject.collider.gameObject.CompareTag("Weapon") || currentObject.collider.gameObject.CompareTag("Shield"))
                {
                    currentObject.collider.GetComponent<WeaponUI>().DisableUI();
                }
            }
        }
    }
}
