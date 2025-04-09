using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeSceneManager : MonoBehaviour {
    [SerializeField] private GameObject[] goButtonObjects;
    [SerializeField] private Image[] taskStateImages; // UI holder of the task in progress/completed
    [SerializeField] private Sprite taskSprite, completedTaskSprite; // UI sprite for the task in progress and task completed
    [SerializeField] private Image taskCompletedImage;

    void Start() {
        for (int i = 0; i < goButtonObjects.Length; i++) {
            if (i == PlayerData.FresherTaskIndex)
                goButtonObjects[i].SetActive(true);
            else
                goButtonObjects[i].SetActive(false);
        }

        for (int i = 0; i < taskStateImages.Length; i++)  {
            taskStateImages[i].sprite = (PlayerData.FresherTaskIndex > i || PlayerData.FresherTaskIndex == -1) ? completedTaskSprite : taskSprite;
        }

        if (PlayerData.FresherTaskIndex == -1)
            taskCompletedImage.gameObject.SetActive(true);
    }
}
