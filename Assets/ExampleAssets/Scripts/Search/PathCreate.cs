using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mono.Data.Sqlite;
using System.IO;
using System.Data;

public class PathCreate : MonoBehaviour
{
// Путь к файлу базы данных SQLite
    private string connectionString = "URI=file:KGPIP.db";
    private IDataReader reader;

    public TMP_InputField start_field;
    public TMP_InputField end_field;

    public class Graph
    {
        private int V = 31; // Количество вершин
        private List<int>[] adj; // Список смежности
        public Dictionary<string, int> vertexIndices; // Словарь для хранения индексов вершин по названию
        public Dictionary<Tuple<int, int>, GameObject> edgeObjects; // Словарь для хранения объектов Unity, привязанных к ребрам

        // Конструктор
        public Graph(int v)
        {
            V = v;
            adj = new List<int>[V];
            for (int i = 0; i < V; ++i)
                adj[i] = new List<int>();
            vertexIndices = new Dictionary<string, int>();
            edgeObjects = new Dictionary<Tuple<int, int>, GameObject>();
        }

        // Добавление вершины в граф
        public void AddVertex(int v, string name)
        {
            if (!vertexIndices.ContainsKey(name))
            {
                vertexIndices[name] = v;
                //Debug.Log(name.ToString() + ".." + vertexIndices[name].ToString());
            }
            else
            {
                Debug.LogWarning("Vertex with name " + name + " already exists.");
                return;
            }

            if (v >= V)
            {
                int oldV = V;
                V = v + 1;
                Array.Resize(ref adj, V);
                for (int i = oldV; i < V; ++i)
                    adj[i] = new List<int>();
            }
        }

        // Добавление ребра в граф с указанием объекта Unity, привязанного к ребру
        public void AddEdge(int v, int w, GameObject uObject)
        {
            adj[v].Add(w);
            adj[w].Add(v);
            edgeObjects[Tuple.Create(v, w)] = uObject;
        }

        // Получение объекта Unity, привязанного к ребру
        public GameObject GetEdgeObject(int v, int w)
        {
            if (edgeObjects.ContainsKey(Tuple.Create(w, v)))
            {
                return edgeObjects[Tuple.Create(v, w)];
            }
            else
            {
                Debug.LogWarning("Edge object for the edge (" + v + ", " + w + ") does not exist.");
                return null;
            }
        }

        public int GetVertexIndex(string name)
        {
            if (vertexIndices.ContainsKey(name))
                return vertexIndices[name];
            else
            {
                Debug.LogWarning("Vertex with name " + name + " does not exist.");
                return -1;
            }
        }


        // Поиск кратчайшего пути между двумя вершинами
        public List<int> ShortestPath(int start, int end)
        {
            int[] previous = new int[V]; // Массив для хранения предыдущих вершин на кратчайшем пути
            bool[] visited = new bool[V]; // Массив для отслеживания посещенных вершин
            Queue<int> queue = new Queue<int>(); // Очередь для BFS

            for (int i = 0; i < V; i++)
            {
                previous[i] = -1; // Инициализация массива предыдущих вершин
            }

            queue.Enqueue(start); // Начинаем с поиска пути от стартовой вершины
            visited[start] = true;

            while (queue.Count > 0)
            {
                int current = queue.Dequeue();

                if (current == end) // Проверяем, достигли ли мы конечной вершины
                {
                    break;
                }

                foreach (int next in adj[current]) // Ищем смежные вершины текущей вершины
                {
                    if (!visited[next])
                    { 
                        visited[next] = true; // Помечаем вершину как посещенную
                        previous[next] = current; // Сохраняем информацию о предыдущей вершине
                        queue.Enqueue(next);
                    }
                }
            }

            List<int> shortestPath = new List<int>(); // Восстанавливаем кратчайший путь из массива previous
            int currentVertex = end;
            while (currentVertex != -1 && currentVertex != start)
            {
                shortestPath.Add(currentVertex);

                // Включение и отображение объекта Unity, привязанного к ребру
                int previousVertex = previous[currentVertex];
                if (previousVertex != -1)
                {
                    GameObject edgeObject = GetEdgeObject(previousVertex, currentVertex);
                    Debug.Log("Проверка линка  " + previousVertex + "/" + currentVertex);
                    if (edgeObject != null)
                    {
                        edgeObject.SetActive(true);
                    }
                }

                currentVertex = previousVertex;
            }
            shortestPath.Add(start); // Добавляем стартовую вершину в кратчайший путь
            shortestPath.Reverse(); // Переворачиваем путь, чтобы он начинался с начальной вершины и заканчивался конечной

            return shortestPath;
        }
    }

    // Метод для получения данных из базы данных SQLite и создания графа
    void GenerateGraphFromDatabase()
    {
        graph = new Graph(0); // Создаем пустой граф

        // Устанавливаем соединение с базой данных SQLite
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            // Выполняем запрос к базе данных для получения данных из таблицы Locations
            string query = "SELECT * FROM Locations";
            using (var command = connection.CreateCommand())
            {
                command.CommandText = query;
                using (reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Добавляем вершину в граф
                        graph.AddVertex(Convert.ToInt32(reader["ID"]), reader["RNAME"].ToString()); // Предполагается, что первый столбец - индекс вершины, второй - её имя
                    }
                    
                }
            }

            // Выполняем запрос к базе данных для получения данных из таблицы Links

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM Links";
                //Debug.Log("Количество записей в таблице Links = " + command.ExecuteScalar().ToString());
            }

            query = "SELECT * FROM Links";
            using (var command = connection.CreateCommand())
            {
                command.CommandText = query;
                using (IDataReader reader2 = command.ExecuteReader())
                {
                    //Debug.Log("Началось добавление связей в граф");
                    while (reader2.Read())
                    {
                        // Добавляем ребро в граф для каждой записи из таблицы Links
                        //Debug.Log(reader2["LOCK1"].ToString() + " | " + reader2["LOCK2"].ToString() + " | " + reader2["UOBJ"].ToString());
                        graph.AddEdge(Convert.ToInt32(reader2["LOCK1"]), Convert.ToInt32(reader2["LOCK2"]), GameObject.Find(reader2["UOBJ"].ToString())); // Предполагается, что первый столбец - имя первой вершины, второй - имя второй вершины, третий - имя объекта Unity
                    }
                }
            }

            connection.Close();
        }


    }

    Graph graph;

    // Метод для поиска объекта Unity по его имени на сцене
    GameObject GetObjectByName(string name)
    {
        GameObject obj = GameObject.Find(name);
        if (obj == null)
        {
            Debug.LogWarning("Object with name " + name + " not found on the scene.");
        }
        return obj;
    }

    // Вызываем метод для генерации графа из базы данных SQLite при старте сцены
    void Start()
    {
        GenerateGraphFromDatabase();
    }

    public void CreateShortPath()
    {
        List<int> shortestPath = graph.ShortestPath(graph.GetVertexIndex(start_field.text), graph.GetVertexIndex(end_field.text));
        foreach (int vertex in shortestPath)
        {
            Debug.Log(vertex);
        }
    }
}
