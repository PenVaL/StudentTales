using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour {
    public static int FresherTaskIndex = 0; // -1 = completed, 0 = task 1
    public static bool EnterfromFresherTaskEntry = false;
    public static bool isAwardedFresherTask = false;

    public void EnterFromFresherTaskEntry(bool val) {
        EnterfromFresherTaskEntry = val;
    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
