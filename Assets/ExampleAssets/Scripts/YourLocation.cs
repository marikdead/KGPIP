using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class YourLocation : MonoBehaviour
{
    public Transform cameraPosition;
    private TextMeshProUGUI inputField;
    private List<GameObject> locations;

    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponent<TextMeshProUGUI>();
        GameObject[] vidArray = GameObject.FindGameObjectsWithTag("appearing");
        locations = new List<GameObject>(vidArray);
    }

    // Update is called once per frame
    void Update()
    {
        float minDist = 10000000000000000;
        string thislocation = "";

        foreach(GameObject location in locations){

            float thisDist = Vector3.Distance(cameraPosition.position, location.transform.position);
            //Debug.Log(location.name + "    " + thisDist);

            if(thisDist < minDist){
                thislocation = location.name;
                minDist = thisDist;
            }

        }

        inputField.text = thislocation;
    }
}