using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (cam != null)
        {
            // Make this transform face the camera
            transform.LookAt(transform.position + cam.transform.forward);
        }
    }
}
