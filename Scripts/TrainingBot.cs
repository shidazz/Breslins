using System.Collections;
using UnityEngine;

public class TrainingBot : MonoBehaviour
{
    [SerializeField] private Trajectory trajectory;
    [SerializeField] private BallPhysics ball;

    public float paddleOffset = 0.1f;

    private bool moving = false;
    private float targetPosition;
    private float ballPath;
    private float ballLandPoint;
    private AnimationController anim;

    void Awake()
    {
        anim = GetComponent<AnimationController>();
    }

    private void Update()
    {
        ballPath = trajectory.controlPoints[5].position.x;
        ballLandPoint = trajectory.controlPoints[3].position.z;

        if (ballLandPoint > 0 && targetPosition != ballPath + paddleOffset)
            targetPosition = ballPath + paddleOffset;
        if (!moving && transform.position.x != targetPosition && ballLandPoint > 0)
            StartCoroutine(MoveToBall(targetPosition, 0.5f));

        if (transform.position.z - ball.transform.position.z <= 2)
            anim.UpdateAnimations("swing", 1);
    }

    private IEnumerator MoveToBall(float endPosition, float duration)
    {
        moving = true;
        float time = 0;
        float startPosition = transform.position.x;
        Vector3 lerpPosition = new Vector3(startPosition, transform.position.y, transform.position.z);

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
        trajectory.MovePoints("any", "far", 0);
        moving = false;
    }
}
