using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.InMatchActions inMatch;
    private Player player;

    private void Awake()
    {
        playerInput = new PlayerInput();
        inMatch = playerInput.InMatch;
        player = GetComponent<Player>();
        inMatch.ResetBall.performed += ctx => player.ResetBall();
        inMatch.Swing.performed += ctx => StartCoroutine(player.Swing(inMatch.Swing.ReadValue<float>(), inMatch.Spin.ReadValue<float>()));
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
