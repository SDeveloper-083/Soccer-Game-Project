using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector]public Animator animator;
    PlayerController playerController;
    CharacterController controller;
    public PlayerData playerData;
    public float DistanceToBall;
    Ball ball;
    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        ball = playerController.ball;
        controller.transform.LookAt(new Vector3(ball.transform.position.x, 0, ball.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        DistanceToBall = Vector3.Distance(transform.position, ball.transform.position);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        Ball ball = other.gameObject.GetComponent<Ball>();
        if(other.gameObject.CompareTag("Ball"))
            other.transform.SetParent(transform);
    }
    public void passAnimationEvent()
    {
        playerController.PassBallToPlayer(playerController.targetPlayer);
        playerController.playerManager = playerController.targetPlayer;
        playerController.input.Pass = false;
        playerController.input.powerVal = 0;
        playerController.teamState = TeamState.Attacking;
    }
}
