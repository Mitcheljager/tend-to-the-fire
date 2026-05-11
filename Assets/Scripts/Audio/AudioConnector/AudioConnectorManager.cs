using System.Collections.Generic;
using UnityEngine;

public class AudioConnectorManager : MonoBehaviour {
    public List<AudioConnector> audioConnectors = new();

    private PlayerState playerState;

    void Start() {
        playerState = FindAnyObjectByType<PlayerState>();

        SetAudioConnectors();
    }

    void OnDrawGizmos() {
        SetAudioConnectors();
    }

    private void SetAudioConnectors() {
        audioConnectors.Clear();

        AudioConnector[] items = FindObjectsByType<AudioConnector>(FindObjectsSortMode.None);

        foreach (AudioConnector item in items) {
            audioConnectors.Add(item.GetComponent<AudioConnector>());
        }
    }

    public List<AudioConnector> GetReachableAudioConnectors(Vector3 startPosition) {
        LayerMask layerMask = LayerMask.GetMask("Default");

        List<AudioConnector> reachableAudioConnectors = new();

        foreach (AudioConnector audioConnector in audioConnectors) {
            if (audioConnector.transform.position == startPosition) continue;
            if (Physics.Linecast(startPosition, audioConnector.transform.position, layerMask)) continue;

            reachableAudioConnectors.Add(audioConnector);
        }

        return reachableAudioConnectors;
    }

    public (AudioConnector, float) FindBestAudioConnector(Vector3 startPosition) {
        LayerMask layerMask = LayerMask.GetMask("Default");

        // Check if we can see the player directly from the start position
        if (!Physics.Linecast(startPosition, playerState.transform.position, layerMask)) return (null, Vector3.Distance(startPosition, playerState.transform.position));

        List<AudioConnector> reachableAudioConnectors = GetReachableAudioConnectors(startPosition);
        Dictionary<AudioConnector, float> distances = new();
        Queue<AudioConnector> queue = new();
        HashSet<AudioConnector> visited = new();
        List<AudioConnector> audioConnectorsInViewOfPlayer = new();

        foreach (AudioConnector connector in reachableAudioConnectors) {
            queue.Enqueue(connector);
            visited.Add(connector);
            distances[connector] = Vector3.Distance(startPosition, connector.transform.position);
        }

        while (queue.Count > 0) {
            AudioConnector current = queue.Dequeue();
            float currentDistance = distances[current];

            // If we can see the player from the current connector, add it to the list
            if (!Physics.Linecast(current.transform.position, playerState.transform.position, layerMask)) {
                audioConnectorsInViewOfPlayer.Add(current);
            }

            foreach (AudioConnector reachable in current.reachableAudioConnectors) {
                if (visited.Contains(reachable)) continue;

                visited.Add(reachable);
                queue.Enqueue(reachable);
                distances[reachable] = currentDistance + Vector3.Distance(current.transform.position, reachable.transform.position);
            }
        }

        // Find the audio connector in view of the player with the shortest total distance
        AudioConnector bestConnector = null;
        float shortestTotalDistance = float.MaxValue;

        foreach (AudioConnector connector in audioConnectorsInViewOfPlayer) {
            float totalDistance = distances[connector] + Vector3.Distance(connector.transform.position, playerState.transform.position);
            if (totalDistance >= shortestTotalDistance) continue;

            shortestTotalDistance = totalDistance;
            bestConnector = connector;
        }

        return (bestConnector, shortestTotalDistance);
    }
}
