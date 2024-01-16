using System.Linq;
using BCIEssentials.LSLFramework;
using BCIEssentials.Utilities;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimpleMarkerSubscriber : MonoBehaviour, ILSLMarkerSubscriber
{
    [Header("Configuration")]
    [SerializeField]
    private LSLServiceProvider _provider;

    [SerializeField] private string _streamName = "PythonResponse";
    [SerializeField] private bool _subscribeOnStart = false;

    [Header("UI")] [SerializeField] private Button _subscribeButton;
    [SerializeField] private Button _unsubscribeButton;
    [SerializeField] private TMP_Text _statusText;
    [SerializeField] private TMP_Text _responseText;

    [Space(20), InspectorReadOnly] public LSLMarkerReceiver LslMarkerReceiver;

    public bool Subscribed { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        _statusText.text = "No Connection";
        _responseText.text = "No Responses";
        _subscribeButton.onClick.AddListener(Subscribe);
        _unsubscribeButton.onClick.AddListener(Unsubscribe);

        if (_provider == null)
        {
            return;
        }

        LslMarkerReceiver = _provider.GetMarkerReceiverByName(_streamName) as LSLMarkerReceiver;
        if (LslMarkerReceiver != null && _subscribeOnStart)
        {
            Subscribe();
        }
    }

    public void Subscribe()
    {
        if (!Subscribed && LslMarkerReceiver != null)
        {
            Debug.Log("Subscribed!");
            _statusText.text = "Subscribed!";
            LslMarkerReceiver.Subscribe(this);
            Subscribed = true;
        }
    }

    public void Unsubscribe()
    {
        if (Subscribed && LslMarkerReceiver != null)
        {
            Debug.Log("Unsubscribed!");
            _statusText.text = "Unsubscribed!";
            LslMarkerReceiver.Unsubscribe(this);
            Subscribed = false;
        }
    }

    public void NewMarkersCallback(LSLMarkerResponse[] latestMarkers)
    {
        Debug.Log($"{latestMarkers.Length} New Markers Received");

        var responseStrings = latestMarkers.Select(r => r.Value[0]).ToArray();
        _responseText.text = $"Responses:\n{string.Join(", ", responseStrings)}";
    }
}
