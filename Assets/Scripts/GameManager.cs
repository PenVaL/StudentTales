using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    [SerializeField] private GameObject player; // used to get PlayerInputController for disableInput
    [SerializeField] private int curTaskIndex;

    [SerializeField] private GameObject[] fresherGameDestPts; // destPt for the fresherTask
    [SerializeField] private Sprite[] fresherTaskSprites; // all sprites for the fresherTask
    [SerializeField] private Sprite taskSprite, completedTaskSprite; // UI sprite for the task in progress and task completed
    [SerializeField] private Image fresherTaskImage, taskStateImage; // UI holder of the task in progress and task completed

    [SerializeField] private float waitCompleteDuration = 3.0f;
    bool isFresherTasksCompleted = false;

    [SerializeField] private AudioSource successSFX;
    [SerializeField] private GameObject TaskCompletedVFX;

    PlayerInputController playerInput;
    NavManager navManager;
    
    void Start() {
        navManager = GameObject.Find("NavManager").GetComponent<NavManager>();
        playerInput = player.GetComponent<PlayerInputController>();


        if (PlayerData.FresherTaskIndex == -1 || !PlayerData.EnterfromFresherTaskEntry) {
            isFresherTasksCompleted = true;
            fresherTaskImage.gameObject.SetActive(false);
            return;
        } 

        curTaskIndex = PlayerData.FresherTaskIndex; // get from PlayerData
        fresherTaskImage.sprite = fresherTaskSprites[curTaskIndex]; // update UI
    }

    public GameObject GetCurTaskDestPt() {
        // if task completed, return null
        if (isFresherTasksCompleted) {
            return null;
        }
        return fresherGameDestPts[curTaskIndex];
    }

    public void CheckTaskCompletion() {
        if (isFresherTasksCompleted)
            return;

        if (navManager.CheckIsAtDestPt(fresherGameDestPts[curTaskIndex])) {
            Debug.Log("Player Complete Fresher Task: " + curTaskIndex);
            // Update current Task Index
            if (curTaskIndex + 1 < fresherGameDestPts.Length) {
                curTaskIndex++;
                PlayerData.FresherTaskIndex = curTaskIndex; // update PlayerData
            } else {
                isFresherTasksCompleted = true;
                PlayerData.FresherTaskIndex = -1; // update PlayerData
            }
            StartCoroutine(TaskCompletionEvents());
        }    
    }

    IEnumerator TaskCompletionEvents() {
        playerInput.DisableInput(waitCompleteDuration); // Disable Player Input
        taskStateImage.sprite = completedTaskSprite;
        successSFX.Play();
        TaskCompletedVFX.SetActive(false);
        TaskCompletedVFX.SetActive(true);
        yield return new WaitForSeconds(waitCompleteDuration);

        // Update UI
        if (!isFresherTasksCompleted) { 
            fresherTaskImage.sprite = fresherTaskSprites[curTaskIndex];
            taskStateImage.sprite = taskSprite;
        }
        else {
            fresherTaskImage.gameObject.SetActive(false);
        }
    }
}