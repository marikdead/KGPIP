using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Android;
using TMPro;

public class GPS : MonoBehaviour
{
    public TextMeshProUGUI gpsOut;
    private bool isUpdating;

    void Start()
    {
        Debug.Log("Start method");

        if (!isUpdating)
        {
            StartCoroutine(GetLocation());
            isUpdating = !isUpdating;
        }
    }

    void SetInitialCoordinates(float latitude, float longitude)
    {
        float altitude = 100f; // Твоя высота
        //transform.position = new Vector3(longitude, latitude, altitude);

        UpdateGPSOut();
    }

    void UpdateGPSOut()
    {
        gpsOut.text = "Location: " + transform.position.x.ToString("F6") + " | " + transform.position.y.ToString("F6") + " | " + transform.position.z.ToString("F6");
        //Debug.Log("Location: " + transform.position.x + " | " + transform.position.y + " | " + transform.position.z);
    }

    void Update()
    {
        // Проверяем изменения позиции и обновляем текст
        if (transform.hasChanged)
        {
            UpdateGPSOut();
            transform.hasChanged = false;
        }
    }

    IEnumerator GetLocation()
    {
        Debug.Log("GetLocation method");

        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            //Debug.Log("Requesting permissions");
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }

        if (!Input.location.isEnabledByUser)
        {
            //Debug.Log("Location services not enabled, waiting 10 seconds");
            yield return new WaitForSeconds(10);
        }

        Input.location.Start();

        int maxWait = 10;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            //Debug.Log("Waiting for GPS initialization...");
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            //Debug.Log("Timed out");
            gpsOut.text = "Timed out";
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            //Debug.Log("Failed to determine device location");
            gpsOut.text = "Unable to determine device location";
            yield break;
        }

        // Вызываем SetInitialCoordinates при старте, после получения первых данных
        SetInitialCoordinates(Input.location.lastData.latitude, Input.location.lastData.longitude);

        while (isUpdating)
        {
            yield return null;
        }
    }
}
