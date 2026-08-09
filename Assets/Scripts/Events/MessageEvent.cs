using UnityEngine;
using UnityEngine.Events;

public class MessageEvent : MonoBehaviour {
    public static UnityEvent<string> OnShowSmallMessage = new();

    public static void ShowSmallMessage(string message) {
        OnShowSmallMessage.Invoke(message);
    }
}
