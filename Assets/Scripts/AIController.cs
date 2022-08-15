using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public PlayerManager player;
    public TeamManager team;
    public Pitch pitch;
    public Ball ball;
    float Distance;
    CharacterController controller;
    private float targetRotation = 0.0f;
    private float rotationVelocity;
    private float RotationSmoothTime = 0.12f;
    // Start is called before the first frame update
    void Start()
    {
        pitch = FindObjectOfType<Pitch>();
        ball = pitch.ball;
    }

    // Update is called once per frame
    void Update()
    {
        player = team.playerSort[0];
        controller = player.GetComponent<CharacterController>();
        transform.position = controller.transform.position;
        Distance = Vector3.Distance(ball.transform.position, transform.position);
        Vector3 target = (Distance >= 1f) ? Vector3.Normalize(ball.transform.position - transform.position) : Vector3.zero;
        controller.Move(target * 4f * Time.deltaTime);
        
        Vector3 inputDirection = target;
        if (target != Vector3.zero)
        {
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, RotationSmoothTime);
            
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            controller.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
    }
}
