using UnityEngine;

public class FollowPath : MonoBehaviour {

    // Tank target
    Transform goal;
    // Tank speed
    [SerializeField] float speed = 100.0f;
    // Final distance from target
    [SerializeField] float accuracy = 200.0f;
    // Tank rotation speed
    [SerializeField] float rotSpeed = 20.0f;
    // Access to the WPManager script
    public GameObject wpManager;
    // Array of waypoints
    GameObject[] wps;
    // Current waypoint
    GameObject currentNode;
    // Starting waypoint index
    [SerializeField] int currentWP = 3;
    // Access to the Graph script
    Graph g;

    // Use this for initialization
    void Start() {
        // Get hold of wpManager and Graph scripts
        wps = wpManager.GetComponent<WPManager>().waypoints;
        g = wpManager.GetComponent<WPManager>().graph;
        // Set the current Node
        currentNode = wps[0];
    }
    
    /*
    public void GoToHeli() {

        // Use the AStar method passing it currentNode and destination
        g.AStar(currentNode, wps[4]);
        // Reset index
        currentWP = 0;
    } */

    // Update is called once per frame
    void LateUpdate() {
        
        // If we've nowhere to go, set destination to next wps index
        if (g.getPathLength() == 0 || currentWP == g.getPathLength()) {
            // randomly move to a waypoint
            int randomDesination = Random.Range(0, wps.Length);
            g.AStar(currentNode, wps[randomDesination]);
            Debug.Log("AStar tank randomDesination = " + randomDesination);

            // Reset index
            currentWP = 0;
            //return; // If we've nowhere to go then just return
        }   

        //the node we are closest to at this moment
        currentNode = g.getPathPoint(currentWP);

        //if we are close enough to the current waypoint move to next
        if (Vector3.Distance(
            g.getPathPoint(currentWP).transform.position,
            transform.position) < accuracy) {
            currentWP++;
        }

        //if we are not at the end of the path
        if (currentWP < g.getPathLength()) {
            goal = g.getPathPoint(currentWP).transform;
            Vector3 lookAtGoal = new Vector3(goal.position.x,
                                            this.transform.position.y,
                                            goal.position.z);
            Vector3 direction = lookAtGoal - this.transform.position;

            // Rotate towards the heading
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                                    Quaternion.LookRotation(direction),
                                                    Time.deltaTime * rotSpeed);

            // Move the tank
            this.transform.Translate(0, 0, speed * Time.deltaTime);
        }

    }
}