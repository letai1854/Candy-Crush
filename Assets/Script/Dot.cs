using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public enum Type
{
    BombColumn,
    BombRow,
    BombNormal,
    BombWide
}
public enum TypeTag
{
    Blue,
    Green,
    Purple,
    Red,
    Yellow,
    seven
}
public class Dot : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
{

    public  Type typeBomb;
    public bool isMatches=false;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    public float swipAngle=0;
    public float swipeResist = 1f;
    public int column;
    public int row;

    public int targetX;
    public int targetY;

    private Board board;
    Vector2 tempPosition;
    private FindMatches findMatches;
    public int previousRow;
    public int previousColumn;
    GameObject otherDot;
    private bool isCheckingForMatches = false;

    private SpriteRenderer spriteRenderer;
    void Start()
    {
        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
       
        targetX = column;
        targetY = row;
        if(isMatches)
        {
            
                spriteRenderer.color = Color.white;

        }

        if (math.abs(targetX - transform.position.x) > 0.1f)
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.6f);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }

        }
        else
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            if (!isCheckingForMatches)
            {

                StartCoroutine(CheckMatchesAfterMove());
            }

        }
        if (math.abs(targetY - transform.position.y) > 0.1f)
        {
            tempPosition = new Vector2(transform.position.x, targetY);
         
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.6f);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }
        }
        else
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            if (!isCheckingForMatches)
            {
                StartCoroutine(CheckMatchesAfterMove());
            }
        }
            

    }
    IEnumerator CheckMatchesAfterMove()
    {
        isCheckingForMatches = true;
        yield return new WaitForSeconds(0.5f); // Add slight delay for movement to complete
        findMatches.FindAllMatches();
        isCheckingForMatches = false;

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (board.CheckMove)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (board.CheckMove)
        {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            Caculate();
        }


    }

    private void Caculate()
    {
        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist ||
            Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            swipAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            CheckMove();
        }


    }
    public void CheckMove()
    {
        if(swipAngle>-45 && swipAngle<=45 && column< board.width-1)
        {
            otherDot = board.allDots[ column+1,row];
            previousColumn = column;
            previousRow = row;
            otherDot.GetComponent<Dot>().column -= 1;
            column = column + 1;
        }
        if(swipAngle>45 && swipAngle<=135 && row< board.height - 1)
        {
            otherDot = board.allDots[column,row + 1 ];
            previousColumn = column;
            previousRow = row;
            otherDot.GetComponent<Dot>().row -= 1;
            row = row + 1;
        }
        if( (swipAngle>135 || swipAngle <=-135) && column > 0)
        {
            otherDot = board.allDots[column - 1,row   ];
            previousColumn = column;
            previousRow = row;
            otherDot.GetComponent<Dot>().column += 1;
            column = column - 1;
        }
        if(swipAngle > -135 && swipAngle <= -45 && row >0)
        {
            otherDot = board.allDots[column, row - 1];
            previousColumn = column;
            previousRow = row;
            otherDot.GetComponent<Dot>().row += 1;
            row = row - 1;
        }
       
        board.CheckMove = false;
        StartCoroutine(ReturnPosition());
    }
    IEnumerator ReturnPosition()
    {
        yield return new WaitForSeconds(0.5f);
        CheckMoveCo();
    }
    public void CheckMoveCo()
    {
        if (otherDot != null)
        {
            if (!otherDot.GetComponent<Dot>().isMatches && !isMatches)
            {
                otherDot.GetComponent<Dot>().column = column;
                otherDot.GetComponent<Dot>().row = row;
                column = previousColumn;
                row = previousRow;
                board.CheckMove = true;
                
            }
            else
            {
                //board.currentDot.GetComponent<Dot>().row = row;
                //board.currentDot.GetComponent<Dot>().column = column;
                board.DestroyAllIsMatches();
                
            }
        }
        else
        {
            board.CheckMove = true;
        }
        
    }
}
