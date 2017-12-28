using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.PlayerGo += PlayerGo;
    }

    private void PlayerGo(GameManager.MoveSide moveSide, bool swap)
    {
        StartCoroutine(MoveTo(moveSide, swap));
    }

    private IEnumerator MoveTo(GameManager.MoveSide moveSide, bool swap)
    {
        Vector3 startPosition = transform.position;
        Vector3 spherePosition = Vector3.zero;
        Vector3 pointPosition = Vector3.zero;

        float alpha = 0;
        float beta = -90;

        if (moveSide == GameManager.MoveSide.Up)
        {
            spherePosition = new Vector3(startPosition.x, transform.localScale.y / 2, startPosition.z + GameManager.Instance.stepBetweenGround.y);
            GameManager.Instance.playerPosOnGrid.x += 2;
        }
        else if (moveSide == GameManager.MoveSide.Down)
        {
            spherePosition = new Vector3(startPosition.x, transform.localScale.y / 2, startPosition.z - GameManager.Instance.stepBetweenGround.y);
            GameManager.Instance.playerPosOnGrid.x -= 2;
        }
        else if (moveSide == GameManager.MoveSide.Right)
        {
            spherePosition = new Vector3(startPosition.x + GameManager.Instance.stepBetweenGround.x, transform.localScale.y / 2, startPosition.z);
            alpha = 90;
            GameManager.Instance.playerPosOnGrid.y += 2;
        }
        else if (moveSide == GameManager.MoveSide.Left)
        {
            spherePosition = new Vector3(startPosition.x - GameManager.Instance.stepBetweenGround.x, transform.localScale.y / 2, startPosition.z);
            alpha = 90;
            GameManager.Instance.playerPosOnGrid.y -= 2;
        }

        
        float alphaRad = 0, betaRad = 0;
        float radius = GameManager.Instance.stepBetweenGround.x;

        float progress = 0;
        float speed = 3f;

        while (progress < 1)
        {
            yield return new WaitForFixedUpdate();

            if (moveSide == GameManager.MoveSide.Up)
            {
                alpha = 180 * progress;
                alphaRad = Mathf.Deg2Rad * (180 + alpha);
                if (alpha > 170) alphaRad = Mathf.Deg2Rad * (360);
                betaRad = Mathf.Deg2Rad * beta;
            }
            else if (moveSide == GameManager.MoveSide.Down)
            {
                alpha = 180 * progress;
                alphaRad = Mathf.Deg2Rad * (-alpha);
                if (alpha > 170) alphaRad = Mathf.Deg2Rad * (-180);
                betaRad = Mathf.Deg2Rad * beta;
            }
            else if (moveSide == GameManager.MoveSide.Left)
            {
                beta = 180 * progress;
                alphaRad = Mathf.Deg2Rad * (alpha);
                betaRad = Mathf.Deg2Rad * (beta);
                if (beta > 170) betaRad = Mathf.Deg2Rad * (180);
            }
            else if (moveSide == GameManager.MoveSide.Right)
            {
                beta = 180 * progress;
                alphaRad = Mathf.Deg2Rad * (alpha);
                betaRad = Mathf.Deg2Rad * (180 - beta);
                if (beta > 170) betaRad = Mathf.Deg2Rad * (0);
            }

            //Debug.Log(alpha + " " + beta);

            pointPosition.x = spherePosition.x + radius * Mathf.Sin(alphaRad) * Mathf.Cos(betaRad);
            pointPosition.y = spherePosition.y + radius * Mathf.Sin(alphaRad) * Mathf.Sin(betaRad);
            pointPosition.z = spherePosition.z + radius * Mathf.Cos(alphaRad);

            transform.position = pointPosition;

            progress += Time.fixedDeltaTime * speed;
        }
    }


}
