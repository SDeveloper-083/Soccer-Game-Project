using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TeamState
{
    Attacking,
    Deffending,
    AttackKickOff,
    DeffendKickOff
};
public class PlayerController : MonoBehaviour
{
    public Pitch pitch;
    public PlayerManager playerManager;
    public PlayerManager targetPlayer;
    public TeamManager teamManager;
    public TeamState teamState;
    CharacterController controller;
    public Ball ball;
    public Goal goalTarget;
    [SerializeField] Transform ballTarget;
    [HideInInspector]public InputManager input;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public bool isDribble;
    bool haveBall;
    public float Distance;
    [SerializeField]private float playerSpeed = 5.0f;
    [SerializeField]private float sprintSpeed = 6.0f;
    [SerializeField]private float gravityValue = -9.81f;
    private Vector3 move;
    private float targetRotation = 0.0f;
    private float rotationVelocity;
    private float RotationSmoothTime = 0.12f;

    private void Start()
    {
        input = GetComponent<InputManager>();
        pitch = FindObjectOfType<Pitch>();
        ball = pitch.ball;
        teamState = TeamState.AttackKickOff;
        playerManager = teamManager.playerSort[temp];
    }

    void Update()
    {
        controller = playerManager.GetComponent<CharacterController>();
        transform.position = controller.transform.position;
        Distance = Vector3.Distance(transform.position, ball.transform.position);
        switch(teamState)
        {
            default:
            case TeamState.Attacking:
            {
                teamState = (Distance > 10f) ? TeamState.Deffending : TeamState.Attacking;
                Move();
                Dribble();
                Pass();
                LoftedPass();
                Strike();
                break;
            }
            case TeamState.Deffending:
            {
                teamState = (Distance > 10f) ? TeamState.Deffending : TeamState.Attacking;
                Move();
                ChangePlayer();
                break;
            }
            case TeamState.AttackKickOff:
            {
                KickOff();
                break;
            }
            case TeamState.DeffendKickOff:
            {
                break;
            }
            
        }
    }
    private void Move()
    {
        float targetSpeed = input.Sprint ? sprintSpeed : playerSpeed;
        if(input.Move == Vector2.zero) targetSpeed = 0.0f;

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 target = Vector3.Normalize(ball.transform.position - transform.position);
        move = isDribble ? target : new Vector3(input.Move.x,0,input.Move.y);
        controller.Move(move * Time.deltaTime * targetSpeed);
        
		Vector3 inputDirection = isDribble ? target : new Vector3(input.Move.x, 0.0f, input.Move.y).normalized;

        if (move != Vector3.zero)
        {
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, RotationSmoothTime);
            
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            controller.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        playerManager.animator.SetFloat("Speed", controller.velocity.magnitude);

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    private void Dribble()
    {
        isDribble = Distance <= 4f? true:false;
        Vector3 direction = new Vector3 (input.Move.x, 0f, input.Move.y);

        if(isDribble && Distance <= 1.25f)
        {
            if(direction == Vector3.zero)
                ball.GetComponent<Rigidbody>().AddForce(playerManager.transform.forward * 2);
            else
                ball.GetComponent<Rigidbody>().AddForce(direction * 4f);
        }
    }
    private void KickOff()
    {
        Vector3 Direction = new Vector3(input.Move.x, 0f, input.Move.y);
        
        targetPlayer = (Direction == Vector3.zero)? teamManager.playerSort[1]:FindPlayerInDirection(Direction);
        
        if(input.Pass)
        {
            playerManager.animator.Play("pass");
        }
        
    }
    private void Pass()
    {
        Vector3 Direction = new Vector3(input.Move.x, 0f, input.Move.y);
        
        targetPlayer = (Direction == Vector3.zero)? teamManager.playerSort[1]:FindPlayerInDirection(Direction);
        

        if(targetPlayer != null && isDribble && Distance <= 1.25f)
        {
            if(input.Pass)
            {
                playerManager.animator.Play("pass", 0);
            }
        }
    }
    private void LoftedPass()
    {
        Vector3 Direction = new Vector3(input.Move.x, 0f, input.Move.y);
        
        targetPlayer = (Direction == Vector3.zero)? teamManager.playerSort[1]:FindPlayerInDirection(Direction);
        

        if(targetPlayer != null && isDribble && Distance <= 1.25f)
        {
            if(input.LoftedPass)
            {
                HighPassBallToPlayer(targetPlayer);
                playerManager = targetPlayer;
                input.LoftedPass = false;
                input.powerVal = 0;
            }
        }
    }
    private void Strike()
    {
        if(input.Strike && isDribble && Distance <= 1.25f)
        {
            ball.transform.SetParent(null);
            Vector3 target = Vector3.Normalize(goalTarget.transform.position - ball.transform.position);
            Vector3 targetGoal = new Vector3(target.x, target.y * input.powerVal * 10f, target.z * input.Move.y);
            ball.GetComponent<Rigidbody>().AddForce(targetGoal * 30f * input.powerVal, ForceMode.Impulse);
            input.Strike = false;
            input.powerVal = 0;
        }
    }
    private Vector3 DirectionTo(PlayerManager player)
    {
        return Vector3.Normalize(player.transform.position - ball.transform.position);
    }
    private Vector3 DirectionGoal(Goal goal)
    {
        return Vector3.Normalize(goal.transform.position - ball.transform.position);
    }

    private PlayerManager FindPlayerInDirection(Vector3 direction)
    {
        PlayerManager selectedPlayer = null;
        float angle = Mathf.Infinity;
        foreach (var player in teamManager.playerList)
        {
            var directionToPlayer = DirectionTo(player);
            var playerAngle = Vector3.Angle(direction, directionToPlayer);
            if(playerAngle < angle)
            {
                selectedPlayer = player;
                angle = playerAngle;
            }
        }
        return selectedPlayer;
    }
    public void PassBallToPlayer(PlayerManager targetPlayer)
    {
        var direction = DirectionTo(targetPlayer);
        var Distance = Vector3.Distance(playerManager.transform.position, targetPlayer.transform.position);
        var powerRange = Mathf.Clamp(Distance, 5f, 20f);
        ball.transform.SetParent(null);
        ball.GetComponent<Rigidbody>().isKinematic = false;
        ball.GetComponent<Rigidbody>().AddForce(direction * powerRange * input.powerVal, ForceMode.Impulse);
    }
    public void HighPassBallToPlayer(PlayerManager targetPlayer)
    {
        var direction = DirectionTo(targetPlayer);
        var Distance = Vector3.Distance(playerManager.transform.position, targetPlayer.transform.position);
        var powerRange = Mathf.Clamp(Distance, 20f, 50f);
        ball.transform.SetParent(null);
        ball.GetComponent<Rigidbody>().isKinematic = false;
        ball.GetComponent<Rigidbody>().AddForce(new Vector3(direction.x/2.1f, 0.5f, direction.z/2.1f) * powerRange * input.powerVal, ForceMode.Impulse);
    }
    public void StrikeToGoal(Goal goal)
    {
        var direction = DirectionGoal(goal);
        var Distance = Vector3.Distance(playerManager.transform.position, goal.transform.position);
        var powerRange = Mathf.Clamp(Distance, 20f, 50f);
        ball.transform.SetParent(null);
        ball.GetComponent<Rigidbody>().isKinematic = false;
        ball.GetComponent<Rigidbody>().AddForce(direction * 2f * input.powerVal);
    }

    Vector3 SampleParabola(Vector3 start, Vector3 end, float height, float t)
    {
        float parabolicT = t * 2 - 1;
        Vector3 travelDirection = end - start;
        Vector3 result = start + t * travelDirection;
        result.y += (-parabolicT * parabolicT + 1) * height;

        return result;
    }
    
    [SerializeField]private int temp = 0;
    private void ChangePlayer()
    {
        targetPlayer = teamManager.playerSort[temp + 1];
        playerManager = teamManager.playerSort[0];
        if(input.ChangePlayer)
        {
            temp = temp > 0 ? 0 : 1;
            playerManager = targetPlayer;
        }    
    }

}
