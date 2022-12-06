using System.Collections;
using UnityEngine;

public class TrainingBot : MonoBehaviour
{
    [SerializeField] private Trajectory trajectory;
    [SerializeField] private BallPhysics ball;

    private bool moving = false;
    private float targetPosition;

    private void Update()
    {
        if (trajectory.controlPoints[3].position.z > 0 && targetPosition != trajectory.controlPoints[5].position.x + 1)
            targetPosition = trajectory.controlPoints[5].position.x + 1;
        if (!moving && transform.position.x != targetPosition)
            StartCoroutine(MoveToBall(targetPosition, 0.5f));
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
        Debug.Log("Moved to ball");
        yield return new WaitUntil(() => ball.hasHit);
        trajectory.MovePoints("any", "opponent", 0);
        Debug.Log("Bot hit ball");
        moving = false;
    }
}
