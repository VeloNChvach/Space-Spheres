using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : Singleton<GM>
{
    // Serializable fields
    // Ground
    [SerializeField] private Transform groundPrefab;
    [SerializeField] private Transform groundContainer;
    // Sphere
    [SerializeField] private Transform spherePrefab;
    [SerializeField] private Transform sphereContainer;
    [SerializeField] public Material sphereMaterial;
    // Color
    [SerializeField] private Transform colorContainer;
    [SerializeField] private Transform colorPrefab;
    // Light
    [SerializeField] private Transform lightGlobalPrefab;
    [SerializeField] private Transform lightContainer;


    [HideInInspector] public Vector2 density;

    private Vector2[,] pathGrid;
    private Transform player;
    private List<Transform> groundList;
    private List<Transform> sphereList;

    private enum ObjForRandom
    {
        Player,
        Sphere,
        Color
    }



    private void Awake()
    {
        groundList = new List<Transform>();
    }

    private void Start()
    {
        //CreateLevel();
    }

    private void CreateLevel()
    {
        CreatePathGrid();

    }

    private void CreatePathGrid()
    {
        float koef = 9.5f;
        //float[] xRange = new float[2] { -koef * density.x / density.y, koef * density.x / density.y};
        float[] xRange = new float[2] { -koef, koef };
        float[] yRange = new float[2] { -koef, koef };
        Vector2 density = new Vector2(13, 13);
        float localSizeGroundCoef = 1.1f;
        float heightGroundPrefab = 0.1f;
        

    Vector2 stepBetweenGround = new Vector2
        {
            x = (xRange[1] * 2) / ((float)density.x),
            y = (yRange[1] * 2) / ((float)density.y)
        };

        Vector2 startMatrixPosition = new Vector2
        {
            x = (2 - (density.x - 1) * stepBetweenGround.x) / 2 - 1,
            y = (2 - (density.y - 1) * stepBetweenGround.y) / 2 - 1
        };

        pathGrid = new Vector2[(int)density.y, (int)density.x];

        for (int i = 0; i < density.y; i++)
        {
            for (int j = 0; j < density.x; j++)
            {
                pathGrid[i, j] = new Vector2(startMatrixPosition.x + stepBetweenGround.x * j, startMatrixPosition.y + stepBetweenGround.y * i);
                Transform ground = Instantiate(groundPrefab, pathGrid[i, j], Quaternion.identity, groundContainer);
                ground.position = new Vector3(pathGrid[i, j].x, 0, pathGrid[i, j].y);
                ground.localScale = new Vector3(stepBetweenGround.x / localSizeGroundCoef, heightGroundPrefab, stepBetweenGround.y / localSizeGroundCoef);
                groundList.Add(ground);
            }
        }
    }

    private void CreatePlayer()
    {

        //playerPosOnGrid = RandomPosition(density, ObjForRandom.Player);
        Vector2 playerPosOnGrid = new Vector2(2, 2);
        player = Instantiate(spherePrefab, sphereContainer);
        player.position = new Vector3(pathGrid[
        (int)(playerPosOnGrid.x), (int)(playerPosOnGrid.y)].x,
        player.localScale.y / 2,
        pathGrid[(int)(playerPosOnGrid.x), (int)(playerPosOnGrid.y)].y);
        MeshRenderer meshRendererPlayer = player.GetComponent<MeshRenderer>();
        meshRendererPlayer.material.color = GetColor(0);
        //playerMoveSide = MoveSide.Down;
        //playerColor = Colors.Blue;
        player.gameObject.AddComponent<PlayerController>();
    }

    private void CreateSphere()
    {

    }

    private void CreateColor()
    {

    }


    //private Vector2 RandomPosition(Vector2 density, ObjForRandom obj)
    //{
    //    bool readyCalculate = true;
    //    bool nextIteration = true;
    //    int x = 0, y = 0, rndX = 0, rndY = 0;
    //    Vector2 newPosition;

    //    while (readyCalculate)
    //    {
    //        nextIteration = true;

    //        if (obj == ObjForRandom.Player)
    //        {
    //            rndX = UnityEngine.Random.Range((int)1, ((int)density.x - 1) / 2 - 1) * 2;
    //            rndY = UnityEngine.Random.Range((int)1, ((int)density.y - 1) / 2 - 1) * 2;
    //        }
    //        else if (obj == ObjForRandom.Sphere)
    //        {
    //            rndX = UnityEngine.Random.Range((int)2, ((int)density.x - 1) / 2) * 2 - 1;
    //            rndY = UnityEngine.Random.Range((int)2, ((int)density.y - 1) / 2) * 2 - 1;
    //            newPosition = new Vector2(rndY, rndX);

    //            for (int i = 0; i < sphereList.Count; i++)
    //            {
    //                if (newPosition == sphereList[i].Position)
    //                {
    //                    nextIteration = false;
    //                }
    //            }
    //        }
    //        else if (obj == ObjForRandom.Color)
    //        {
    //            rndX = UnityEngine.Random.Range((int)0, ((int)density.x - 1) / 2) * 2;
    //            rndY = UnityEngine.Random.Range((int)0, ((int)density.y - 1) / 2) * 2;
    //            newPosition = new Vector2(rndY, rndX);

    //            for (int i = 0; i < colorPosOnGrid.Count; i++)
    //            {
    //                if (newPosition == colorPosOnGrid[i] && newPosition == playerPosOnGrid)
    //                {
    //                    nextIteration = false;
    //                }
    //            }
    //        }

    //        if (nextIteration)
    //        {
    //            x = rndY;
    //            y = rndX;
    //            readyCalculate = false;
    //        }
    //    }

    //    return new Vector2(x, y);
    //}

    private Color GetColor(int _color)
    {
        float red = 0f, green = 0f, blue = 0f;

        if (_color == 0)
        {
            red = 0f;
            green = 0f;
            blue = 1f;
        }
        else if (_color == 1)
        {
            red = 1f;
            green = 0f;
            blue = 0f;
        }
        else if (_color == 2)
        {
            red = 0f;
            green = 1f;
            blue = 0f;
        }

        return new Color(red, green, blue);
    }
}
