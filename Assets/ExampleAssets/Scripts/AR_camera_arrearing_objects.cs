using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR_camera_arrearing_objects : MonoBehaviour
{

    public Transform arCamera;
    public float maxDistance = 1f;
    private List<GameObject> vidObjects;

    // Start is called before the first frame update
    void Start()
    {

        GameObject[] vidArray = GameObject.FindGameObjectsWithTag("appearing");
        vidObjects = new List<GameObject>(vidArray);

    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject obj in vidObjects)
        {
            if (obj != null)
            {
                float distance = Vector3.Distance(arCamera.position, obj.transform.position);

                if (distance <= maxDistance)
                {
                    obj.SetActive(true);
                }
                else
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}
