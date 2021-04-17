using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int divisions = 2;

    private bool makeConnection;
    private Vector3 startPosition;
    private Vector3 endPosition;

    void Start()
    {
        lineRenderer.positionCount = divisions;
    }

    public void MakeConnection (Vector3 startPos, Vector3 endPos)
    {
        startPosition = startPos;
        endPosition = endPos;

        makeConnection = true;
    }

    void Update ()
    {
        if (makeConnection)
        {
            if(divisions <= 2)
            {
                lineRenderer.SetPosition(0, startPosition);
                lineRenderer.SetPosition(1, endPosition);
            }
            else 
            {
                lineRenderer.SetPosition(0, startPosition);

                for(int i=1; i<divisions-1; i++)
                    lineRenderer.SetPosition(i, Vector3.Lerp(startPosition, endPosition, (1/(float)divisions)*i));

                lineRenderer.SetPosition(lineRenderer.positionCount-1, endPosition);                    
            }
        }
    }
}
