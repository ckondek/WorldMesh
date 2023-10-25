using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public delegate void MqttDataReceivedCallback(ModData data);

public class MqttHandler : MonoBehaviour
{
    [SerializeField]
    //private string _mqttHost = "127.0.0.1";

    private string _mqttHost = "192.168.0.183";

    [SerializeField]
    private static string _baseTopic = "world/mesh/1";

    private readonly string _subTopic = _baseTopic + "/in";

    private readonly string _sysTopic = _baseTopic + "/sys";

    private readonly string _errTopic = _baseTopic + "/error";

    private IMqttClient _mqttClient;

    private static readonly CancellationToken cancellationToken = CancellationToken.None;

    private MqttDataReceivedCallback _mqttDataReceivedCallback;

    void Start()
    {
    }

    private void OnApplicationQuit()
    {
        Debug.Log("quit mqtt handler");
        if (_mqttClient.IsConnected)
        {
            Debug.Log("disconnect mqtt broker");
            _mqttClient.DisconnectAsync();
            _mqttClient.Dispose();
        }
    }

    public void StartupMqtt()
    {
        MqttFactory factory = new();
        _mqttClient = factory.CreateMqttClient();
        ConnectMqttClient(_mqttClient, Utils.RandomString(10));
    }

    private async void ConnectMqttClient(IMqttClient client, string clientId)
    {
        var options = new MqttClientOptionsBuilder()
            .WithClientId(clientId)
            .WithTcpServer(_mqttHost)
            .WithCleanSession()
            .Build();

        client.UseDisconnectedHandler(async e =>
        {
            await Task.Delay(TimeSpan.FromSeconds(2));

            try
            {
                await client.ConnectAsync(options, cancellationToken);
                PublishToBroker(_baseTopic + "/welcome", "hello world!");
            }
            catch
            {
                Debug.Log("reconnect to mqtt broker failed");
            }
        });

        client.UseApplicationMessageReceivedHandler(e => MqttMessageReceivedCallback(e));

        await client.ConnectAsync(options, cancellationToken);
    }

    private void MqttMessageReceivedCallback(MqttApplicationMessageReceivedEventArgs e)
    {
        if (e.ApplicationMessage.Topic == _subTopic && _mqttDataReceivedCallback != null)
        {
            string json = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            ModData data = JsonUtility.FromJson<ModData>(json);
            _mqttDataReceivedCallback(data);
        }
    }

    public void SubscribeToBroker(MqttDataReceivedCallback callback)
    {
        Debug.Log("try to subscribe to '" + _subTopic + "'");
        _mqttDataReceivedCallback = callback;
        _mqttClient.UseConnectedHandler(async e =>
        {
            await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(_subTopic).Build());
            Debug.Log("subscribed to topic '" + _subTopic + "'");
        });
    }

    public async void PublishToBroker(string topic, string payload)
    {
        Debug.Log("publish to broker");
        var message = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .WithExactlyOnceQoS()
            .Build();

        await _mqttClient.PublishAsync(message, cancellationToken);
    }
}
