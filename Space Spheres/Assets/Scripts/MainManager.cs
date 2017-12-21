using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    // Serializable fields
    // Ground
    [SerializeField] private Transform groundPrefab;
    [SerializeField] private Transform groundContainer;
    // Sphere
    [SerializeField] private Transform spherePrefab;
    [SerializeField] private Transform sphereContainer;
    [SerializeField] private Material[] sphereMaterials;
    // Color
    [SerializeField] private Transform colorContainer;
    [SerializeField] private Material[] colorMaterials;

    // Path grid
    private Vector2[,] pathGrid;
    // Colors, ground
    private List<Transform> colorList;
    private List<Vector2> colorPosOnGrid;
    private float localSizeGroundCoef = 1.1f;
    private Vector2 stepBetweenGround;
    private float heightGroundPrefab = 0.1f;
    // Spheres
    private Transform player;
    private MoveSide playerMoveSide;
    private Vector2 playerPosOnGrid;
    private List<Transform> sphereList;
    private List<Vector2> spherePosOnGrid;
    private List<Vector2> sphereMoveSide;

    // enums
    private enum ObjForRandom
    {
        Player,
        Sphere,
        Color
    }

    private enum MoveSide
    {
        Up,
        Down,
        Left,
        Right
    }

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
            density: new Vector2(7, 13), // Ground resolution (non-paired)
            nSphere: 2,                  // Number of sphere (max 4)
            nColors: 2);                 // Number of color (max 4)


    }

    private void CreateLevel(Vector2 density, int nSphere, int nColors)
    {
        CreateGround(density);
        CreateSpheres(nSphere, density);
        CreateColors(nColors, density);

        CalculateMoving(sphereList[0], spherePosOnGrid[0], sphereMoveSide[0]);

    }

    private void CreateGround(Vector2 density)
    {
        float[] xRange = new float[2] { -5.2f, 5.2f };
        float[] yRange = new float[2] { -9.5f, 9.5f };

        stepBetweenGround = new Vector2
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
                Transform groundGO = Instantiate(groundPrefab, pathGrid[i, j], Quaternion.identity, groundContainer);
                groundGO.position = new Vector3(pathGrid[i, j].x, 0, pathGrid[i, j].y);
                groundGO.localScale = new Vector3(stepBetweenGround.x / localSizeGroundCoef, heightGroundPrefab, stepBetweenGround.y / localSizeGroundCoef);
            }
        }
    }

    private void CreateSpheres(int nSphere, Vector2 density)
    {
        sphereList = new List<Transform>();
        spherePosOnGrid = new List<Vector2>();

        for (int i = 0; i < nSphere; i++)
        {
            if (i == 0)
            {
                playerPosOnGrid = RandomPosition(density, ObjForRandom.Player);
                player = Instantiate(spherePrefab, sphereContainer);
                player.position = new Vector3(pathGrid[
                    (int)(playerPosOnGrid.x), (int)(playerPosOnGrid.y)].x,
                    player.localScale.y / 2,
                    pathGrid[(int)(playerPosOnGrid.x), (int)(playerPosOnGrid.y)].y);
                MeshRenderer meshRendererPlayer = player.GetComponent<MeshRenderer>();
                meshRendererPlayer.material = sphereMaterials[i];
                playerMoveSide = MoveSide.Up;
            }
            else
            {
                Transform sphere = Instantiate(spherePrefab, sphereContainer);
                Vector2 randomPos = RandomPosition(density, ObjForRandom.Sphere);
                float sphereHeightAboveGround = sphere.localScale.y / 2 * -1;
                sphere.position = new Vector3(pathGrid[(int)(randomPos.x), (int)(randomPos.y)].x, sphereHeightAboveGround, pathGrid[(int)(randomPos.x), (int)(randomPos.y)].y);
                MeshRenderer meshRendererSphere = sphere.GetComponent<MeshRenderer>();
                meshRendererSphere.material = sphereMaterials[i];
                sphereList.Add(sphere);
                spherePosOnGrid.Add(randomPos);
                Random.Range(0, 5);
                sphereMoveSide.Add();
            }
        }
    }

    private MoveSide GetRandomMoveSide()
    {
        //MoveSide set;

        //switch (Random.Range(0, 5))
        //{
        //    case 0:
        //        set = MoveSide.Up;
        //        break;
        //    case 1:
        //        set = MoveSide.Right;
        //        break;
        //    case 2:
        //        set MoveSide.Down;
        //        break;
        //    case 3:
        //        set MoveSide.Left;
        //        break;
        //}

        //return null;
    }

    private void CreateColors(int nColors, Vector2 density)
    {
        colorList = new List<Transform>();
        colorPosOnGrid = new List<Vector2>();

        for (int i = 0; i < nColors; i++)
        {
            Transform color = Instantiate(groundPrefab, sphereContainer);
            Vector2 randomPos = RandomPosition(density, ObjForRandom.Color);
            color.position = new Vector3(pathGrid[(int)(randomPos.x), (int)(randomPos.y)].x, 0f, pathGrid[(int)(randomPos.x), (int)(randomPos.y)].y);
            MeshRenderer meshRendererColor = color.GetComponent<MeshRenderer>();
            meshRendererColor.material = colorMaterials[i];
            color.gameObject.AddComponent<Collider>();
            color.localScale = new Vector3(stepBetweenGround.x / localSizeGroundCoef, heightGroundPrefab, stepBetweenGround.y / localSizeGroundCoef);
            colorList.Add(color);
            colorPosOnGrid.Add(randomPos);
        }
    }

    private Vector2 RandomPosition(Vector2 density, ObjForRandom obj)
    {
        bool readyCalculate = true;
        bool nextIteration = true;
        int x = 0, y = 0, rndX = 0, rndY = 0;
        Vector2 newPosition;

        while(readyCalculate)
        {
            nextIteration = true;
            
            if (obj == ObjForRandom.Player)
            {
                rndX = Random.Range((int)0, ((int)density.x - 1) / 2) * 2;
                rndY = Random.Range((int)0, ((int)density.y - 1) / 2) * 2;
            }
            else if (obj == ObjForRandom.Sphere)
            {
                rndX = Random.Range((int)0, ((int)density.x - 1) / 2) * 2 + 1;
                rndY = Random.Range((int)0, ((int)density.y - 1) / 2) * 2 + 1;
                newPosition = new Vector2(rndY, rndX);

                for (int i = 0; i < spherePosOnGrid.Count; i++)
                {
                    if (newPosition == spherePosOnGrid[i])
                    {
                        nextIteration = false;
                    }
                }
            }
            else if (obj == ObjForRandom.Color)
            {
                rndX = Random.Range((int)0, ((int)density.x - 1) / 2) * 2;
                rndY = Random.Range((int)0, ((int)density.y - 1) / 2) * 2;
                newPosition = new Vector2(rndY, rndX);

                for (int i = 0; i < colorPosOnGrid.Count; i++)
                {
                    if (newPosition == colorPosOnGrid[i] && newPosition == playerPosOnGrid)
                    {
                        nextIteration = false;
                    }
                }
            }

            if (nextIteration)
            {
                x = rndY;
                y = rndX;
                readyCalculate = false;
            }
        }
        
        return new Vector2(x, y);
    }

    private void CalculateMoving(Transform currentSphere, Vector2 currentSpherePosOnGrid, Vector2 currentSphereMoveSide)
    {
        // Calculate for spheres: sphereList, spherePosOnGrid
        int mainIdx = sphereList.FindIndex(v => v == currentSphere);

        for (int i = 0; i < spherePosOnGrid.Count; i++)
        {
            if (i == mainIdx)
                continue;


        }

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

