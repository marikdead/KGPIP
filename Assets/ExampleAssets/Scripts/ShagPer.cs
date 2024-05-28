using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShagPer : MonoBehaviour
{

    public Transform cameraPosition;
    private TextMeshProUGUI inputField;
    private List<GameObject> locations;

    private Dictionary<string, string> shag;

    // Start is called before the first frame update
    void Start()
    {
        shag = new Dictionary<string, string>();

        inputField = GetComponent<TextMeshProUGUI>();
        GameObject[] vidArray = GameObject.FindGameObjectsWithTag("L1");
        locations = new List<GameObject>(vidArray);

        shag.Add("Link1-4","Поверните направо и выйдете на лестницу");
        shag.Add("Link4-8","Поднимитесь на этаж выше");
        shag.Add("Link8-12","Поднимитесь на этаж выше");
        shag.Add("Link12-14","Поднимитесь на этаж выше");
        shag.Add("Link14-19","Поднимитесь на этаж выше");
        shag.Add("Link19-21","Выйдете на этаж и идите направо");
    }

    // Update is called once per frame
    void Update()
    {
        float minDist = 10000000000000000;
        string thislocation = "";

        foreach(GameObject location in locations){

            float thisDist = Vector3.Distance(cameraPosition.position, location.transform.position);

            if(thisDist < minDist){
                thislocation = location.name;
                minDist = thisDist;
            }

        }

        inputField.text = shag[thislocation];
    }
}
