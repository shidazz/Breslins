using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.InMatchActions inMatch;
    private Player player;

    void Awake()
    {
        playerInput = new PlayerInput();
        inMatch = playerInput.InMatch;
        player = GetComponent<Player>();
        inMatch.ResetBall.performed += ctx => player.Serve();
        inMatch.Swing.performed += ctx => player.ProcessSwing(inMatch.Swing.ReadValue<float>(), inMatch.Spin.ReadValue<float>());
    }

    void Update()
    {
        player.Move(inMatch.Movement.ReadValue<Vector2>());
        player.Spin(inMatch.Spin.ReadValue<float>());
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
