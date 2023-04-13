using UnityEngine;
using Unity.Netcode;

public class Trajectory : NetworkBehaviour
{
    public Transform[] controlPoints;
    public Vector3[] defaultPointPosition;

    private BallPhysics ball;
    private Vector3 gizmosPosition1;
    private Vector3 gizmosPosition2;

    private readonly float spinCoefficient = Values.spinCoefficient;

    void Awake()
    {
        ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallPhysics>();
    }

    void Start()
    {
        for (int i = 0; i < controlPoints.Length; i++) 
            defaultPointPosition[i] = controlPoints[i].position;
    }

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
    
    public void MovePoints(float tableSide, string returningSide, float spin)
    {
        float travelDirection;
        float bounceDistance;
        float bounceModifier = 0.6f;
        float topspin;
        float sidespin;
        Vector3 startPoint;
        Vector3 p1 = new (0, 4.5f, -1.5f);
        Vector3 p2 = new (0, 4.3f, 1);
        Vector3 landPoint = new (0, 2.5f, 0);
        Vector3 p5;
        Vector3 p6;
        Vector3 p7;

        if (spin == 2)
        {
            topspin = 1;
            sidespin = 0;
            if (ball.speedModifier < Values.speedModifier + 0.1f)
                ball.speedModifier += 0.1f;
            else
                ball.speedModifier = Values.speedModifier;
        }
        else
        {
            sidespin = spin;
            topspin = 0;
            ball.speedModifier = 1;
        }

        sidespin *= spinCoefficient;
        topspin *= spinCoefficient;

        controlPoints[0].position = ball.returnStartPosition;
        startPoint = controlPoints[0].position;

        if (tableSide == 0)
            landPoint.x = Random.Range(-2.5f, 2.5f);

        if (returningSide == "far")
        {
            if (tableSide < 0)
                landPoint.x = Random.Range(2.5f, 1);
            if (tableSide > 0)
                landPoint.x = Random.Range(-2.5f, -1);

            p1.z = 1.5f;
            p2.z = -1;
            landPoint.z = -3.5f;
        }
        else if (returningSide == "near")
        {
            if (tableSide < 0)
                landPoint.x = Random.Range(-2.5f, -1);
            if (tableSide > 0)
                landPoint.x = Random.Range(2.5f, 1);

            p1.z = -1.5f;
            p2.z = 1;
            landPoint.z = 3.5f;
        }

        travelDirection = landPoint.x - startPoint.x;
        sidespin *= Mathf.Sign(landPoint.z);

        p1.x = (startPoint.x + landPoint.x) / 2;

        p2.x = p1.x + travelDirection / 4;

        p1.x -= sidespin * 1.5f;
        p1.y -= topspin / 4;
        p1.z += topspin * Mathf.Sign(landPoint.z);

        p2.x -= sidespin;
        p2.y -= topspin / 2;
        p2.z += topspin * 2 * Mathf.Sign(landPoint.z);

        controlPoints[1].position = p1;
        controlPoints[2].position = p2;

        controlPoints[3].position = landPoint;
        controlPoints[4].position = landPoint;

        bounceDistance = (((p1.z - startPoint.z) + (landPoint.z - p2.z)) / 2) * bounceModifier;

        p5 = new Vector3(landPoint.x + travelDirection * 0.2f, p2.y - 0.8f, landPoint.z + bounceDistance);

        p5.x += sidespin * 1.2f;
        p5.y += topspin / 2;
        p5.z += topspin * Mathf.Sign(landPoint.z);
        
        p6 = new Vector3(p5.x + (p5.x - landPoint.x), p5.y - 0.5f, p5.z + (p5.z - landPoint.z));

        p7 = new Vector3(p6.x + (p6.x - p5.x), 1, p6.z + (p6.z - p5.z));

        controlPoints[6].position = p6;
        controlPoints[5].position = p5;
        controlPoints[7].position = p7;
    }
}
