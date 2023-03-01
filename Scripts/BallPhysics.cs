using System.Collections;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    [SerializeField] private Trajectory trajectory;

    public bool coroutineAllowed = false;
    public bool hasHit = false;
    public float speedModifier = 1;
    public Vector3 returnStartPosition;
    public float spinCoefficient = 1;

    private float tParam = 0f;
    private Vector3 objectPosition;
    private int coroutineCounter = 0;

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
                hasHit = true;
                returnStartPosition = transform.position;
                coroutineAllowed = true;
                //speedModifier += 0.05f;
            }
        }
    }
}
