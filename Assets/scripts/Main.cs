using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public MqttHandler _mqttHandler;

    public GameObject objectToSpawn;

    public float east = 2;
    public float west = -2;
    public float north = 2;
    public float south = -2;

    // Start is called before the first frame update
    void Start()
    {
        _mqttHandler.StartupMqtt();
        _mqttHandler.SubscribeToBroker(HandleMqttData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandleMqttData(ModData data)
    {
        Debug.Log("data received");
        //Instantiate(objectToSpawn, new Vector3(Random.Range(west, east), 0, Random.Range(north, south)), Quaternion.identity);
        Instantiate(objectToSpawn, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
    }
}
