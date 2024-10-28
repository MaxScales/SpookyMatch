using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{

    private EndGameManager endGameManager;

    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;

    //reference to other dot
    public GameObject otherDot;

    //position of dot for debug

    public int column;
    public int row;
    public int previousColumn;
    public int previousRow;
    public int targetX;
    public int targetY;
    public bool isMatched = false;


    //refernce to board
    private Board board;

    //
    private FindMatches findMatches;

    //angle of swipe/mouse click
    public float swipeAngle = 0;
    public float swipeResist = 1f;
    // Start is called before the first frame update
    public bool isColorBomb;
    public bool isColumnBomb;
    public bool isRowBomb;
    public bool isAdjacentBomb;
    public GameObject rowArrow;
    public GameObject colArrow;
    public GameObject colorBomb;
    public GameObject adjacentMarker;

    void Start()
    {
        isColumnBomb = false;
        isRowBomb = false;
        isAdjacentBomb= false;
        isColorBomb= false;

        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
        endGameManager = FindObjectOfType<EndGameManager>();
        
    }

    //testing bombs debug method
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isAdjacentBomb = true;
            GameObject arrow = Instantiate(adjacentMarker, transform.position, Quaternion.identity);
            arrow.transform.parent = this.transform;

        }
    }

    // Update is called once per frame
    void Update()
    {
        //for debugging matches
        /*
        if (isMatched)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(1f, 1f, 1f, 0.2f);
        }
        */
        targetX = column;
        targetY = row;
        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            //Move Towards the target
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();


        }
        else
        {
            //Directly set the position
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;

        }
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            //Move Towards the target
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();

        }
        else
        {
            //Directly set the position
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;

        }
    }

    public IEnumerator CheckMoveCo()
    {
        if(isColumnBomb)
        {
            //this piece is bomb and other piece is color to destroy
            findMatches.MatchPiecesOfColor(otherDot.tag);
            isMatched= true;
        } else if (otherDot.GetComponent<Dot>().isColorBomb)
        {
            //other piece is bomb and this piece is color to destroy
            findMatches.MatchPiecesOfColor(this.gameObject.tag);
            otherDot.GetComponent<Dot>().isMatched = true;
        }
        yield return new WaitForSeconds(.5f);
        if (otherDot != null)
        {
            if (!isMatched && !otherDot.GetComponent<Dot>().isMatched)
            {
                otherDot.GetComponent<Dot>().row = row;
                otherDot.GetComponent<Dot>().column = column;
                row = previousRow;
                column = previousColumn;
                yield return new WaitForSeconds(.5f);
                board.currentDot = null;
                board.currentState = GameState.move;
            }
            else
            {
                if(endGameManager != null)
                {
                    endGameManager.DecreaseCounterMove();
                }
                board.DestroyMatches();

            }

            //otherDot= null;
        }
    }

    private void OnMouseDown()
    {
        if (board.currentState == GameState.move)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        }
    }

    private void OnMouseUp()
    {
        if (board.currentState == GameState.move)
        {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }

    }

    //calculate swipe angle in Degs
    void CalculateAngle()
    {
        //ensure only calculate swipe if there is a swipe
        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {

            board.currentState = GameState.wait;
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            MovePieces();
            
            board.currentDot = this;

        }
        else
        {
            board.currentState = GameState.move;


        }


    }

    void MovePiecesActual(Vector2 direction)
    {
        otherDot = board.allDots[column + (int)direction.x, row + (int)direction.y];
        previousColumn = column;
        previousRow = row;
        otherDot.GetComponent<Dot>().column += -1 * (int)direction.x;
        otherDot.GetComponent<Dot>().row += -1 * (int)direction.y;
        column += (int)direction.x;
        row += (int)direction.y;
        StartCoroutine(CheckMoveCo());
    }

    void MovePieces()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1)
        {
            //Right 
            /*
            otherDot = board.allDots[column + 1, row];
            previousColumn = column;
            previousRow = row;
            otherDot.GetComponent<Dot>().column -= 1;
            column += 1;
            StartCoroutine(CheckMoveCo());
            */
            MovePiecesActual(Vector2.right);
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
        {
            //UP
            MovePiecesActual(Vector2.up);
        }
        else if ((swipeAngle > 135 || swipeAngle <= -1350) && column > 0)
        {
            //Left
            MovePiecesActual(Vector2.left);
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            //down
            MovePiecesActual(Vector2.down);
        } 
            board.currentState = GameState.move;
        

        
    }



    void FindMatches()
    {
        if (column > 0 && column < board.width - 1)
        {
            GameObject leftDot1 = board.allDots[column - 1, row];
            GameObject rightDot1 = board.allDots[column + 1, row];

            //protect against null
            if (leftDot1 != null && rightDot1 != null)
            {
                if (leftDot1.tag == this.gameObject.tag && rightDot1.tag == this.gameObject.tag)
                {
                    leftDot1.GetComponent<Dot>().isMatched = true;
                    rightDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
        }
        if (row > 0 && row < board.height - 1)
        {
            GameObject upDot1 = board.allDots[column, row + 1];
            GameObject downDot1 = board.allDots[column, row - 1];
            if (upDot1 != null && downDot1 != null)
            {
                if (upDot1.tag == this.gameObject.tag && downDot1.tag == this.gameObject.tag)
                {
                    upDot1.GetComponent<Dot>().isMatched = true;
                    downDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
        }
    }

    public void MakeRowBomb()
    {


        isRowBomb = true;
        GameObject arrow = Instantiate(rowArrow, transform.position, Quaternion.identity);
        arrow.transform.parent = this.transform;


    }

    public void MakeColumnBomb()
    {


        isColumnBomb = true;
        GameObject arrow = Instantiate(colArrow, transform.position, Quaternion.identity);
        arrow.transform.parent = this.transform;


    }

    public void MakeColorBomb()
    {
        isColorBomb = true;
        GameObject color = Instantiate(colorBomb, transform.position, Quaternion.identity);
        color.transform.parent = this.transform;
    }

    public void MakeAdjacentBomb()
    {
        isAdjacentBomb = true;
        GameObject marker = Instantiate(adjacentMarker, transform.position, Quaternion.identity);
        marker.transform.parent = this.transform;
    }

}

