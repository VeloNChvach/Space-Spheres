using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    private GameManager.Colors currentColor;

    private void Start()
    {
        Color color = GetComponent<MeshRenderer>().material.color;
        Light colorLight = GetComponentInChildren<Light>();

        if (color.r >= 0.9)
        {
            currentColor = GameManager.Colors.Red;
            colorLight.color = new Color(1f, 0f, 0f);
        }
        else if (color.b >= 0.9)
        {
            currentColor = GameManager.Colors.Blue;
            colorLight.color = new Color(0f, 0f, 1f);
        }
        else if (color.g >= 0.9)
        {
            currentColor = GameManager.Colors.Green;
            colorLight.color = new Color(0f, 1f, 0f);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (GameManager.Instance.playerColor == currentColor)
        {
            Destroy(gameObject);
        }
    }

    
}
