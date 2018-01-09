using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    private GameManager.Colors currentColor;

    private void Start()
    {
        Color color = GetComponent<MeshRenderer>().material.color;

        if (color.r >= 0.9)
        {
            currentColor = GameManager.Colors.Red;
        }
        else if (color.b >= 0.9)
        {
            currentColor = GameManager.Colors.Blue;
        }

        //GameManager.Instance.ColorGo += ColorGo;
    }

    //private void ColorGo(Transform color, bool readyToDestroy)
    //{
    //    Debug.Log("ColorGo " + readyToDestroy);
    //    if (readyToDestroy && color == transform)
    //    {
    //        readyToDestroy = true;
    //    }
    //    else
    //    {
    //        readyToDestroy = false;
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        
        if (GameManager.Instance.playerColor == currentColor)
        {
            Destroy(gameObject);
        }
    }

    //private bool CheckColor(string color)
    //{
    //    // Bad checking

    //    GameManager.Colors sphereColor = GameManager.Colors.Blue;

    //    if (color == "SphereBlue")
    //    {
    //        sphereColor = GameManager.Colors.Blue;
    //    }
    //    else if (color == "SphereRed")
    //    {
    //        sphereColor = GameManager.Colors.Red;
    //    }
    //    else if (color == "SphereGreen")
    //    {
    //        sphereColor = GameManager.Colors.Green;
    //    }
    //    else if (color == "SphereOrange")
    //    {
    //        sphereColor = GameManager.Colors.Orange;
    //    }

    //    if (sphereColor == currentColor)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
}
