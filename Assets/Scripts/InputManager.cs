using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction verticalAction;
    private InputAction horizontalAction;
    private InputAction SprintAction;
    private InputAction StrikeAction;
    private InputAction PassAction;
    private InputAction ChipAction;
    private InputAction LoftedPassAction;
    public Vector2 Move;
    public bool Sprint;
    public bool Strike;
    public bool Pass;
    public bool LoftedPass;
    public bool ChangePlayer;
    [Range(0,1)]public float Power;
    public float powerVal;
    float powerUp;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        verticalAction = playerInput.actions["Vertical"];
        horizontalAction = playerInput.actions["Horizontal"];
        SprintAction = playerInput.actions["Sprint"];
        StrikeAction = playerInput.actions["Strike"];
        PassAction = playerInput.actions["Pass"];
        ChipAction = playerInput.actions["Chip"];
        LoftedPassAction = playerInput.actions["LoftedPass"];
    }
    private void Update()
    {
        Move.y = verticalAction.ReadValue<float>();
        Move.x = horizontalAction.ReadValue<float>();
        ChangePlayer = ChipAction.triggered ? true : false;
        PowerUp();
    }
    private void OnEnable()
    {
        SprintAction.performed += ctx => Sprint = true;
        SprintAction.canceled += ctx => Sprint = false;
        PassAction.performed += ctx => powerUp = 1;
        PassAction.canceled += ctx => 
        {
            powerUp = 0;
            powerVal = Power;
            Pass = true;
        };
        LoftedPassAction.performed += ctx => powerUp = 1;
        LoftedPassAction.canceled += ctx => 
        {
            powerUp = 0;
            powerVal = Power;
            LoftedPass = true;
        };
        StrikeAction.performed += ctx => powerUp = 1;
        StrikeAction.canceled += ctx => 
        {
            powerUp = 0;
            powerVal = Power;
            Strike = true;
        };
    }
    private void OnDisable()
    {
    }
    private void PowerUp()
    {
        Power = Mathf.Lerp(Power, powerUp, 5f * Time.deltaTime);
    }
}
