using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotspot : MonoBehaviour {
    [SerializeField] private GameObject nextSpot;

    NavManager navManager;
    SphereChanger sphereChanger;
    Animator anim;

    void Start() {
        sphereChanger = GameObject.Find("NavManager").GetComponent<SphereChanger>();
        navManager = GameObject.Find("NavManager").GetComponent<NavManager>();
        anim = GetComponent<Animator>();
    }

    public void MoveToNextSpot() {
        sphereChanger.ChangeSphere(nextSpot, navManager.GetPlayerCurSphere());
        navManager.SetPlayerCurSphere(nextSpot);
        //Debug.Log("Moved to " + nextSpot.name);
    }

    public GameObject GetNextSpot() {
        return nextSpot;
    }

    public void SetAnimParam(int index, bool isTrue) { // set animator parameter, index 1: isNextWP, 2: isDestPt
        switch (index) {
            case 1:
                anim.SetBool("isNextWP", isTrue);
                break;
            case 2:
                anim.SetBool("isDestPt", isTrue);
                break;
            default:
                Debug.Log("Wrong input of index");
                break;
        }
    }
}