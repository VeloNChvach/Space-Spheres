using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void Start()
    {
        MainManager.Instance.PlayerGo += PlayerGo;
    }

    private void PlayerGo(MainManager.MoveSide moveSide, bool swap)
    {
        Debug.Log(moveSide + " " + swap);

    }

}
