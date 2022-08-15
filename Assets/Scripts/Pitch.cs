using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitch : MonoBehaviour
{
    public TeamManager[] teams;
    public Ball ball;
    public GameObject rightGoal;
    public GameObject leftGoal;
    // Start is called before the first frame update
    void Start()
    {
        ball = FindObjectOfType<Ball>();
        teams = FindObjectsOfType<TeamManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
