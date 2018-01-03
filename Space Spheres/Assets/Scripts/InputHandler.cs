using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : Singleton<InputHandler>
{
    public event Action SwipeUp;
    public event Action SwipeDown;
    public event Action SwipeLeft;
    public event Action SwipeRight;
    public event Action Tap;

    private Vector2 startTouchPosition = Vector2.zero;

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (startTouchPosition == Vector2.zero)
            {
                startTouchPosition = (Vector2)Input.mousePosition;
            }
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            if (Vector2.Distance((Vector2)Input.mousePosition, startTouchPosition) > 50)
            {
                MovePlayer((Vector2)Input.mousePosition - startTouchPosition);
            }
            else
            {
                Tap.Invoke();
            }

            startTouchPosition = Vector2.zero;
        }

    }

    private void MovePlayer(Vector2 deltaDrag)
    {
        if (Mathf.Abs(deltaDrag.x) > Mathf.Abs(deltaDrag.y))
        {
            //// Horizontal
            if (deltaDrag.x > 0)
            {
                // Right
                SwipeRight.Invoke();
            }
            else
            {
                // Left
                SwipeLeft.Invoke();
            }
        }
        else
        {
            //// Vertical
            if (deltaDrag.y > 0)
            {
                // Up
                SwipeUp.Invoke();
            }
            else
            {
                // Down
                SwipeDown.Invoke();
            }
        }
    }
}
