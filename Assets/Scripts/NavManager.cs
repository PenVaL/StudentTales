using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavManager : MonoBehaviour {
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerCurSphere; // use as Current waypoint (currentNode)
    WPManager wpManager;
    GameManager gm;
    GameObject[] spheres; // also use as Array of waypoints

    [SerializeField] int currentWP = 3; // Starting waypoint index
    // Access to the Graph script
    Graph g;

    // Navigation Activation
    [SerializeField] private Image navImage;
    [SerializeField] private Sprite navSprite, navActivatedSprite;
    bool isNavActivated = false;

    void Start() {
        wpManager = GetComponent<WPManager>();
        spheres = wpManager.waypoints; // Get hold of waypoints
        g = wpManager.GetComponent<WPManager>().graph;

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        player.transform.position = playerCurSphere.transform.position; // Set Player Position
        setSpheresActive();
    }

    void setSpheresActive() {
        for (int i = 0; i < spheres.Length; i++) {
            if (GameObject.ReferenceEquals(playerCurSphere, spheres[i]))
                spheres[i].SetActive(true);
            else
                spheres[i].SetActive(false);
        }
    }

    public GameObject GetPlayerCurSphere() {
        return playerCurSphere;
    }

    public void SetPlayerCurSphere(GameObject nextSphere) { // called in SphereChanger when player move to next spot        
       if (isNavActivated)
            setHotspotsInSphere(playerCurSphere, false); // reset the current Sphere hotspot
        
        nextSphere.SetActive(true);

        playerCurSphere = nextSphere; 
        //Debug.Log("PlayerCurSphere: " + playerCurSphere.name);
        gm.CheckTaskCompletion(); // check if the player complete the task at this spot
        
        if (isNavActivated) {
            if (gm.GetCurTaskDestPt() != null) { // if FresherTask is not completed
                calculateAStarPath(gm.GetCurTaskDestPt()); // re-calculateAStarPath
                setHotspotsInSphere(playerCurSphere, true); // update the current Sphere hotspot for navigation
            }
        }
    }

    void setHotspotsInSphere(GameObject sphere, bool updateNav) {
        Hotspot[] hotspots = sphere.GetComponentsInChildren<Hotspot>(); // get the hotspots in the curSphere
        //Debug.Log($"#Hotspots found in {sphere.name}: {hotspots.Length}");

        if (updateNav) { // updateNav: true = update the nav
            foreach (Hotspot h in hotspots) {
                GameObject nextWPSphere = h.GetNextSpot();

                if (CheckIsNextWP(nextWPSphere) == -1)
                    return; // if pathList is 0, return

                if (CheckIsNextWP(nextWPSphere) == 1 && gm.GetCurTaskDestPt() != null) {
                    Debug.Log($"Next path point Sphere: {nextWPSphere.name}");

                    h.SetAnimParam(1, true); // set isNextWP in hotspot animator to true
                    
                    if (GameObject.ReferenceEquals(nextWPSphere, gm.GetCurTaskDestPt())) // if next sphere is the destPt
                        h.SetAnimParam(2, true); // set isDestPt in hotspot animator to true

                    Debug.Log($"Updated hotspot at sphere: {sphere.name}");
                    Debug.Log($"Updated hotspot: {h.name}");
                }
            }

        } else { // updateNav false = reset the nav
            foreach (Hotspot h in hotspots) {
                h.SetAnimParam(1, false); // set isNextWP in hotspot animator to false
                h.SetAnimParam(2, false); // set isDestPt in hotspot animator to false
            }
        }
    }

    public bool CheckIsAtDestPt(GameObject destPt) {
        return GameObject.ReferenceEquals(playerCurSphere, destPt);
    }

    int CheckIsNextWP(GameObject nextSpot) {        
        if (g.getPathLength() == 1) // if pathList is 0, return -1
            return -1;

        if (GameObject.ReferenceEquals(g.getPathPoint(currentWP + 1), nextSpot))
            return 1; // == true, nextSpot is next WP

        return 0; // == false, nextSpot is not next WP
    }

    public void ToggleNavigation() {
        if (gm.GetCurTaskDestPt() == null) { // safety checking
            // if task is completed, tell player no more tasks plz go to HomeScene
            Debug.Log("No more task to navigate for");
            isNavActivated = false;
            return;
        } else {
            isNavActivated = !isNavActivated; // Toggle the boolean value
        }
        Debug.Log("isNavActivated: " + isNavActivated);

        if (isNavActivated) {
            // UpdateUI
            navImage.sprite = navActivatedSprite; // navActivation button on

            calculateAStarPath(gm.GetCurTaskDestPt());
            setHotspotsInSphere(playerCurSphere, true); // update the current Sphere hotspot for navigation  
        } else {
            // UpdateUI
            navImage.sprite = navSprite; // navActivation button off

            setHotspotsInSphere(playerCurSphere, false); // Reset the current Sphere hotspot
        }
    }

    void calculateAStarPath(GameObject destPt) {
        g.AStar(playerCurSphere, destPt);
        g.printPath();
        Debug.Log($"path length to {destPt.name}: {g.getPathLength()}");
        // Reset index
        currentWP = 0;
    }
}
