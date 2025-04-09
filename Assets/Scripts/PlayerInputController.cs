using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour {
    //public bool EnableInput = true;
    private float disableInputTimer;

    Camera cam;

    void Start() {
        cam = Camera.main;
        //print(cam.name);
    }

    public void DisableInput(float duration) {
        disableInputTimer += duration;
    }

    void Update() {
        if (disableInputTimer >= 0) {
            disableInputTimer -= Time.deltaTime;
            //Debug.Log("disableInputTimer time left: " + disableInputTimer);
            return; // return directly to disable player input
        }

        if (Input.GetMouseButtonDown(0)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.tag == "Hotspot") {
                    Hotspot hotspot = hit.transform.GetComponent<Hotspot>();
                    hotspot.MoveToNextSpot();
                }
            }
        }

    }
}
