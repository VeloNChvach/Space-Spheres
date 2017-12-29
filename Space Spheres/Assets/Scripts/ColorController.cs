using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    private GameManager.Colors currentColor;

    private void Start()
    {
        string color = GetComponent<MeshRenderer>().material.name;
        if (color == "ColorBlue")
        {
            currentColor = GameManager.Colors.Blue;
        }
        else if (color == "ColorRed")
        {
            currentColor = GameManager.Colors.Red;
        }
        else if (color == "ColorGreen")
        {
            currentColor = GameManager.Colors.Green;
        }
        else if (color == "ColorOrange")
        {
            currentColor = GameManager.Colors.Orange;
        }
        //GameManager.Instance.ColorGo += ColorGo;
    }

    private void ColorGo(Transform color, bool readyToDestroy)
    {
        //Debug.Log("ColorGo " + readyToDestroy);
        //if (readyToDestroy && color == transform)
        //{
        //    readyToDestroy = true;
        //}
        //else
        //{
        //    readyToDestroy = false;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CheckColor(other.GetComponent<MeshRenderer>().material.name))
        {
            Destroy(gameObject);
        }
    }

    private bool CheckColor(string color)
    {
        // Bad chacking
        GameManager.Colors sphereColor = GameManager.Colors.Blue;

        if (color == "SphereBlue")
        {
            sphereColor = GameManager.Colors.Blue;
        }
        else if (color == "SphereRed")
        {
            sphereColor = GameManager.Colors.Red;
        }
        else if (color == "SphereGreen")
        {
            sphereColor = GameManager.Colors.Green;
        }
        else if (color == "SphereOrange")
        {
            sphereColor = GameManager.Colors.Orange;
        }

        if (sphereColor == currentColor)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
