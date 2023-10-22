using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class textCoords : MonoBehaviour
{
    public Transform targetObject; 
    public TextMeshProUGUI textMeshPro;

    void Start()
    {
        
    }

    void Update()
    {
        if (targetObject != null && textMeshPro != null)
        {
            Vector3 currentPosition = targetObject.position;
            string coordinatesText = string.Format("X: {0:0.00}\nY: {1:0.00}\nZ: {2:0.00}", currentPosition.x, currentPosition.y, currentPosition.z);
            textMeshPro.text = coordinatesText;
        }
    }
}
