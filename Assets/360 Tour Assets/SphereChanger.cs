using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereChanger : MonoBehaviour {
    [SerializeField] private GameObject player;
    private PlayerInputController playerInput;

    [SerializeField] private GameObject loadingSprite;

    [SerializeField] private float fadeDuration = 0.25f;

    GameObject m_Fader; //This object should be called 'Fader' and placed over the camera    
    bool changing = false; //This ensures that we don't mash to change spheres

    void Awake() {
        //Find the fader object
        m_Fader = GameObject.Find("Fader");
        playerInput = player.GetComponent<PlayerInputController>();
        //Check if we found something
        if (m_Fader == null)
            Debug.LogWarning("No Fader object found on camera.");
    }

    public void ChangeSphere(GameObject nextSphere, GameObject curSphere) {
        if (changing)
            return;
        //Start the fading process
        StartCoroutine(FadeCamera(nextSphere, curSphere));
    }

    IEnumerator FadeCamera(GameObject nextSphere, GameObject curSphere) {
        changing = true;
        playerInput.DisableInput(fadeDuration * 2); // Disable Player Input

        if (m_Fader != null) {
            StartCoroutine(Fade(fadeDuration, m_Fader.GetComponent<Renderer>().material, true));
            loadingSprite.SetActive(true);
            yield return new WaitForSeconds(fadeDuration);

            //nextSphere.SetActive(true);
            player.transform.position = nextSphere.transform.position;
            curSphere.SetActive(false);

            StartCoroutine(Fade(fadeDuration, m_Fader.GetComponent<Renderer>().material, false));
            yield return new WaitForSeconds(fadeDuration);

            loadingSprite.SetActive(false);
        } else {
            //nextSphere.SetActive(true);
            player.transform.position = nextSphere.transform.position;
            curSphere.SetActive(false);
        }

        changing = false;
    }

    IEnumerator Fade(float time, Material mat, bool fadeIn) {
        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;

        float elapsedTime = 0f;
        Color color = mat.color;

        while (elapsedTime < time) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / time;
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            mat.color = color;
            yield return null;
        }

        color.a = endAlpha;
        mat.color = color;
    }

}