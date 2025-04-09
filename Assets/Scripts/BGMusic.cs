using UnityEngine;

public class BGMusic : MonoBehaviour {
    private AudioSource audioSource;
    private float originalVolume;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        originalVolume = audioSource.volume; // Store the original volume
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha8)) {
            ToggleMusicVolume();
        }
    }

    private void ToggleMusicVolume() {
        if (audioSource.volume > 0f)
            audioSource.volume = 0f; // Set the volume to 0
        else
            audioSource.volume = originalVolume; // Set the volume back to the original volume
    }
}