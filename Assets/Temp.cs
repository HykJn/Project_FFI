using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    public Transform start, end;
    Vector2 middle;
    private float dist = 10f;
    bool isStart;
    float t = 0.0f;
    List<Vector2> points = new List<Vector2>();
    private void Start()
    {
        middle = (start.position + end.position) / 2;

        Vector2 temp = new Vector2(-(end.position - start.position).y, (end.position - start.position).x);
        middle += temp.normalized;
        Debug.Log(middle);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isStart = !isStart;
            points.Clear();
        }

        if (isStart)
        {
            Vector2 p1 = Vector2.Lerp(start.position, middle, t);
            Vector2 p2 = Vector2.Lerp(middle, end.position, t);
            Vector2 p = Vector2.Lerp(p1, p2, t);
            points.Add(p);
            this.transform.position = p;

            t += Time.deltaTime;
            if (t >= 1.0f)
            {
                isStart = false;
                t = 0.0f;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(start.position, middle);
        Gizmos.DrawLine(end.position, middle);

        for (int i = 0; i < points.Count - 1; i++)
        {
            Gizmos.DrawLine(points[i], points[i + 1]);
        }
    }
}