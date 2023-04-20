using System;
using UnityEngine;

namespace Scripts.Logic
{
    public class Drawer : MonoBehaviour
    {
        [SerializeField] private Material material;
        
        private bool _isDrawing;
        private Vector3[] _vertices;
        private Vector2[] _uv;
        private int[] _triangles;
        private Mesh _mesh;
        private GameObject _drawObject;
        private MeshFilter _meshFilter;

        private void Start()
        {
            _vertices = new Vector3[4];
            _uv = new Vector2[4];
            _triangles = new int[6];

            _mesh = new Mesh();
            _mesh.vertices = _vertices;
            _mesh.uv = _uv;
            _mesh.triangles = _triangles;
            
            _drawObject = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));
            _drawObject.transform.parent = transform;
            _drawObject.transform.position = transform.position;

            _meshFilter = _drawObject.GetComponent<MeshFilter>();
            _meshFilter.mesh = _mesh;

            _drawObject.GetComponent<MeshRenderer>().material = material;
        }

        public void DrawSquare(Vector2Int from, Vector2Int to)
        {
            Debug.Log(from + " " + to);
            _vertices[0] = new Vector3(from.x, 1, to.y);
            _vertices[1] = new Vector3(to.x, 1,to.y);
            _vertices[2] = new Vector3(from.x, 1, from.y);
            _vertices[3] = new Vector3(to.x, 1, from.y);

            _uv[0] = new Vector2(from.x, to.y);
            _uv[1] = new Vector2(to.x, to.y);
            _uv[2] = new Vector2(from.x, from.y);
            _uv[3] = new Vector2(to.x, from.y);

            if (from.x < to.x )
            {
                if (from.y <= to.y)
                {
                    SetFrontTriangles();
                }
                else
                {
                    SetFlippedTriangles();
                }
            }
            else
            {
                if (from.y <= to.y)
                {
                    SetFlippedTriangles();
                }
                else
                {
                    SetFrontTriangles();
                }
            }

            _mesh.vertices = _vertices;
            _mesh.uv = _uv;
            _mesh.triangles = _triangles;
            
            _meshFilter.mesh = _mesh;
        }

        private void SetFlippedTriangles()
        {
            _triangles[0] = 1;
            _triangles[1] = 0;
            _triangles[2] = 2;
            _triangles[3] = 2;
            _triangles[4] = 3;
            _triangles[5] = 1;
        }

        private void SetFrontTriangles()
        {
            _triangles[0] = 0;
            _triangles[1] = 1;
            _triangles[2] = 2;
            _triangles[3] = 2;
            _triangles[4] = 1;
            _triangles[5] = 3;
        }
    }
}