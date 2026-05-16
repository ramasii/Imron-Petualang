using UnityEngine;
public class Billboard : MonoBehaviour
{
   private Camera mainCamera;
   void Start()
   {
       mainCamera = Camera.main; // Cache the main camera
   }
   void LateUpdate()
   {
       transform.LookAt(mainCamera.transform); // Rotate towards the camera
    //    transform.Rotate(0, 180, 0); // Adjust orientation if needed
   }
}