using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatches : MonoBehaviour
{
    // Start is called before the first frame update
    private Board board;
    public List<GameObject> currentMatches;
    void Start()
    {
        board = FindObjectOfType<Board>();
        currentMatches = new List<GameObject>();
    }
    public void FindAllMatches()
    {
        for(int i = 0; i < board.width; i++)
        {
            for(int j=0;j< board.height; j++) {
                GameObject currentDot = board.allDots[i, j];
                if (currentDot != null)
                {
                    

                    if (i>0 && i<board.width-1)
                    {
                        GameObject leftDot = board.allDots[i-1, j];
                        GameObject rightDot = board.allDots[i +1, j];
                        
                        if (leftDot != null && rightDot != null)
                        {
                            if(leftDot.tag == currentDot.tag &&rightDot.tag == currentDot.tag)
                            {
                                BombRowOrColumn(currentDot, leftDot, rightDot);
                                leftDot.GetComponent<Dot>().isMatches = true;
                                rightDot.GetComponent<Dot>().isMatches = true;
                                currentDot.GetComponent<Dot>().isMatches = true;
                                CreateBombRow(j);
                            }
                        }
                    }
                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject upDot = board.allDots[i, j+1];
                        GameObject downDot = board.allDots[i, j-1];

                        if (upDot != null && downDot != null)
                        {
                            if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag)
                            {
                                BombRowOrColumn(currentDot, upDot, downDot);
                                upDot.GetComponent<Dot>().isMatches = true;
                                downDot.GetComponent<Dot>().isMatches = true;
                                currentDot.GetComponent<Dot>().isMatches = true;
                            }
                        }
                    }
                }
               
            }
        }
    }

    public void BombRowOrColumn(GameObject currentDot, GameObject leftDot, GameObject rightDot)
    {
        int rowCurrentDot = currentDot.GetComponent<Dot>().row;
        int columnCurrentDot = currentDot.GetComponent<Dot>().column;
        int rowLeftDot = leftDot.GetComponent<Dot>().row;
        int columnLeftDot = leftDot.GetComponent<Dot>().column;
        int rowRightDot = rightDot.GetComponent<Dot>().row;
        int columnRightDot = rightDot.GetComponent<Dot>().column;
       
        if (currentDot.GetComponent<Dot>().typeBomb.ToString()==(Type.BombColumn.ToString()))
        {
            
            for (int i = 0; i < board.height; i++)
            {
                if(board.allDots[columnCurrentDot, i].GetComponent<Dot>() != null)
                {
                    board.allDots[columnCurrentDot,i].GetComponent<Dot>().isMatches = true;

                }
            }
        }
        if (currentDot.GetComponent<Dot>().typeBomb.ToString() == (Type.BombRow.ToString()))
        {
            

            for (int j = 0; j < board.width; j++)
            {
                if (board.allDots[j, rowCurrentDot].GetComponent<Dot>()!=null)
                {
                    board.allDots[j,rowCurrentDot].GetComponent<Dot>().isMatches = true;

                }
            }
        }
        if (leftDot.GetComponent<Dot>().typeBomb.ToString() == (Type.BombColumn.ToString()))
        {
           

            for (int i = 0; i < board.height; i++)
            {
                if (board.allDots[columnLeftDot, i].GetComponent<Dot>()!=null)
                {
                    board.allDots[columnLeftDot, i].GetComponent<Dot>().isMatches = true;

                }
            }
        }
        if (leftDot.GetComponent<Dot>().typeBomb.ToString() == (Type.BombRow.ToString()))
        {
            

            for (int j = 0; j < board.width; j++)
            {
                if (board.allDots[j, rowLeftDot].GetComponent<Dot>()!=null)
                {
                    board.allDots[j, rowLeftDot].GetComponent<Dot>().isMatches = true;

                }
            }
        }
        if (rightDot.GetComponent<Dot>().typeBomb.ToString() == (Type.BombColumn.ToString()))
        {
            

            for (int i = 0; i < board.height; i++)
            {
                if (board.allDots[columnRightDot, i].GetComponent<Dot>()!=null)
                {
                    board.allDots[columnRightDot, i].GetComponent<Dot>().isMatches = true;

                }
            }
        }
        if (rightDot.GetComponent<Dot>().typeBomb.ToString() == (Type.BombRow.ToString()))
        {
          

            for (int j = 0; j < board.width; j++)
            {
                if (board.allDots[j, rowRightDot].GetComponent<Dot>()!=null)
                {
                    board.allDots[j, rowRightDot].GetComponent<Dot>().isMatches = true;
                }
            }
        }
    }
    public void CreateBombRow(int j)
    {

        for(int i = 0; i < board.width; i++)
        {
            if (board.allDots[i, j].GetComponent<Dot>() != null)
            {
                if (board.allDots[i, j] != null && board.allDots[i, j].GetComponent<Dot>().isMatches)
                {
                   if(!currentMatches.Contains(board.allDots[i, j]))
                    {
                        currentMatches.Add(board.allDots[i, j]);

                    }
                }
            }
        }
    }
}
