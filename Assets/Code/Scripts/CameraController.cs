using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera[] cameras;

    private int selectedIndex = 0;

    void Start()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            if (i != selectedIndex) cameras[i].enabled = false;
        }
    }

    public void changeCamera(Camera camera)
    {
        selectedIndex = System.Array.IndexOf(cameras, camera);
        changeCamera(selectedIndex);
    }

    public void changeCamera(int cameraIndex)
    {
        selectedIndex = cameraIndex;
        for(int i = 0; i < cameras.Length; i++)
        {
            if (i == cameraIndex) cameras[i].enabled = true;
            else cameras[i].enabled = false;
        }
    }

    public GameObject getActiveCameraGameObject()
    {
        return getActiveCamera().gameObject;
    }

    public Camera getActiveCamera()
    {
        return cameras[selectedIndex];
    }
}
