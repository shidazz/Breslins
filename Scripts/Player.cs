using System.Collections;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class Player : NetworkBehaviour
{
    public static string p1scoreLabel;
    public static string p2scoreLabel;
    public static bool inPlay = false;

    private Trajectory trajectory;
    private BallPhysics ball;
    private AnimationController anim;
    private string returningSide;
    private bool multiplayerEnabled;
    private float speed;

    void Awake()
    {
        anim = GetComponent<AnimationController>();
        trajectory = GameObject.FindGameObjectWithTag("Trajectory").GetComponent<Trajectory>();
        ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallPhysics>();
    }

    void Start()
    {
        returningSide = "far";
        speed = Values.playerSpeed;

        if (SceneManager.GetActiveScene().name == "Training" || SceneManager.GetActiveScene().name == "MainMenu")
            multiplayerEnabled = false;
        else if (SceneManager.GetActiveScene().name == "Match")
            multiplayerEnabled = true;

        if (multiplayerEnabled)
        {
            p1scoreLabel = "Player 1: \n";
            p2scoreLabel = "Player 2: \n";
        }
        else
        {
            p1scoreLabel = "You: \n";
            p2scoreLabel = "Bot: \n";
        }

        UI.player1.text = p1scoreLabel + "0";
        UI.player2.text = p2scoreLabel + "0";
    }

    public override void OnNetworkSpawn()
    {
        if (IsHost && IsOwner)
        {
            transform.position = new Vector3(0, 3, -6);
        }
        else if (IsClient && IsOwner)
        {
            transform.SetPositionAndRotation(new Vector3(0, 3, 6), new Quaternion(0, 180, 0, 0));
            GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector3(0, 6, 10);
            GameObject.FindGameObjectWithTag("MainCamera").transform.Rotate(40f, 180f, 0f);
        }
        else if (IsHost && !IsOwner)
        {
            transform.SetPositionAndRotation(new Vector3(0, 3, 6), new Quaternion(0, 180, 0, 0));
        }
    }

    void Update()
    {
        if (multiplayerEnabled)
        {
            if (IsHost && IsOwner)
            {
                if (transform.position.z <= -6)
                    transform.Translate(0, 0, speed / 4 * Time.deltaTime);
            }

            else if (IsClient && IsOwner)
            {
                if (transform.position.z >= 6)
                    transform.Translate(0, 0, speed / 4 * Time.deltaTime);
            }
        }
        else
        {
            if (transform.position.z <= -6)
                transform.Translate(0, 0, speed / 4 * Time.deltaTime);
        }
    }

    public void Move(Vector2 input)
    {
        anim.UpdateAnimations("moving", input.x);
        transform.Translate(input.x * speed * Time.deltaTime, 0, input.y * speed * Time.deltaTime);
    }

    public float Spin(float input)
    {
        return input;
    }

    public IEnumerator Swing(float direction, float spin)
    {
        anim.UpdateAnimations("swing", direction);
        yield return new WaitUntil(() => ball.hasHit);
        trajectory.MovePoints(direction, returningSide, spin);
    }

    public void ProcessSwing(float direction, float spin)
    {
        if (ball.transform.position.z < 0)
            returningSide = "near";
        if (ball.transform.position.z > 0)
            returningSide = "far";

        if (multiplayerEnabled)
        {
            if (IsHost && IsOwner)
            {
                if (trajectory.controlPoints[5].position.x >= transform.position.x + 1)
                    anim.UpdateAnimations("backhand", 1);
                else
                    anim.UpdateAnimations("backhand", -1);
            }

            else if (IsClient && IsOwner)
            {
                if (trajectory.controlPoints[5].position.x <= transform.position.x - 1)
                    anim.UpdateAnimations("backhand", 1);
                else
                    anim.UpdateAnimations("backhand", -1);
            }

            if (IsOwner)
                StartCoroutine(Swing(direction, spin));
        }
        else
        {
            StartCoroutine(Swing(direction, spin));

            if (trajectory.controlPoints[5].position.x >= transform.position.x + 1)
                anim.UpdateAnimations("backhand", 1);
            else
                anim.UpdateAnimations("backhand", -1);
        }
    }

    public void Serve()
    {
        if (!inPlay)
        {
            inPlay = true;

            if (multiplayerEnabled)
            {
                if (IsHost)
                {
                    ResetBall();
                }
            }
            else
                ResetBall();
        }
    }

    private void ResetBall()
    {
        returningSide = "far";
        for (int i = 0; i < trajectory.controlPoints.Length; i++)
            trajectory.controlPoints[i].position = trajectory.defaultPointPosition[i];
        ball.transform.position = trajectory.defaultPointPosition[0];
        ball.speedModifier = Values.speedModifier;
        ball.coroutineAllowed = true;
    }
}
