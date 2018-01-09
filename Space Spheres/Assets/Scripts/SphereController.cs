﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    private MeshRenderer mesh;
    private GameManager.Colors currentColor;
    private Light light;

    private void Awake()
    {
        GameManager.Instance.SphereGo += SphereGo;
    }

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        currentColor = GameManager.Colors.Red;
        light = GetComponent<Light>();
        light.color = new Color(1, 0, 0);
    }

    private void SphereGo(Transform sphere, GameManager.MoveSide moveSide, bool swap, int numInList)
    {
        GameManager.Instance.sphereList[numInList] = transform;
        StartCoroutine(MoveTo(sphere, moveSide, swap, numInList));
    }

    private IEnumerator MoveTo(Transform sphere, GameManager.MoveSide moveSide, bool swap, int numIntList)
    {
        if (sphere == transform)
        {
            Vector3 startPosition = transform.position;
            Vector3 spherePosition = Vector3.zero;
            Vector3 pointPosition = Vector3.zero;

            float alpha = 0;
            float beta = -90;

            if (moveSide == GameManager.MoveSide.Up)
            {
                spherePosition = new Vector3(startPosition.x, -transform.localScale.y / 2, startPosition.z + GameManager.Instance.stepBetweenGround.y);
                GameManager.Instance.spherePosOnGrid[numIntList] = new Vector2(GameManager.Instance.spherePosOnGrid[numIntList].x + 2, GameManager.Instance.spherePosOnGrid[numIntList].y);
            }
            else if (moveSide == GameManager.MoveSide.Down)
            {
                spherePosition = new Vector3(startPosition.x, -transform.localScale.y / 2, startPosition.z - GameManager.Instance.stepBetweenGround.y);
                GameManager.Instance.spherePosOnGrid[numIntList] = new Vector2(GameManager.Instance.spherePosOnGrid[numIntList].x - 2, GameManager.Instance.spherePosOnGrid[numIntList].y);
            }
            else if (moveSide == GameManager.MoveSide.Right)
            {
                spherePosition = new Vector3(startPosition.x + GameManager.Instance.stepBetweenGround.x, -transform.localScale.y / 2, startPosition.z);
                alpha = 90;
                GameManager.Instance.spherePosOnGrid[numIntList] = new Vector2(GameManager.Instance.spherePosOnGrid[numIntList].x, GameManager.Instance.spherePosOnGrid[numIntList].y + 2);
            }
            else if (moveSide == GameManager.MoveSide.Left)
            {
                spherePosition = new Vector3(startPosition.x - GameManager.Instance.stepBetweenGround.x, -transform.localScale.y / 2, startPosition.z);
                alpha = 90;
                GameManager.Instance.spherePosOnGrid[numIntList] = new Vector2(GameManager.Instance.spherePosOnGrid[numIntList].x, GameManager.Instance.spherePosOnGrid[numIntList].y - 2);
            }


            float alphaRad = 0, betaRad = 0;
            float radius = GameManager.Instance.stepBetweenGround.x;

            float progress = 0;
            float speed = 3f;

            float red = 0, green = 0, blue = 0;

            while (progress < 1)
            {
                yield return new WaitForFixedUpdate();

                if (moveSide == GameManager.MoveSide.Up)
                {
                    alpha = 180 * progress;
                    alphaRad = Mathf.Deg2Rad * (180 - alpha);
                    if (alpha > 177) alphaRad = Mathf.Deg2Rad * (360);
                    betaRad = Mathf.Deg2Rad * beta;
                }
                else if (moveSide == GameManager.MoveSide.Down)
                {
                    alpha = 180 * progress;
                    alphaRad = Mathf.Deg2Rad * (alpha);
                    if (alpha > 177) alphaRad = Mathf.Deg2Rad * (-180);
                    betaRad = Mathf.Deg2Rad * beta;
                }
                else if (moveSide == GameManager.MoveSide.Left)
                {
                    beta = 180 * progress;
                    alphaRad = Mathf.Deg2Rad * (alpha);
                    betaRad = Mathf.Deg2Rad * (-beta);
                    if (beta > 177) betaRad = Mathf.Deg2Rad * (180);
                }
                else if (moveSide == GameManager.MoveSide.Right)
                {
                    beta = 180 * progress;
                    alphaRad = Mathf.Deg2Rad * (alpha);
                    betaRad = Mathf.Deg2Rad * (180 + beta);
                    if (beta > 177) betaRad = Mathf.Deg2Rad * (0);
                }

                pointPosition.x = spherePosition.x + radius * Mathf.Sin(alphaRad) * Mathf.Cos(betaRad);
                pointPosition.y = spherePosition.y + radius * Mathf.Sin(alphaRad) * Mathf.Sin(betaRad);
                pointPosition.z = spherePosition.z + radius * Mathf.Cos(alphaRad);

                transform.position = pointPosition;


                if (swap && currentColor == GameManager.Colors.Red)
                {
                    red = Mathf.Lerp(1f, 0f, progress);
                    blue = Mathf.Lerp(0f, 1f, progress);
                    green = 0;
                    mesh.material.color = new Color(red, green, blue);
                    light.color = new Color(red, green, blue);
                }
                else if (swap && currentColor == GameManager.Colors.Blue)
                {
                    red = Mathf.Lerp(0f, 1f, progress);
                    blue = Mathf.Lerp(1f, 0f, progress);
                    green = 0;
                    mesh.material.color = new Color(red, green, blue);
                    light.color = new Color(red, green, blue);
                }

                progress += Time.fixedDeltaTime * speed;
            }

            if (swap && currentColor == GameManager.Colors.Blue)
            {
                currentColor = GameManager.Colors.Red;
            }
            else if (swap && currentColor == GameManager.Colors.Red)
            {
                currentColor = GameManager.Colors.Blue;
            }

        }
    }

}
