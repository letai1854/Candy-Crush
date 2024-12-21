
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("infor board")]
    public int width;
    public int height;


    private FindMatches findMatches;
    public GameObject[] dots;
    public GameObject[] dotSeven;
    public GameObject[,] allBackground;
    public GameObject[,] allDots;

    [Header("info object")]
    public GameObject backgroundOdd;
    public GameObject backgroundEven;
    public int offset;
    
    void Start()
    {
        allBackground = new GameObject[width, height];
        allDots = new GameObject[width, height];
        findMatches = FindObjectOfType<FindMatches>();
        SetUp();
    }
    GameObject dotTemp;
    public GameObject explosion;

    public int RandomRateBlueRow;
    public int RandomRateBlueColumn;

    public int RandomRateRedRow;
    public int RandomRateRedColumn;

    public int RandomRateYellowRow;
    public int RandomRateYellowColumn;

    public int RandomRateGreenRow;
    public int RandomRateGreenColumn;

    public int RandomRatePurpleRow;
    public int RandomRatePurpleColumn;

    public int sevenBomb;
    public bool CheckMove=true;

    //public GameObject currentDot;
   
    void Update()
    {

    }

    public void SetUp()
    {

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 posTemp = new Vector2(i, j + offset);
                Vector2 posTempBG = new Vector2(i, j);
                if ((i + j) % 2 == 0)
                {

                    GameObject background = Instantiate(backgroundOdd, posTempBG, Quaternion.identity) as GameObject;
                    background.transform.parent = this.transform;
                    background.name = "bg";
                }
                else if ((i + j) % 2 != 0)
                {
                    GameObject background = Instantiate(backgroundEven, posTempBG, Quaternion.identity) as GameObject;
                    background.transform.parent = this.transform;
                    background.name = "bg";
                }
                int dotToUse = Random.Range(0, dots.Length);
                Vector2 posTemp1 = new Vector2(i, j + 10);
                dotToUse = RandomDot(i, j, dotToUse);

                GameObject dot = Instantiate(dotTemp, posTemp1, Quaternion.identity) as GameObject;
                dot.GetComponent<Dot>().row = j;
                dot.GetComponent<Dot>().column = i;
                dot.transform.parent = this.transform;
                dot.name = "[" + i + ":" + j + "]";
                allDots[i, j] = dot;
            }
        }
    }

    private int RandomDot(int i, int j, int dotToUse)
    {
        int index = 0;
        while (CreateDots(i, j, dots[dotToUse]) && index < 100)
        {
            dotToUse = Random.Range(0, dots.Length);
            index++;
        }

        return dotToUse;
    }

    public bool CreateDots(int i, int j, GameObject dot)
    {
        if (j > 1 )
        {
            if (allDots[i, j - 1].tag == dot.tag && allDots[i, j - 2].tag == dot.tag)
            {
                return true;
            }
        }
        if (i > 1 )
        {
            if (allDots[i - 1, j].tag == dot.tag && allDots[i - 2, j].tag == dot.tag)
            {
                return true;
            }



        }
        dotTemp = CheckDot(dot);
        return false;
    }
    public GameObject CheckDot(GameObject dot)
    {
        int rateBomb = Random.Range(0, 100);
        if(dot.GetComponent<Dot>().typeBomb ==  Type.BombRow)
        {
            if(dot.tag == (TypeTag.Blue).ToString())
            {
                if (rateBomb<RandomRateBlueRow)
                {
                    return dot;
                }
                return dots[2];
            }
            else if (dot.tag == (TypeTag.Green).ToString())
            {
                if (rateBomb < RandomRateGreenRow)
                {
                    return dot;
                }
                return dots[5];
            }
            else if (dot.tag == (TypeTag.Red).ToString())
            {
                if (rateBomb < RandomRateRedRow)
                {
                    return dot;
                }
                return dots[11];
            }
            else if (dot.tag == (TypeTag.Yellow).ToString())
            {
                if (rateBomb < RandomRateYellowRow)
                {
                    return dot;
                }
                return dots[14];
            }
            else if (dot.tag == (TypeTag.Purple).ToString())
            {
                if (rateBomb < RandomRatePurpleRow)
                {
                    return dot;
                }
                return dots[8];
            } 
        }
        else  if (dot.GetComponent<Dot>().typeBomb == Type.BombColumn )
        {
            if (dot.tag == (TypeTag.Blue).ToString())
            {
                if (rateBomb < RandomRateBlueColumn)
                {
                    return dot;
                }
                return dots[2];
            }
            else if (dot.tag == (TypeTag.Green).ToString())
            {
                if (rateBomb < RandomRateGreenColumn)
                {
                    return dot;
                }
                return dots[5];
            }
            else if (dot.tag == (TypeTag.Red).ToString())
            {
                if (rateBomb < RandomRateRedColumn)
                {
                    return dot;
                }
                return dots[11];
            }
            else if (dot.tag == (TypeTag.Yellow).ToString())
            {
                if (rateBomb < RandomRateYellowColumn)
                {
                    return dot;
                }
                return dots[14];
            }
            else if (dot.tag == (TypeTag.Purple).ToString())
            {
                if (rateBomb < RandomRatePurpleColumn)
                {
                    return dot;
                }
                return dots[8];
            }
        }
      
        return dot;
    }
    public void DestroyAllIsMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (allDots[i, j].GetComponent<Dot>().isMatches)
                    {
                       
                        findMatches.currentMatches.Clear();
                        Destroy(allDots[i, j]);
                        StartCoroutine(ShowExplosion(i, j));
                        allDots[i, j] = null;
                    }
                }
            }
        }
        IncreaseDot();
    }
    IEnumerator ShowExplosion(int i, int j)
    {
        yield return new WaitForSeconds(0.01f);
        Vector2 temp = new Vector2(i, j);
        GameObject explosionShow = Instantiate(explosion, temp, Quaternion.identity);
        yield return new WaitForSeconds(0.6f);
        Destroy(explosionShow );
        foreach (GameObject gameObject in findMatches.currentMatches)
        {
            Debug.Log(gameObject.GetComponent<Dot>().name.ToLower());
        }
    }
    void IncreaseDot()
    {
        
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                    {
                        allDots[i, j].GetComponent<Dot>().row -= nullCount;
                        allDots[i, j] = null;
                    }

                
                
            }
            nullCount = 0;
        }
        StartCoroutine(FillBoard());
    }
    IEnumerator FillBoard()
    {
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < width; i++)
            {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    int dotToUse = Random.Range(0, dots.Length);
                    Vector2 posTemp1 = new Vector2(i, j + 10);
                    dotToUse = RandomDot(i, j, dotToUse);

                    GameObject dot = Instantiate(dotTemp, posTemp1, Quaternion.identity) as GameObject;
                    dot.GetComponent<Dot>().row = j;
                    dot.GetComponent<Dot>().column = i;
                    dot.transform.parent = this.transform;
                    dot.name = "[" + i + ":" + j + "]";
                    allDots[i, j] = dot;
                }
                
            }
       
        }
        yield return new WaitForSeconds(0.5f);
        DestroyReturnDot();

    }
    public void DestroyReturnDot()
    {
        while (MatchesOnBoard())
        {
            DestroyAllIsMatches();
        }
        CheckMove = true;
    }
    public bool MatchesOnBoard()
    {
        for(int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (allDots[i, j].GetComponent<Dot>().isMatches)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
