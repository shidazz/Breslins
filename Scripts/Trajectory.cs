using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] public Transform[] controlPoints;

    private Vector3 gizmosPosition1;
    private Vector3 gizmosPosition2;
    private Vector3 point1Range;
    private Vector3 point2Range;
    private Vector3 landPointRange;

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

    public void MovePoints(float landPointX, float spin, string tableSide)
    {
        float travelDirection;
        float bounceDistance;
        float bounceModifier = 0.5f;

        if (tableSide == "right")
        {
            point1Range.z = Random.Range(0, 2);
            point2Range.z = Random.Range(-2, 0);
            landPointRange.z = Random.Range(-4.5f, -2.5f);
        }
        if (tableSide == "left")
        {
            point1Range.z = Random.Range(-2, 0);
            point2Range.z = Random.Range(0, 2);
            landPointRange.z = Random.Range(2.5f, 4.5f);
        }

        controlPoints[0].position = BallPhysics.returnStartPosition;

        landPointRange.x = landPointX;
        controlPoints[3].position = new Vector3(landPointRange.x, 2.5f, landPointRange.z);

        travelDirection = landPointRange.x - controlPoints[0].position.x;

        point1Range.x = (controlPoints[0].position.x + controlPoints[3].position.x) / 2;
        controlPoints[1].position = new Vector3(point1Range.x, Random.Range(4, 5), point1Range.z);

        point2Range.x = controlPoints[1].position.x + travelDirection * 0.3f;
        controlPoints[2].position = new Vector3(point2Range.x, Random.Range(4, 5), point2Range.z);

        controlPoints[4].position = controlPoints[3].position;

        bounceDistance = (((controlPoints[1].position.z - controlPoints[0].position.z) + (controlPoints[3].position.z - controlPoints[2].position.z)) / 2) * bounceModifier;

        controlPoints[5].position = new Vector3(controlPoints[3].position.x + (controlPoints[3].position.x - controlPoints[2].position.x), controlPoints[2].position.y - 0.5f, controlPoints[3].position.z + bounceDistance);
        
        controlPoints[6].position = new Vector3(controlPoints[5].position.x + (controlPoints[5].position.x - controlPoints[4].position.x), controlPoints[5].position.y - 0.5f, controlPoints[5].position.z + (controlPoints[5].position.z - controlPoints[4].position.z));
        
        controlPoints[7].position = new Vector3(controlPoints[6].position.x + (controlPoints[6].position.x - controlPoints[5].position.x), 1, controlPoints[6].position.z + (controlPoints[6].position.z - controlPoints[5].position.z));
    }
}
