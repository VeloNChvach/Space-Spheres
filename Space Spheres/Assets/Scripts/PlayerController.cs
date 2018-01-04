using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    private MeshRenderer mesh;
    [HideInInspector] public GameManager.Colors currentColor;
    private Light light;

    private void Awake()
    {
        GameManager.Instance.PlayerGo += PlayerGo;
    }

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        currentColor = GameManager.Colors.Blue;
        light = GetComponent<Light>();
        light.color = new Color(0, 0, 1);
    }

    private void PlayerGo(GameManager.MoveSide moveSide, bool swap)
    {
        StartCoroutine(MoveTo(moveSide, swap));
    }

    private IEnumerator MoveTo(GameManager.MoveSide moveSide, bool swap)
    {
        //Debug.Log(swap + " player");

        Vector3 startPosition = transform.position;
        Vector3 spherePosition = Vector3.zero;
        Vector3 pointPosition = Vector3.zero;

        float alpha = 0;
        float beta = -90;

        if (moveSide == GameManager.MoveSide.Up)
        {
            spherePosition = new Vector3(startPosition.x, transform.localScale.y / 2, startPosition.z + GameManager.Instance.stepBetweenGround.y);
            if (!swap)
            {
                GameManager.Instance.playerPosOnGrid.x += 2;
            }
        }
        else if (moveSide == GameManager.MoveSide.Down)
        {
            spherePosition = new Vector3(startPosition.x, transform.localScale.y / 2, startPosition.z - GameManager.Instance.stepBetweenGround.y);
            if (!swap)
            {
                GameManager.Instance.playerPosOnGrid.x -= 2;
            }
        }
        else if (moveSide == GameManager.MoveSide.Right)
        {
            spherePosition = new Vector3(startPosition.x + GameManager.Instance.stepBetweenGround.x, transform.localScale.y / 2, startPosition.z);
            alpha = 90;
            if (!swap)
            {
                GameManager.Instance.playerPosOnGrid.y += 2;
            }
        }
        else if (moveSide == GameManager.MoveSide.Left)
        {
            spherePosition = new Vector3(startPosition.x - GameManager.Instance.stepBetweenGround.x, transform.localScale.y / 2, startPosition.z);
            alpha = 90;
            if (!swap)
            {
                GameManager.Instance.playerPosOnGrid.y -= 2;
            }
        }

        
        float alphaRad = 0, betaRad = 0;
        float radius = GameManager.Instance.stepBetweenGround.x;

        float progress = 0;
        float speed = 3f;

        

        if (swap)
        {
            float red = 0, green = 0, blue = 0;

            while (progress < 1)
            {
                yield return new WaitForFixedUpdate();

                if (moveSide == GameManager.MoveSide.Up)
                {
                    alpha = 360 * progress;
                    alphaRad = Mathf.Deg2Rad * (180 + alpha);
                    if (alpha > 355) alphaRad = Mathf.Deg2Rad * (360);
                    betaRad = Mathf.Deg2Rad * beta;
                }
                else if (moveSide == GameManager.MoveSide.Down)
                {
                    alpha = 360 * progress;
                    alphaRad = Mathf.Deg2Rad * (-alpha);
                    if (alpha > 355) alphaRad = Mathf.Deg2Rad * (-180);
                    betaRad = Mathf.Deg2Rad * beta;
                }
                else if (moveSide == GameManager.MoveSide.Left)
                {
                    beta = 360 * progress;
                    alphaRad = Mathf.Deg2Rad * (alpha);
                    betaRad = Mathf.Deg2Rad * (beta);
                    if (beta > 355) betaRad = Mathf.Deg2Rad * (180);
                }
                else if (moveSide == GameManager.MoveSide.Right)
                {
                    beta = 360 * progress;
                    alphaRad = Mathf.Deg2Rad * (alpha);
                    betaRad = Mathf.Deg2Rad * (180 - beta);
                    if (beta > 355) betaRad = Mathf.Deg2Rad * (0);
                }

                pointPosition.x = spherePosition.x + radius * Mathf.Sin(alphaRad) * Mathf.Cos(betaRad);
                pointPosition.y = spherePosition.y + radius * Mathf.Sin(alphaRad) * Mathf.Sin(betaRad);
                pointPosition.z = spherePosition.z + radius * Mathf.Cos(alphaRad);

                transform.position = pointPosition;

                if (currentColor == GameManager.Colors.Red)
                {
                    red = Mathf.Lerp(1f, 0f, progress);
                    blue = Mathf.Lerp(0f, 1f, progress);
                    green = 0;
                }
                else if (currentColor == GameManager.Colors.Blue)
                {
                    red = Mathf.Lerp(0f, 1f, progress);
                    blue = Mathf.Lerp(1f, 0f, progress);
                    green = 0;
                }

                mesh.material.color = new Color(red, green, blue);
                light.color = new Color(red, green, blue);

                progress += Time.fixedDeltaTime * speed;
            }

            if (currentColor == GameManager.Colors.Blue)
            {
                currentColor = GameManager.Colors.Red;
            }
            else if (currentColor == GameManager.Colors.Red)
            {
                currentColor = GameManager.Colors.Blue;
            }
                    
        }
        else
        {
            while (progress < 1)
            {
                yield return new WaitForFixedUpdate();

                if (moveSide == GameManager.MoveSide.Up)
                {
                    alpha = 180 * progress;
                    alphaRad = Mathf.Deg2Rad * (180 + alpha);
                    if (alpha > 175) alphaRad = Mathf.Deg2Rad * (360);
                    betaRad = Mathf.Deg2Rad * beta;
                }
                else if (moveSide == GameManager.MoveSide.Down)
                {
                    alpha = 180 * progress;
                    alphaRad = Mathf.Deg2Rad * (-alpha);
                    if (alpha > 175) alphaRad = Mathf.Deg2Rad * (-180);
                    betaRad = Mathf.Deg2Rad * beta;
                }
                else if (moveSide == GameManager.MoveSide.Left)
                {
                    beta = 180 * progress;
                    alphaRad = Mathf.Deg2Rad * (alpha);
                    betaRad = Mathf.Deg2Rad * (beta);
                    if (beta > 175) betaRad = Mathf.Deg2Rad * (180);
                }
                else if (moveSide == GameManager.MoveSide.Right)
                {
                    beta = 180 * progress;
                    alphaRad = Mathf.Deg2Rad * (alpha);
                    betaRad = Mathf.Deg2Rad * (180 - beta);
                    if (beta > 175) betaRad = Mathf.Deg2Rad * (0);
                }

                pointPosition.x = spherePosition.x + radius * Mathf.Sin(alphaRad) * Mathf.Cos(betaRad);
                pointPosition.y = spherePosition.y + radius * Mathf.Sin(alphaRad) * Mathf.Sin(betaRad);
                pointPosition.z = spherePosition.z + radius * Mathf.Cos(alphaRad);

                transform.position = pointPosition;

                progress += Time.fixedDeltaTime * speed;
            }
        }

    }

}
