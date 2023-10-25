using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public MqttHandler _mqttHandler;

    public GameObject cube;

    public float east = 2;
    public float west = -2;
    public float north = 2;
    public float south = -2;

    private Color cubeCol = Color.blue;

    // Start is called before the first frame update
    void Start()
    {
        _mqttHandler.StartupMqtt();
        _mqttHandler.SubscribeToBroker(HandleMqttData);
    }

    // Update is called once per frame
    void Update()
    {
        var renderer = cube.GetComponent<Renderer>();
        //renderer.material.SetColor("_Color", cubeCol);
        renderer.material.color = cubeCol;
    }

    private void HandleMqttData(ModData data)
    {
        Debug.Log("data received - foobar");

        //cubeCol.r = 1;
        //cubeCol.g = 0;
        //cubeCol.b = 0;

        System.Random rand = new System.Random();
        
        cubeCol.r = (float) rand.NextDouble();
        cubeCol.g = (float) rand.NextDouble();
        cubeCol.b = (float) rand.NextDouble();

        Debug.Log("red part: " + cubeCol.r);

        // Instantiate(objectToSpawn, new Vector3(Random.Range(west, east), 0, Random.Range(north, south)), Quaternion.identity);
        // Instantiate(objectToSpawn, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);

    }
}
