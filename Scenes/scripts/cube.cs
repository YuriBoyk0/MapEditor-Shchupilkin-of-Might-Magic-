using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cube : MonoBehaviour
{
    public Camera cam;
    public Transform target;
    public float speedX = 360f;
    public float speedy = 240f;
    public float limitY = 40f;
    public float minDistance = 1.5f;
    public float hideDistance = 2f;
    public LayerMask obstacles;
    public LayerMask noPlayer;
    private float _maxDistance;
    private Vector3 _localPosition;
    private float _currentYRotation;
    private LayerMask _camOrigin;
    // Start is called before the first frame update
    private Vector3 _position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    void Start()
    {
        _localPosition = target.InverseTransformPoint(_position);
        _maxDistance = Vector3.Distance(_position, target.position);
        _camOrigin = cam.cullingMask;
    }

    void LateUpdate()
    {
        _position = target.TransformPoint(_localPosition);
        

        _localPosition = target.InverseTransformPoint(_position);
    }
}
