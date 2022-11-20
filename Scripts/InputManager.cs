using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.InMatchActions inMatch;
    private Player player;
    private BallPhysics ball;

    private void Awake()
    {
        playerInput = new PlayerInput();
        inMatch = playerInput.InMatch;
        player = GetComponent<Player>();
        ball = GameObject.Find("Ball").GetComponent<BallPhysics>();
        inMatch.ResetBall.performed += ctx => ball.ResetBall();
    }

    private void Update()
    {
        player.Move(inMatch.Movement.ReadValue<float>());
    }

    private void OnEnable()
    {
        inMatch.Enable();
    }
    private void OnDisable()
    {
        inMatch.Disable();
    }
}
