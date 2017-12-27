using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private void Start()
    {
        GameManager.Instance.PlayerGo += PlayerGo;
    }

    private void PlayerGo(GameManager.MoveSide moveSide, bool swap, float timeToMove)
    {
        Debug.Log(moveSide);

        //StartCoroutine(MoveTo(moveSide, swap, timeToMove));
        CalculateMove(moveSide, swap, timeToMove);
    }

    private IEnumerator MoveTo(GameManager.MoveSide moveSide, bool swap, float timeToMove)
    {
        Vector3 startPosition = transform.position;
        Vector3 spherePosition = Vector3.zero;
        Vector3 pointPosition;

        if (moveSide == GameManager.MoveSide.Up)
        {
            spherePosition = new Vector3(startPosition.x, startPosition.y, startPosition.z + GameManager.Instance.stepBetweenGround.y);
        }
        else if (moveSide == GameManager.MoveSide.Down)
        {
            spherePosition = new Vector3(startPosition.x, startPosition.y, startPosition.z - GameManager.Instance.stepBetweenGround.y);
        }
        else if (moveSide == GameManager.MoveSide.Right)
        {
            spherePosition = new Vector3(startPosition.x + GameManager.Instance.stepBetweenGround.x, startPosition.y, startPosition.z);
        }
        else if (moveSide == GameManager.MoveSide.Left)
        {
            spherePosition = new Vector3(startPosition.x - GameManager.Instance.stepBetweenGround.x, startPosition.y, startPosition.z);
        }

        float alpha = 0, beta = 0;
        float alphaRad, betaRad;
        float radius = GameManager.Instance.stepBetweenGround.x;

        float progress = 0;

        //        while (progress < timeToMove)
        while (progress < 1)
        {
            yield return null;

            //alpha = 180 * progress;
            beta = 180 * progress;


            alphaRad = Mathf.Deg2Rad * (90 - alpha);
            betaRad = Mathf.Deg2Rad * (180 - beta);

            pointPosition.x = spherePosition.x + radius * Mathf.Sin(alphaRad) * Mathf.Cos(betaRad);
            pointPosition.y = spherePosition.y + radius * Mathf.Sin(alphaRad) * Mathf.Sin(betaRad);
            pointPosition.z = spherePosition.z + radius * Mathf.Cos(alphaRad);

            transform.position = pointPosition;

            //transform.position = Vector3.RotateTowards(startPosition, spherePosition, alpha, radius);

            progress += Time.deltaTime;
        }
    }


    bool isMoving = false;
    [Range(0, 1)] public float progress;
    private Vector3 pointPosition;
    Vector3 spherePosition = Vector3.zero;


    private void CalculateMove(GameManager.MoveSide moveSide, bool swap, float timeToMove)
    {
        Vector3 startPosition = transform.position;
        spherePosition = Vector3.zero;

        if (moveSide == GameManager.MoveSide.Up)
        {
            spherePosition = new Vector3(startPosition.x, startPosition.y, startPosition.z + GameManager.Instance.stepBetweenGround.y);
        }
        else if (moveSide == GameManager.MoveSide.Down)
        {
            spherePosition = new Vector3(startPosition.x, startPosition.y, startPosition.z - GameManager.Instance.stepBetweenGround.y);
        }
        else if (moveSide == GameManager.MoveSide.Right)
        {
            spherePosition = new Vector3(startPosition.x + GameManager.Instance.stepBetweenGround.x, startPosition.y, startPosition.z);
        }
        else if (moveSide == GameManager.MoveSide.Left)
        {
            spherePosition = new Vector3(startPosition.x - GameManager.Instance.stepBetweenGround.x, startPosition.y, startPosition.z);
        }

        isMoving = true;
        progress = 0;
    }

    float alpha = 0, beta = 0;
    float alphaRad, betaRad;
    float radius = GameManager.Instance.stepBetweenGround.x;

    private void Update()
    {
        if (!isMoving)
            return;

            //alpha = 180 * progress;
            beta = 180 * progress;

            alphaRad = Mathf.Deg2Rad * (90 - alpha);
            betaRad = Mathf.Deg2Rad * (180 - beta);

            pointPosition.x = spherePosition.x + radius * Mathf.Sin(alphaRad) * Mathf.Cos(betaRad);
            pointPosition.y = spherePosition.y + radius * Mathf.Sin(alphaRad) * Mathf.Sin(betaRad);
            pointPosition.z = spherePosition.z + radius * Mathf.Cos(alphaRad);

            transform.position = pointPosition;

    }
}
