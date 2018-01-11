using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 newPosition = Vector3.zero;
    //private Transform playerTransform;
    private float[] bounds;

    private void Start()
    {
        //cameraTransform = gameObject.transform;
        //GameManager.Instance.player.position.x
        //newPosition = Vector3.zero;
        startPosition = gameObject.transform.position;
        startRotation = gameObject.transform.rotation;
        //playerTransform = GameManager.Instance.player;
        bounds = new float[2] { -4.6f, 4.6f };
    }

    private void LateUpdate()
    {
        newPosition = new Vector3(Mathf.Clamp(GameManager.Instance.player.position.x, bounds[0], bounds[1]), startPosition.y, startPosition.z);
        transform.SetPositionAndRotation(Vector3.Lerp(transform.position, newPosition, 0.1f), startRotation);
        
    }

}
