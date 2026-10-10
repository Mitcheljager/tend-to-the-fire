using System.Collections.Generic;
using UnityEngine;

public class UIScreenManager : MonoBehaviour {
    public UIScreen currentUIScreen = null;
    public List<UIScreen> UIScreens = new();
    public UIScreen initialUIScreen;

    public void ShowScreen(UIScreen newUIScreen) {
        foreach(UIScreen UIScreen in UIScreens) {
            if (UIScreen == newUIScreen) UIScreen.Show();
            else UIScreen.Hide();
        }

        currentUIScreen = newUIScreen;
    }

    public bool IsOnInitialUIScreen() {
        return currentUIScreen == initialUIScreen;
    }

    public void Reset() {
        ShowScreen(initialUIScreen);
    }
}
