using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mono.Data.Sqlite;
using System.IO;

public class Searchinput : MonoBehaviour
{
    //public GameObject prefab;  префаб, который будем спавнить
    //public Transform spawnParent;  родитель, куда будем спавнить

    private string dbName = "URI=file:KGPIP.db";
     private TMP_InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        using (var connectionString = new SqliteConnection(dbName)){
            Debug.Log(connectionString.ToString());
            connectionString.Open();
            connectionString.Close();
        }

        inputField = GetComponent<TMP_InputField>();

        inputField.onEndEdit.AddListener(OnEndEdit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEndEdit(string text)
    {
        using (var connectionString = new SqliteConnection(dbName)){
            Debug.Log(connectionString.ToString());
            connectionString.Open();

            using (var command = connectionString.CreateCommand()){
                        command.CommandText = "SELECT * FROM * WHERE *;";
            }

            connectionString.Close();
        }
    }
}
