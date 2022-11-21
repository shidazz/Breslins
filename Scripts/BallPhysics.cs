using System.Collections;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    [SerializeField] private Trajectory trajectory;

    private float tParam = 0f;
    private Vector3 objectPosition;
    private bool coroutineAllowed = true;
    private bool hasHit = false;
    public float speedModifier = 0.5f;
    public static Vector3 returnStartPosition;

    void Update()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute());
        }
    }

    private IEnumerator GoByTheRoute()
    {
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

                yield return new WaitForEndOfFrame();

                if (hasHit)
                {
                    hasHit = false;
                    break;
                }
            }
            tParam = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasHit)
        {
            if (collision.gameObject.CompareTag("Paddle") || collision.gameObject.CompareTag("Wall")) 
            {
                hasHit = true;
                returnStartPosition = transform.position;
                trajectory.MovePoints();
                tParam = 0;
                coroutineAllowed = true;
                Debug.Log("Collision");
            }
        }
    }

    public void ResetBall()
    {
        transform.position = new Vector3(0, 3.5f, 5);
        coroutineAllowed = true;
    }
}
