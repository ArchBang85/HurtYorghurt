﻿using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random; // Need to specify because there's an overlap between System and UnityEngine

//namespace Completed;

public class BoardManager : MonoBehaviour {
        
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

            public Count (int min, int max)
            {
                minimum = min;
                maximum = max;
            }
    }

    public int columns = 8; 
    public int rows = 8;
    public Count wallCount = new Count(5, 9); // minimum and maximum of walls
    public Count foodCount = new Count(1, 3);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;
    
    private Transform boardHolder;
    private List <Vector3> gridPositions = new List<Vector3> ();

    void InitialiseList()
    {

        // Clear list
        gridPositions.Clear ();

        // Loop through cols
        for (int x = 1; x < columns - 1; x++)
        {
            // Loop through rows
            for (int y = 1; y < rows - 1; y++)
            {
                // At each index add a new Vector3 
                gridPositions.Add(new Vector3(x, y, 0f));
            }

        }

    }

    void BoardSetup ()
    {
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                
                }
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }

    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;

    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);


        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitialiseList();

        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        int enemyCount = (int)Mathf.Log(level, 2f); // logarithmic. 3 enemies, level 8, etc.
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);

    }

}
