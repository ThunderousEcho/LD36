using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pyramid : MonoBehaviour {
    const float movementSpeed = 4;

    public bool isPlayer = false;
    public Pyramid player;

    public MeshFilter filter;
    public Rigidbody2D body;

    static Vector3[] verts = {
        new Vector3(0, 0, -1),
        new Vector3(1, 1, 0),
        new Vector3(-1, 1, 0),

        new Vector3(0, 0, -1),
        new Vector3(-1, 1, 0),
        new Vector3(-1, -1, 0),

        new Vector3(0, 0, -1),
        new Vector3(-1, -1, 0),
        new Vector3(1, -1, 0),

        new Vector3(0, 0, -1),
        new Vector3(1, -1, 0),
        new Vector3(1, 1, 0)
    };

    static Vector2[] uvs = {
        new Vector2(0.5f, 1),
        new Vector2(0, 0),
        new Vector2(1, 0),

        new Vector2(0.5f, 1),
        new Vector2(0, 0),
        new Vector2(1, 0),

        new Vector2(0.5f, 1),
        new Vector2(0, 0),
        new Vector2(1, 0),

        new Vector2(0.5f, 1),
        new Vector2(0, 0),
        new Vector2(1, 0)
    };

    static int[] tris= {
        0, 2, 1,
        3, 5, 4,
        6, 8, 7,
        9, 11, 10
    };

    void Start() {
        Mesh m = new Mesh();
        m.vertices = verts;
        m.triangles = tris;
        m.uv = uvs;
        m.RecalculateNormals();
        m.RecalculateBounds();
        m.Optimize();
        filter.mesh = m;
    }

    void Update() {
        if (isPlayer) {
            Vector2 motion = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Time.deltaTime * movementSpeed;
            body.MovePosition((Vector2)transform.position + motion);
        }

        float distToMouse = Vector2.Distance(Input.mousePosition, Camera.main.WorldToScreenPoint(transform.position));
        distToMouse /= Screen.height;
        distToMouse *= 8;
        if (distToMouse < 1) {
            filter.transform.localScale = Vector3.zero;
        } else if (distToMouse < 2) {
            filter.transform.localScale = Vector3.one * (distToMouse - 1) * 0.5f;
            filter.transform.localPosition = -Vector3.forward * (2-distToMouse) * 4;
        } else {
            filter.transform.localScale = Vector3.one * 0.5f;
            filter.transform.localPosition = Vector3.zero;
        }
    }
}