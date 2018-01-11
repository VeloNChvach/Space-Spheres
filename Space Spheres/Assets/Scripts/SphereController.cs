using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    private MeshRenderer mesh;
    private Light light;
    private GameManager.Colors colorToChange;
    private bool isColor = true;

    private void Awake()
    {
        GameManager.Instance.SphereGo += SphereGo;
    }

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        light = GetComponent<Light>();
    }

    private void SphereGo(Transform sphere, GameManager.MoveSide moveSide, bool swap, int numInList)
    {
        if (gameObject.transform != sphere)
        {
            return;
        }

        if (isColor)
        {
            if (GameManager.Instance.sphereColor[numInList] == GameManager.Colors.Blue)
            {
                mesh.material.color = new Color(0, 0, 1);
                light.color = new Color(0, 0, 1);
            }
            else if (GameManager.Instance.sphereColor[numInList] == GameManager.Colors.Red)
            {
                mesh.material.color = new Color(1, 0, 0);
                light.color = new Color(1, 0, 0);
            }
            else if (GameManager.Instance.sphereColor[numInList] == GameManager.Colors.Green)
            {
                mesh.material.color = new Color(0, 1, 0);
                light.color = new Color(0, 1, 0);
            }

            isColor = false;
        }

        if (swap && GameManager.Instance.playerColor == GameManager.Colors.Blue)
        {
            colorToChange = GameManager.Colors.Blue;
        }
        else if (swap && GameManager.Instance.playerColor == GameManager.Colors.Red)
        {
            colorToChange = GameManager.Colors.Red;
        }
        else if (swap && GameManager.Instance.playerColor == GameManager.Colors.Green)
        {
            colorToChange = GameManager.Colors.Green;
        }

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


                if (swap && GameManager.Instance.sphereColor[numIntList] == GameManager.Colors.Red && colorToChange == GameManager.Colors.Blue)
                {
                    red = Mathf.Lerp(1f, 0f, progress);
                    blue = Mathf.Lerp(0f, 1f, progress);
                    green = 0;
                    mesh.material.color = new Color(red, green, blue);
                    light.color = new Color(red, green, blue);
                }
                else if (swap && GameManager.Instance.sphereColor[numIntList] == GameManager.Colors.Blue && colorToChange == GameManager.Colors.Red)
                {
                    red = Mathf.Lerp(0f, 1f, progress);
                    blue = Mathf.Lerp(1f, 0f, progress);
                    green = 0;
                    mesh.material.color = new Color(red, green, blue);
                    light.color = new Color(red, green, blue);
                }
                else if (swap && GameManager.Instance.sphereColor[numIntList] == GameManager.Colors.Green && colorToChange == GameManager.Colors.Red)
                {
                    red = Mathf.Lerp(0f, 1f, progress);
                    blue = 0;
                    green = Mathf.Lerp(1f, 0f, progress);
                    mesh.material.color = new Color(red, green, blue);
                    light.color = new Color(red, green, blue);
                }
                else if (swap && GameManager.Instance.sphereColor[numIntList] == GameManager.Colors.Green && colorToChange == GameManager.Colors.Blue)
                {
                    red = 0;
                    blue = Mathf.Lerp(0f, 1f, progress);
                    green = Mathf.Lerp(1f, 0f, progress);
                    mesh.material.color = new Color(red, green, blue);
                    light.color = new Color(red, green, blue);
                }
                else if (swap && GameManager.Instance.sphereColor[numIntList] == GameManager.Colors.Blue && colorToChange == GameManager.Colors.Green)
                {
                    red = 0;
                    blue = Mathf.Lerp(1f, 0f, progress);
                    green = Mathf.Lerp(0f, 1f, progress);
                    mesh.material.color = new Color(red, green, blue);
                    light.color = new Color(red, green, blue);
                }
                else if (swap && GameManager.Instance.sphereColor[numIntList] == GameManager.Colors.Red && colorToChange == GameManager.Colors.Green)
                {
                    red = Mathf.Lerp(1f, 0f, progress);
                    blue = 0;
                    green = Mathf.Lerp(0f, 1f, progress);
                    mesh.material.color = new Color(red, green, blue);
                    light.color = new Color(red, green, blue);
                }

                progress += Time.fixedDeltaTime * speed;
            }

            if (swap && colorToChange == GameManager.Colors.Blue)
            {
                GameManager.Instance.sphereColor[numIntList] = GameManager.Colors.Blue;
            }
            else if (swap && colorToChange == GameManager.Colors.Red)
            {
                GameManager.Instance.sphereColor[numIntList] = GameManager.Colors.Red;
            }
            else if (swap && colorToChange == GameManager.Colors.Green)
            {
                GameManager.Instance.sphereColor[numIntList] = GameManager.Colors.Green;
            }

        }
    }

}
