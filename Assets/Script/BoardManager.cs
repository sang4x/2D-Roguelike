using System;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;
        public Count(int min, int max)
        {
            min = minimum;
            max = maximum;
        }
    }
    public int column = 8;
    public int row = 8;
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit;
    public GameObject[] FoodTiles;
    public GameObject[] floortiles;
    public GameObject[] walltiles;
    public GameObject[] enermytiles;
    public GameObject[] outerwalltiles;
    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();
    void Initialiselist()
    {
        gridPositions.Clear();
        for (int x = 1; x<column-1; x++)
        {
            for(int y =1; y <row - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }
    void boardsetup()
    {
        boardHolder = new GameObject("Board").transform;
        for (int x = -1; x < column+1; x++)
        {
            for (int y = -1; y < row +1; y++)
            {
                GameObject toInstantiate = floortiles[Random.Range(0, floortiles.Length)];
                if (x == - 1 || x == column || y == -1 || y == row)
                {
                    toInstantiate = outerwalltiles[Random.Range(0,outerwalltiles.Length)];
                }
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
       
    }
    Vector3 RandomPosition()
    {
        int randomindex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomindex];
        gridPositions.RemoveAt(randomindex);
        return randomPosition;
    }
    void LayoutObjectRandom(GameObject[] tileArray, int min, int max)
    {
        int objectcount = Random.Range(min, max + 1);
        for (int i = 0; i < objectcount; i++)
        {
            Vector3 randomposition = RandomPosition();
            GameObject tilechoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tilechoice, randomposition, Quaternion.identity);
        }
    }
    public void setupscene(int level)
    {
        boardsetup();
        Initialiselist();
        LayoutObjectRandom(walltiles, wallCount.minimum,wallCount.maximum);
        LayoutObjectRandom(FoodTiles, foodCount.minimum, foodCount.maximum);
        int enermycount = (int) MathF.Log(level,2f);
        LayoutObjectRandom(enermytiles, enermycount, enermycount);
        Instantiate(exit, new Vector3(column - 1, row - 1, 0f),Quaternion.identity);

    }
} 
