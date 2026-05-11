using System.Collections.Generic;
using UnityEngine;

public class AudioConnector : MonoBehaviour {
    public List<AudioConnector> reachableAudioConnectors = new();

    private AudioConnectorManager audioConnectorManager;

    void OnDrawGizmos() {
        Gizmos.color = reachableAudioConnectors.Count > 0 ? Color.cyan : Color.red;
        Gizmos.DrawSphere(transform.position, 0.2f);

        SetReachableAudioConnectors();

        foreach (AudioConnector audioConnector in reachableAudioConnectors) {
            Gizmos.DrawLine(transform.position, audioConnector.transform.position);
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = reachableAudioConnectors.Count > 0 ? Color.blue : Color.red;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }

    void Start() {
        SetReachableAudioConnectors();
    }

    private void SetReachableAudioConnectors() {
        reachableAudioConnectors.Clear();

        if (audioConnectorManager == null) audioConnectorManager = GameObject.Find("Audio Connector Manager").GetComponent<AudioConnectorManager>();
        reachableAudioConnectors = audioConnectorManager.GetReachableAudioConnectors(transform.position);
    }
}
