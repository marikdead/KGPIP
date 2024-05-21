using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mono.Data.Sqlite;
using System.IO;
using System.Data;

public class Searchinput : MonoBehaviour
{
    public GameObject prefab;
    public Transform spawnParent; // Перетащи сюда объект Canvas или другой объект, под которым ты хочешь спавнить кнопки
    public float yOffset = 10f; // Отступ по вертикали между кнопками  
    public float xOffset = 250f;

    public Animator animator;

    private string dbName = "URI=file:KGPIP.db";
    private IDataReader reader;
     private TMP_InputField inputField;

    void Start()
    {
        using (var connectionString = new SqliteConnection(dbName)){
            connectionString.Open();
            connectionString.Close();
        }

        inputField = GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEndEdit()
    {
        GameObject[] buttonsArray = GameObject.FindGameObjectsWithTag("searchButton");
        foreach (GameObject button in buttonsArray)
        {
            Destroy(button);
        }
        
        using (var connectionString = new SqliteConnection(dbName))
        {
            connectionString.Open();

            float currentYPosition = 700f; // Начальная позиция Y для первой кнопки

            using (var command = connectionString.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Locations WHERE RNAME LIKE '%" + inputField.text + "%';";

                using (reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string rName = reader["RNAME"].ToString();
                        SpawnPrefabWithText(rName, currentYPosition);

                        // Увеличиваем текущую позицию Y для следующей кнопки
                        currentYPosition -= yOffset;
                    }
                }
            }

            connectionString.Close();
        }
    }

    void SpawnPrefabWithText(string text, float yPos)
    {
        // Создаем новый объект prefab
        GameObject spawnedObject = Instantiate(prefab, new Vector3(xOffset, yPos, 0f), Quaternion.identity, spawnParent);

        // Находим вложенный объект с компонентом TextMeshPro
        TextMeshProUGUI textMeshPro = spawnedObject.GetComponentInChildren<TextMeshProUGUI>();

        // Устанавливаем текст
        if (textMeshPro != null)
        {
            textMeshPro.text = text;
        }
        else
        {
            Debug.LogError("TMP component not found in the prefab!");
        }
    }
}
