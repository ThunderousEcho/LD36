using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pyramid : MonoBehaviour {

    const float movementSpeed = 2;

    LinkedList<Vector2> pathRight = new LinkedList<Vector2>();
    LinkedList<Vector2> pathLeft = new LinkedList<Vector2>();

    public MeshFilter pathMesh;

    float lastPathAdd = 0;
    sbyte lastAng = -5;

    void Update() {

        if (pathRight.Count > 64) {
            pathRight.RemoveLast();
            pathLeft.RemoveLast();
        }
            
        Vector2 motion = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Time.deltaTime * movementSpeed;
        if (motion != Vector2.zero) {
            transform.Translate(motion);

            float angle = Mathf.Atan2(motion.y, motion.x) * Mathf.Rad2Deg;

            Debug.Log(angle);

            if (angle > 0) {
                if (angle < 90)
                    addPoints(0);
                else
                    addPoints(1);
            } else {
                if (angle > -90)
                    addPoints(-1);
                else
                    addPoints(-2);
            }

            Mesh m = new Mesh();
            Vector3[] points = new Vector3[pathRight.Count * 2];

            var pointsR = pathRight.GetEnumerator();
            var pointsL = pathLeft.GetEnumerator();

            List<int> tris = new List<int>();

            for (int i = 0; i < pathRight.Count * 2; i += 2) {

                pointsR.MoveNext();
                points[i] = pointsR.Current;

                pointsL.MoveNext();
                points[i + 1] = pointsL.Current;

                if (i < (pathRight.Count - 1) * 2) {
                    tris.Add(i);
                    tris.Add(i + 1);
                    tris.Add(i + 2);

                    tris.Add(i + 1);
                    tris.Add(i + 3);
                    tris.Add(i + 2);
                    
                }
            }

            m.vertices = points;
            m.triangles = tris.ToArray();
            m.RecalculateBounds();
            m.RecalculateNormals();
            m.Optimize();
            pathMesh.mesh = m;

            if (Time.time - lastPathAdd > 0.25f) {
                lastPathAdd = Time.time;
            } else {
                pathLeft.RemoveFirst();
                pathRight.RemoveFirst();
            }
        }
    }

    void addPoints(sbyte angId) {

        /*if (lastAng != angId) {
            if (lastAng != -5) {
                Vector2 lastLeftPoint = getLeftPoint(lastAng);

                pathRight.AddFirst(transform.position - (Vector3)lastLeftPoint);
                pathLeft.AddFirst(transform.position + (Vector3)lastLeftPoint);
            }
            lastAng = angId;
        }*/

        Vector2 leftPoint = getLeftPoint(angId);

        pathRight.AddFirst(transform.position - (Vector3)leftPoint);
        pathLeft.AddFirst(transform.position + (Vector3)leftPoint);
    }

    Vector2 getLeftPoint(sbyte angId) {
        switch (angId) {
            case 0:
                return new Vector2(0.5f, -0.5f);
            case 1:
                return new Vector2(0.5f, 0.5f);
            case -1:
                return new Vector2(-0.5f, -0.5f);
            case -2:
                return new Vector2(-0.5f, 0.5f);
        }

        throw new System.Exception(angId + " is not a valid input value for getLeftPoint().");
    }
}