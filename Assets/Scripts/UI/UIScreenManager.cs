using System.Collections.Generic;
using UnityEngine;

public class UIScreenManager : MonoBehaviour {
    public UIScreen currentUIScreen = null;
    public List<UIScreen> UIScreens = new();

    public void ShowScreen(UIScreen newUIScreen) {
        foreach(UIScreen UIScreen in UIScreens) {
            if (UIScreen == newUIScreen) UIScreen.Show();
            else UIScreen.Hide();
        }

        currentUIScreen = newUIScreen;
    }
}
