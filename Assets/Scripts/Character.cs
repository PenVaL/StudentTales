using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour {
    [SerializeField] Image fresherTaskAwardButton;
    [SerializeField] GameObject fresherTaskAward;
    [SerializeField] Color unlocked, activated;
    [SerializeField] GameObject AwardSparksVFX;
    bool isFresherTaskAwardActivated;

    Animator anim;
    AudioSource audio;

    void Start() {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();

        // check if player have done fresher task
        if (PlayerData.FresherTaskIndex == -1) {
            fresherTaskAwardButton.color = unlocked;
        }
    }

    public void CheckFresherTaskAwarded() {
        // check if player is not being awarded, but have done fresher task
        if (!PlayerData.isAwardedFresherTask && PlayerData.FresherTaskIndex == -1) {
            StartCoroutine(AwardFresherTaskEvent());
            PlayerData.isAwardedFresherTask = true; // set to true
        }
    }

    public void ToggleFesherTaskAward() {
        // check if player have done fresher task
        if (PlayerData.FresherTaskIndex != -1)
            return;

        isFresherTaskAwardActivated = !isFresherTaskAwardActivated;

        if (isFresherTaskAwardActivated) {
            fresherTaskAwardButton.color = activated;
            fresherTaskAward.SetActive(true);
            AwardSparksVFX.SetActive(true);
            TriggerShakeHandAnim();
        } else {
            fresherTaskAwardButton.color = unlocked;
            fresherTaskAward.SetActive(false);
            AwardSparksVFX.SetActive(false);
        }
    }

    public void TriggerShakeHandAnim() {
        anim.SetTrigger("ShakeHand");
        audio.Play();
    }

    IEnumerator AwardFresherTaskEvent()  {
        Animator awardAnimator = fresherTaskAwardButton.gameObject.GetComponent<Animator>();
        awardAnimator.enabled = true; // Enable the Animator component to play the animation
        yield return new WaitForSeconds(1f);
        awardAnimator.enabled = false; // Disable to prevent triggering everytime
    }
}
