using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class textCoords : MonoBehaviour
{
    public Transform targetObject; // Объект, координаты которого вы хотите отобразить
    public TextMeshProUGUI textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targetObject != null && textMeshPro != null)
        {
            // Получаем текущие координаты объекта
            Vector3 currentPosition = targetObject.position;

            // Форматируем текст с координатами
            string coordinatesText = string.Format("X: {0:0.00}\nY: {1:0.00}\nZ: {2:0.00}", currentPosition.x, currentPosition.y, currentPosition.z);

            // Устанавливаем новый текст в компонент TextMeshPro
            textMeshPro.text = coordinatesText;
        }
    }
}
