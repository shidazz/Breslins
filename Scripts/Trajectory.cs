using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] public Transform[] controlPoints;
    [SerializeField] private BallPhysics ball;

    private Vector3 gizmosPosition1;
    private Vector3 gizmosPosition2;

    /*
    private void OnDrawGizmos()
    {
        for (float t = 0; t <= 1; t += 0.05f)
        {
            gizmosPosition1 = Mathf.Pow(1 - t, 3) * controlPoints[0].position + 3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position + 3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position + Mathf.Pow(t, 3) * controlPoints[3].position;
            gizmosPosition2 = Mathf.Pow(1 - t, 3) * controlPoints[4].position + 3 * Mathf.Pow(1 - t, 2) * t * controlPoints[5].position + 3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[6].position + Mathf.Pow(t, 3) * controlPoints[7].position;

            Gizmos.DrawSphere(gizmosPosition1, 0.25f);
            Gizmos.DrawSphere(gizmosPosition2, 0.25f);
        }
        for (int i = 0; i < 7; i++)
        {
            Gizmos.DrawLine(new Vector3(controlPoints[i].position.x, controlPoints[i].position.y, controlPoints[i].position.z), new Vector3(controlPoints[i+1].position.x, controlPoints[i+1].position.y, controlPoints[i+1].position.z));
        }
    }
    */

    public void MovePoints(string tableSide, string returningSide, float spin)
    {
        Debug.Log("points moved");
        float travelDirection;
        float bounceDistance;
        float bounceModifier = 0.6f;
        Vector3 startPoint;
        Vector3 p1 = new Vector3(0, 4.5f, -1.5f);
        Vector3 p2 = new Vector3(0, 4.5f, 1);
        Vector3 landPoint = new Vector3(0, 2.5f, 0);
        Vector3 p5;
        Vector3 p6;

        controlPoints[0].position = ball.returnStartPosition;
        startPoint = controlPoints[0].position;

        if (tableSide == "left")
            landPoint.x = Random.Range(-2.5f, 0);
        if (tableSide == "right")
            landPoint.x = Random.Range(2.5f, 0);
        if (tableSide == "any")
            landPoint.x = Random.Range(-2.5f, 2.5f);

        if (returningSide == "far")
        {
            p1.z = 1.5f;
            p2.z = -1;
            landPoint.z = Random.Range(-4.5f, -2.5f);
        }
        if (returningSide == "near")
        {
            p1.z = -1.5f;
            p2.z = 1;
            landPoint.z = Random.Range(2.5f, 4.5f);
        }

        controlPoints[3].position = landPoint;


        travelDirection = landPoint.x - startPoint.x;

        p1.x = (startPoint.x + landPoint.x) / 2;
        controlPoints[1].position = p1;

        p2.x = p1.x + travelDirection * 0.2f;
        controlPoints[2].position = p2;

        controlPoints[4].position = landPoint;

        bounceDistance = (((p1.z - startPoint.z) + (landPoint.z - p2.z)) / 2) * bounceModifier;

        p5 = new Vector3(landPoint.x + travelDirection * 0.2f, p2.y - 0.8f, landPoint.z + bounceDistance);
        controlPoints[5].position = p5;
        
        p6 = new Vector3(p5.x + (p5.x - landPoint.x), p5.y - 0.5f, p5.z + (p5.z - landPoint.z));
        controlPoints[6].position = p6;

        controlPoints[7].position = new Vector3(p6.x + (p6.x - p5.x), 1, p6.z + (p6.z - p5.z));
    }
}
