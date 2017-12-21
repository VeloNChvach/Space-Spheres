using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : Singleton<MainManager>
{
    // Serializable fields
    // Ground
    [SerializeField] private Transform groundPrefab;
    [SerializeField] private Transform groundContainer;
    // Sphere
    [SerializeField] private Transform spherePrefab;
    [SerializeField] private Transform sphereContainer;
    // Color
    //[SerializeField] private Transform colorPrefab;
    [SerializeField] private Transform colorContainer;

    // All for Path grid
    private Vector2[,] pathGrid;
    // Ground list
    private List<Transform> groundList;
    private List<Transform> colorList;
    private List<Vector2> colorPosOnGrid;
    // Spheres
    private Transform player;
    private Vector2 playerPosOnGrid;
    private List<Transform> sphereList;
    private List<Vector2> spherePosOnGrid;


    private void Start()
    {
        // Actions
        InputHandler.Instance.MoveUp += MoveUp;
        InputHandler.Instance.MoveDown += MoveDown;
        InputHandler.Instance.MoveLeft += MoveLeft;
        InputHandler.Instance.MoveRight += MoveRight;
        InputHandler.Instance.Tap += Tap;
        // Create level
        CreateLevel(
            density: new Vector2(7, 13), // ground resolution
            nSphere: 5,                  // Number of sphere
            nColors: 2);                 // Number of color
        

    }

    private void CreateLevel(Vector2 density, int nSphere, int nColors)
    {
        CreateGround(density);
        CreateSpheres(nSphere, density);
        CreateColors(nColors, density);
    }

    private void CreateGround(Vector2 density)
    {
        float[] xRange = new float[2] { -5.2f, 5.2f };
        float[] yRange = new float[2] { -9.5f, 9.5f };

        Vector2 step = new Vector2
        {
            x = (xRange[1] * 2) / ((float)density.x),
            y = (yRange[1] * 2) / ((float)density.y)
        };

        Vector2 startMatrixPosition = new Vector2
        {
            x = (2 - (density.x - 1) * step.x) / 2 - 1,
            y = (2 - (density.y - 1) * step.y) / 2 - 1
        };

        pathGrid = new Vector2[(int)density.y, (int)density.x];
        groundList = new List<Transform>((int)density.x * (int)density.y);
        float localSizeCoef = 1.1f;
        float heightPrefab = 0.1f;

        for (int i = 0; i < density.y; i++)
        {
            for (int j = 0; j < density.x; j++)
            {
                pathGrid[i, j] = new Vector2(startMatrixPosition.x + step.x * j, startMatrixPosition.y + step.y * i);
                Transform groundGO = Instantiate(groundPrefab, pathGrid[i, j], Quaternion.identity, groundContainer);
                groundGO.position = new Vector3(pathGrid[i, j].x, 0, pathGrid[i, j].y);
                groundGO.localScale = new Vector3(step.x / localSizeCoef, heightPrefab, step.y / localSizeCoef);
                groundList.Add(groundGO);
            }
        }
    }

    private void CreateSpheres(int nSphere, Vector2 density)
    {
        if (nSphere == 1)
        {
            playerPosOnGrid = Vector2.zero;
            player = Instantiate(spherePrefab, sphereContainer);
            player.position = new Vector3(pathGrid[
                (int)(playerPosOnGrid.x), (int)(playerPosOnGrid.y)].x, 
                player.localScale.y / 2, 
                pathGrid[(int)(playerPosOnGrid.x), (int)(playerPosOnGrid.y)].y);
            MeshRenderer meshRendererPlayer = player.GetComponent<MeshRenderer>();
            Material[] materials = meshRendererPlayer.materials;
            meshRendererPlayer.material = materials[0];
        }
        else
        {
            sphereList = new List<Transform>();
            spherePosOnGrid = new List<Vector2>();

            for (int i = 0; i < nSphere; i++)
            {
                
                Debug.Log(RandomPosition(density, "Sphere"));
            }
        }
    }

    private Vector2 RandomPosition(Vector2 density, string what)
    {
        bool readyCalculate = true;
        bool nextIteration = true;
        int x = 0, y = 0, rndX = 0, rndY = 0;
        Vector2 newPosition;

        while(readyCalculate)
        {
            nextIteration = true;
            rndX = Random.Range((int)0, ((int)density.x - 1) / 2) * 2;
            rndY = Random.Range((int)0, ((int)density.y - 1) / 2) * 2;
            newPosition = new Vector2(rndX, rndY);

            if (newPosition != playerPosOnGrid && nextIteration)
            {
                if (what == "Sphere")
                {
                    for (int i = 0; i < spherePosOnGrid.Count; i++)
                    {
                        if (newPosition == spherePosOnGrid[i])
                        {
                            nextIteration = false;
                        }
                    }
                }
                else if (what == "Color")
                {
                    for (int i = 0; i < colorPosOnGrid.Count; i++)
                    {
                        if (newPosition == colorPosOnGrid[i])
                        {
                            nextIteration = false;
                        }
                    }
                }
                
                if (nextIteration)
                {
                    x = rndX;
                    y = rndY;
                    readyCalculate = false;
                }

            }
        }
        



        return new Vector2(x, y);
    }

    private void CreateColors(int nColors, Vector2 density)
    {

    }

    private void MoveUp()
    {
        Debug.Log("MoveUp");
    }

    private void MoveDown()
    {
        Debug.Log("MoveDown");
    }

    private void MoveLeft()
    {
        Debug.Log("MoveLeft");
    }

    private void MoveRight()
    {
        Debug.Log("MoveRight");
    }

    private void Tap()
    {
        Debug.Log("Tap");
    }
}

