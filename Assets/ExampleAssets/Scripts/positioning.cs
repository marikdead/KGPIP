using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class positioning : MonoBehaviour
{
    public Transform targetObject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (targetObject != null)
        {
            // Получаем текущую позицию целевого объекта
            Vector3 targetPosition = targetObject.position;

            // Перемещаем этот объект на 10 единиц выше по оси Y относительно целевого объекта
            Vector3 newPosition = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z + 5f);

            // Устанавливаем новую позицию для этого объекта
            transform.position = newPosition;
        }
    }
}
