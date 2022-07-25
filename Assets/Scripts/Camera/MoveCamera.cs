using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float rotationSpeed = 700f;
    public GameObject followTarget;
    [SerializeField]
    private LayerMask layerMask;
    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");        
       
        Vector3 rotation = new Vector3(mouseY, mouseX, 0);
        transform.Rotate(rotation, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);

        transform.position = followTarget.transform.position;
        
        if (transform.rotation.eulerAngles.x > 40 && transform.rotation.eulerAngles.x < 50)
        {
            
            transform.rotation = Quaternion.Euler(40, transform.rotation.eulerAngles.y, 0);
            
        }
        if (transform.rotation.eulerAngles.x < 330 && transform.rotation.eulerAngles.x > 320)
        {
            transform.rotation = Quaternion.Euler(330, transform.rotation.eulerAngles.y, 0);
        }

        CheckRaycast();
    }
   private void CheckRaycast()
    {
        Vector3 cameraPosition = GetComponentInChildren<Camera>().gameObject.transform.position;
        Vector3 startPosOffset = new Vector3(0, 4, 0);
        Vector3 startPos = followTarget.transform.position + startPosOffset;

        Debug.DrawLine(startPos, cameraPosition, Color.red);
        if (Physics.Linecast(startPos, cameraPosition, out RaycastHit hit, layerMask))
        {
            Vector3 distanceToMove = hit.point - cameraPosition;
            transform.position += distanceToMove; 
        }
    }
}
