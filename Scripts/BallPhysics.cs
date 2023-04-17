using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class BallPhysics : MonoBehaviour
{
    public bool coroutineAllowed = false;
    public bool hasHit = false;
    public float speedModifier;
    public Vector3 returnStartPosition;
    public static int player1Score = 0;
    public static int player2Score = 0;

    private Trajectory trajectory;
    private AudioManager audioManager;
    private float tParam = 0f;
    private Vector3 objectPosition;
    private int coroutineCounter = 0;

    void Awake()
    {
        trajectory = GameObject.FindGameObjectWithTag("Trajectory").GetComponent<Trajectory>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    void Start()
    {
        speedModifier = Values.speedModifier;
    }

    void Update()
    {
        if (coroutineAllowed && coroutineCounter == 0)
        {
            StartCoroutine(TravelTrajectory());
        }
    }

    private IEnumerator TravelTrajectory()
    {
        coroutineCounter++;
        coroutineAllowed = false;

        Vector3[] p = new Vector3[8];

        for (int i = 0; i < trajectory.controlPoints.Length; i++)
            p[i] = trajectory.controlPoints[i].position;

        for (int i = 0; i <= 4; i += 4)
        {
            while (tParam < 1)
            {
                tParam += Time.deltaTime * speedModifier;

                objectPosition = Mathf.Pow(1 - tParam, 3) * p[i] + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p[i+1] + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p[i+2] + Mathf.Pow(tParam, 3) * p[i+3];

                transform.position = objectPosition;

                yield return new WaitForSeconds(0.000016f);

                if (hasHit)
                {
                    hasHit = false;
                    break;
                }
            }
            if (!hasHit && transform.position.y <= 1)
            {
                Player.inPlay = false;

                if (transform.position.z > 0)
                {
                    player1Score++;
                    UI.player1.text = Player.p1scoreLabel + player1Score;
                }
                if (transform.position.z < 0)
                {
                    player2Score++;
                    UI.player2.text = Player.p2scoreLabel + player2Score;
                }
            }

            tParam = 0;
        }
        coroutineCounter = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasHit)
        {
            if (collision.gameObject.CompareTag("Paddle")) 
            {
                if (transform.position.z < 0)
                    audioManager.Play("PaddleHit_1");
                if (transform.position.z > 0)
                    audioManager.Play("PaddleHit_2");
                hasHit = true;
                returnStartPosition = transform.position;
                coroutineAllowed = true;
                //speedModifier += 0.05f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Table"))
            audioManager.Play("TableHit_1");
    }
}
