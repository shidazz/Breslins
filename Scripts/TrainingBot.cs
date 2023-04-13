using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingBot : MonoBehaviour
{
    private Trajectory trajectory;
    private BallPhysics ball;

    public float paddleOffset = 0.1f;
    public float botSpeed = 2f;

    private bool moving = false;
    private float targetPosition;
    private float ballPath;
    private float ballLandPoint;
    private AnimationController anim;
    private readonly float[] spinOptions = {-1, 0 , 1, 2};

    void Awake()
    {
        anim = GetComponent<AnimationController>();
        trajectory = GameObject.FindGameObjectWithTag("Trajectory").GetComponent<Trajectory>();
        ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallPhysics>();
    }

    private void Update()
    {
        ballPath = trajectory.controlPoints[5].position.x;
        ballLandPoint = trajectory.controlPoints[3].position.z;

        if (ballLandPoint > 0 && targetPosition != ballPath + paddleOffset)
            targetPosition = ballPath + paddleOffset;
        if (!moving && transform.position.x != targetPosition && ballLandPoint > 0)
            StartCoroutine(MoveToBall(targetPosition, 1 / botSpeed));

        if (transform.position.z - ball.transform.position.z <= 2)
            anim.UpdateAnimations("swing", 1);
    }

    private IEnumerator MoveToBall(float endPosition, float duration)
    {
        moving = true;
        float time = 0;
        float startPosition = transform.position.x;
        int spinOutput;
        Vector3 lerpPosition = new (startPosition, transform.position.y, transform.position.z);

        while (time < duration)
        {
            lerpPosition.x = Mathf.Lerp(startPosition, endPosition, time / duration);
            transform.position = lerpPosition;
            time += Time.deltaTime;
            yield return null;
        }
        lerpPosition.x = endPosition;
        transform.position = lerpPosition;
        yield return new WaitUntil(() => ball.hasHit);
        if (SceneManager.GetActiveScene().name == "Main Menu")
            spinOutput = 1;
        else
            spinOutput = Random.Range(0, spinOptions.Length);
        trajectory.MovePoints(0, "far", spinOptions[spinOutput]);
        moving = false;
    }
}
