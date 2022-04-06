using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoardConstructor : MonoBehaviour
{
    enum Dificulty { VeryEasy, Easy, NormalDefensive, Normal };
    [SerializeField]
    Dificulty dificulty;

    [SerializeField]
    //this will store the lose condition image
    GameObject imgLoseCondition;
    [SerializeField]
    //this will store the win condition image
    GameObject imgWinCondition;
    //this will be true when the game ends (someone kills the king)
    bool gameFinished = false;

    [SerializeField]
    GameObject squaresParent;

    [SerializeField]
    //this will store the white squares from the board
    GameObject whiteBoardSquare;
    [SerializeField]
    //this will store the black squares from the board
    GameObject blackBoardSquare;

    
    /*this array of game objects will store every square and posititions of them
    This will be used to spawn the squares and it works the following way 
    the y is from bottom to top
    The x is from left to right
    the board is drawn from left to right, and when the 8 pieces are drawn just move up the y axis

    Exemple the [0,7] position is the one below (the A)
    ********
    ********
    ********
    *******A
    
    [3,5] position is the one below (the A)
    ********
    *****A**
    ********
    ********
    ********
    */
    GameObject[,] chesSquares = new GameObject[8, 8];
    
    //this will store the squares that are green, because they are selected, i do this so that i know which squares to paint with the original color again
    struct SquaresSelected
    {
        public ChessPiece pieceSelected;
        public Vector2Int pos;
        public Color oldColor;
    }

    //this list  will store the green squares
    List<SquaresSelected> squaresSelected = new List<SquaresSelected>();

    //this will store every chessPiece of the game
    [HideInInspector]
    public List<ChessPiece> chessPieces = new List<ChessPiece>();
    /*this is going to store the position of the first ai piece, this is done for optimization reasons, we basically with this we dont need to go trough every 
     * piece when we only want ai we just use this number the same is aplicable for the non ai pieces, with this we now when the ai starts so we only go trough the non ai pieces*/
    int IndexOfcurrenStartOfAiPieces = 16;

    /*START OF CREATING THE PIECES GAMEOBJECTS*/
    [Header("THIS ARE THE PLAYER PIECES")]
    [SerializeField]
    GameObject kingGameObject;
    [SerializeField]
    GameObject queenGameObject;
    [SerializeField]
    GameObject rookGameObject;
    [SerializeField]
    GameObject bishopGameObject;
    [SerializeField]
    GameObject knightGameObject;
    [SerializeField]
    GameObject pawnGameObject;
    /*END OF CREATING THE PIECES GAMEOBJECTS*/

    /*START OF CREATING THE AI PIECES GAMEOBJECTS*/
    [Header("THIS ARE THE AI PLAYER PIECES")]
    [SerializeField]
    GameObject aiKingGameObject;
    [SerializeField]
    GameObject aiQueenGameObject;
    [SerializeField]
    GameObject aiRookGameObject;
    [SerializeField]
    GameObject aiBishopGameObject;
    [SerializeField]
    GameObject aiKnightGameObject;
    [SerializeField]
    GameObject aiPawnGameObject;
    /*END OF CREATING THE AI PIECES GAMEOBJECTS*/

    //this function will construct the board by spawning every square
    void ConstructChessBoard()
    {
        //this bool will control if the square is black or white
        bool paintBlack=false;
        //get the size of the chess board first dimension
        for (int y = 0; y < chesSquares.GetLength(0); y++)
        {
            //Invert the color when we move up one colum so that the color above is diferent then the one below
            if (paintBlack == true)
                paintBlack = false;
            else
                paintBlack = true;
            
            //this will go trhough every x  position (left to right) and spawn the correct square
            for (int x=0; x< chesSquares.GetLength(1); x++)
            {
                if(paintBlack==true)
                {
                    chesSquares[y,x]=Instantiate(blackBoardSquare, new Vector3(x,y,0), Quaternion.identity,squaresParent.transform);
                    paintBlack = false;
                }
                else
                {
                    chesSquares[y, x] = Instantiate(whiteBoardSquare, new Vector3(x, y, 0), Quaternion.identity, squaresParent.transform);
                    paintBlack = true;
                }
            }
        }
        
    }

    //this function will construct the pieces by spawning every piece
    void ConstructAllPieces()
    {
        //this will create every piece and store it here
        chessPieces.AddRange(new List<ChessPiece>
        {
            //spawn All player pieces X then Y
            new ChessPiece(ChessPiece.typeOfChessPiece.Rook, rookGameObject, false, new Vector2Int(0,0)),
            new ChessPiece(ChessPiece.typeOfChessPiece.Knight, knightGameObject, false, new Vector2Int(1,0)),
            new ChessPiece(ChessPiece.typeOfChessPiece.Bishop, bishopGameObject, false, new Vector2Int(2,0)),
            new ChessPiece(ChessPiece.typeOfChessPiece.Queen, queenGameObject, false, new Vector2Int(3,0)),
            new ChessPiece(ChessPiece.typeOfChessPiece.King, kingGameObject, false, new Vector2Int(4,0)),
            new ChessPiece(ChessPiece.typeOfChessPiece.Bishop, bishopGameObject, false, new Vector2Int(5,0)),
            new ChessPiece(ChessPiece.typeOfChessPiece.Knight, knightGameObject, false, new Vector2Int(6,0)),
            new ChessPiece(ChessPiece.typeOfChessPiece.Rook, rookGameObject, false, new Vector2Int(7,0)),
            //Spawn All Player Pawns
            new ChessPiece(ChessPiece.typeOfChessPiece.Pawns, pawnGameObject, false, new Vector2Int(0,1)),
            new ChessPiece(ChessPiece.typeOfChessPiece.Pawns, pawnGameObject, false, new Vector2Int(1,1)),
            new ChessPiece(ChessPiece.typeOfChessPiece.Pawns, pawnGameObject, false, new Vector2Int(2,1)),
            new ChessPiece(ChessPiece.typeOfChessPiece.Pawns, pawnGameObject, false, new Vector2Int(3,1)),
            new ChessPiece(ChessPiece.typeOfChessPiece.Pawns, pawnGameObject, false, new Vector2Int(4,1)),
            new ChessPiece(ChessPiece.typeOfChessPiece.Pawns, pawnGameObject, false, new Vector2Int(5,1)),
            new ChessPiece(ChessPiece.typeOfChessPiece.Pawns, pawnGameObject, false, new Vector2Int(6,1)),
            new ChessPiece(ChessPiece.typeOfChessPiece.Pawns, pawnGameObject, false, new Vector2Int(7,1)),


            //spawn All AI pieces X then Y
            new ChessPiece(ChessPiece.typeOfChessPiece.Rook, aiRookGameObject, true, new Vector2Int(0,7)),
            new ChessPiece(ChessPiece.typeOfChessPiece.Knight, aiKnightGameObject, true, new Vector2Int(1,7)),
            new ChessPiece(ChessPiece.typeOfChessPiece.Bishop, aiBishopGameObject, true, new Vector2Int(2,7)),
            new ChessPiece(ChessPiece.typeOfChessPiece.Queen, aiQueenGameObject, true, new Vector2Int(3,7)),
            new ChessPiece(ChessPiece.typeOfChessPiece.King, aiKingGameObject, true, new Vector2Int(4,7)),
            new ChessPiece(ChessPiece.typeOfChessPiece.Bishop, aiBishopGameObject, true, new Vector2Int(5,7)),
            new ChessPiece(ChessPiece.typeOfChessPiece.Knight, aiKnightGameObject, true, new Vector2Int(6,7)),
            new ChessPiece(ChessPiece.typeOfChessPiece.Rook, aiRookGameObject, true, new Vector2Int(7,7)),
            //Spawn All AI Pawns
            new ChessPiece(ChessPiece.typeOfChessPiece.Pawns, aiPawnGameObject, true, new Vector2Int(0,6)),
            new ChessPiece(ChessPiece.typeOfChessPiece.Pawns, aiPawnGameObject, true, new Vector2Int(1,6)),
            new ChessPiece(ChessPiece.typeOfChessPiece.Pawns, aiPawnGameObject, true, new Vector2Int(2,6)),
            new ChessPiece(ChessPiece.typeOfChessPiece.Pawns, aiPawnGameObject, true, new Vector2Int(3,6)),
            new ChessPiece(ChessPiece.typeOfChessPiece.Pawns, aiPawnGameObject, true, new Vector2Int(4,6)),
            new ChessPiece(ChessPiece.typeOfChessPiece.Pawns, aiPawnGameObject, true, new Vector2Int(5,6)),
            new ChessPiece(ChessPiece.typeOfChessPiece.Pawns, aiPawnGameObject, true, new Vector2Int(6,6)),
            new ChessPiece(ChessPiece.typeOfChessPiece.Pawns, aiPawnGameObject, true, new Vector2Int(7,6)),
        });
    }

    //this square is  going to paint back to the original color the green squares
    void RepaintPaintedSquares()
    { 
        foreach(SquaresSelected x in squaresSelected)
        {
            chesSquares[x.pos.x, x.pos.y].gameObject.GetComponent<SpriteRenderer>().color = x.oldColor;
        }
        squaresSelected.Clear();
    }

    //this will show every possible move to the user by putting the tiles that we can move as green
    void ShowAllPossibleMoves(ChessPiece chessPiece_)
    {
        //this is going to call the function that is going to repaint the squares
        RepaintPaintedSquares();
        //this will store every moves possible
        List<Vector2Int> allMovesPossible = new List<Vector2Int>();
        //this will get all of the possible moves
        allMovesPossible.AddRange(chessPiece_.ReturnPossibleMoves(new Vector2Int((int)chessPiece_.chessPieceGameObject.transform.localPosition.x, (int)chessPiece_.chessPieceGameObject.transform.localPosition.y), chessPieces));

        //go trought every possible move and change its color to green
        foreach (Vector2Int allMovesPossible_ in allMovesPossible)
        {
            //this variable will be used to store the position and the old color of the square that is going to be green
            SquaresSelected tmpVariable;

            tmpVariable.pieceSelected = chessPiece_;
            //set the pos
            tmpVariable.pos = new Vector2Int((int)allMovesPossible_.y, (int)allMovesPossible_.x);
            //set the color
            tmpVariable.oldColor = chesSquares[(int)allMovesPossible_.y, (int)allMovesPossible_.x].gameObject.GetComponent<SpriteRenderer>().color;
            //add the variable with all the information to the list
            squaresSelected.Add(tmpVariable);

            //paint green the square that can be moved
            chesSquares[(int)allMovesPossible_.y, (int)allMovesPossible_.x].gameObject.GetComponent<SpriteRenderer>().color = Color.green;    
        }
    }


    //this will return what piece is at the position we asks
    public ChessPiece FindPieceAtPosition(Vector2Int pos)
    {
        //go trough every chesspiece and see if that piece is on the position we are asking
        foreach(ChessPiece chesspiece_ in chessPieces)
        { 
            //if the chesspiece is on the same position as the position we are asking return what chesspiece is there
            if((Vector2)chesspiece_.chessPieceGameObject.transform.position == pos)
            {
                return chesspiece_;
            }
        }
        return null;
    }

    //if this is true its because its the ai turn to play
    bool aiTurn = false;

    // Start is called before the first frame update
    void Start()
    {
        //set the win and lose condition image to inactive (invisable)
        imgLoseCondition.SetActive(false);
        imgWinCondition.SetActive(false);

        ConstructChessBoard();
        ConstructAllPieces();
    }

    //this function will be called every time the mouse is clicked
    void MouseClickFunction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //this will create x and y that will store what square was clicked
        int x, y;
        //assign the converted float of the mouse click pos
        x = (int)ray.origin.x;
        y = (int)ray.origin.y;
        //this will check if the diference is more then 0.5 to fix the converting to int problem, this 2 ifs make it so it works
        if (ray.origin.x - (int)ray.origin.x > 0.5f)
        {
            x = (int)ray.origin.x + 1;
        }
        if (ray.origin.y - (int)ray.origin.y > 0.5f)
        {
            y = (int)ray.origin.y + 1;
        }

        //this will check if theres any piece at the clicked position, if there is, it will return the piece there
        ChessPiece chessPieceAtPosition = FindPieceAtPosition(new Vector2Int(x, y));
        
        /*if the variable chessPieceAtPosition is initialized 
        * (i do this because if i dont findt anyoune on the function above, it will return null)
        *Show all the moves possible*/
        if (!Object.Equals(chessPieceAtPosition, default(ChessPiece)) && chessPieceAtPosition.isAi==false)
        {
            ShowAllPossibleMoves(chessPieceAtPosition);
        }
        //if we dont touched a piece check if we touched a selected square if we did move the piece there and repaint it
        else
        {
            foreach (SquaresSelected temp in squaresSelected)
            {
                //if the pos of the current painted square is the same as the mouse pos move the piece and repaint it
                if (temp.pos == new Vector2Int(y, x))
                {
                    //call the function that will kill the enemy piece if we found one
                    KillPieceIfBoolIsTrue(true, new Vector2Int(temp.pos.y, temp.pos.x));
                    //move the piece to the position selected
                    temp.pieceSelected.chessPieceGameObject.transform.position = new Vector2(temp.pos.y, temp.pos.x);
                    //if someone won exit this so that the ai cant play
                    if (gameFinished == true)
                        break;
                    RepaintPaintedSquares();
                    //say that its the ai turn to move
                    aiTurn = true;
                    //call the function that will start the ai functionality
                    Aiturn();
                    break;
                }
            }
        }
    }

    //this function will kill if we have a piece at the position requested, it will only kill if the bool is true
    public int KillPieceIfBoolIsTrue(bool doYouWantMeToKill, Vector2Int pos)
    {
        //MIGUEL LER ISTO OPTIMISACAO
        //THIS FUNCTION PROBABLY COULD BE OPTIMIZED, SINCE WE USE THE FindPieceAtPosition FUNCTION TO FIND THE CHESS PIECE THERE, INSTEAD WE COULD RETURN THE POSITION OF THE CHESS PIECE THERE, AND LIKE THAT WE DONT
        //NEED TO DO ANOTHER LOOP TO CHECK IF THIS PIECE ID IS THE SAME AS THE PIECE ID OF THE CHESS PIECE WE WANT TO DESTROY, SINCE WE WOULD HAVE THE INDEX RETURNED FROM THE FindPieceAtPosition FUNCTION
        

        //see if we find a piece at the position requested
        ChessPiece chessPieceAtPosition = FindPieceAtPosition(pos);

        /*if the variable chessPieceAtPosition is initialized 
        * (i do this because if i dont findt anyoune on the function above, it will return null)
        *Show all the moves possible*/
        if (!Object.Equals(chessPieceAtPosition, default(ChessPiece)))
        {
            //this loop will search for every piece and destroy it, and then return the points of the chesspiece
            for(int i = 0; i<chessPieces.Count; i++)
            {
                //if this is the chess piece at the found position above
                if (chessPieces[i].myId == chessPieceAtPosition.myId)
                {
                    //if we want to kill the piece, remove it from the array
                    if (doYouWantMeToKill == true)
                    {
                        //if the piece we are about to destroy is ai and had more then 500 points (ai king piece) we stop the game, since we won it
                        if (chessPieceAtPosition.isAi == true && chessPieceAtPosition.points > 500)
                        {
                            imgWinCondition.SetActive(true); //activate the image displaying the win condition
                            gameFinished = true; // tell the game its over
                        }
                        //if the piece we are about to destroy is NOT ai and had more then 500 points (my king piece) we stop the game, since we lost it
                        else if (chessPieceAtPosition.isAi == false && chessPieceAtPosition.points > 500)
                        {
                            imgLoseCondition.SetActive(true); //activate the image displaying the lose condition
                            gameFinished = true; // tell the game its over
                        }

                        chessPieces.RemoveAt(i);
                        Destroy(chessPieceAtPosition.chessPieceGameObject);

                    }
                    return chessPieceAtPosition.points;
                }
                    
            }            
        }
        return 0;
    }

    //this will fake kill a piece, i do this so that in one or more depth search we can kill a piece check ai and then revive it
    public int FakeKillPieceIfBoolIsTrue(Vector2Int pos)
    {
        //MIGUEL LER ISTO OPTIMISACAO
        //THIS FUNCTION PROBABLY COULD BE OPTIMIZED, SINCE WE USE THE FindPieceAtPosition FUNCTION TO FIND THE CHESS PIECE THERE, INSTEAD WE COULD RETURN THE POSITION OF THE CHESS PIECE THERE, AND LIKE THAT WE DONT
        //NEED TO DO ANOTHER LOOP TO CHECK IF THIS PIECE ID IS THE SAME AS THE PIECE ID OF THE CHESS PIECE WE WANT TO DESTROY, SINCE WE WOULD HAVE THE INDEX RETURNED FROM THE FindPieceAtPosition FUNCTION


        //see if we find a piece at the position requested
        ChessPiece chessPieceAtPosition = FindPieceAtPosition(pos);

        /*if the variable chessPieceAtPosition is initialized 
        * (i do this because if i dont findt anyoune on the function above, it will return null)
        *Show all the moves possible*/
        if (!Object.Equals(chessPieceAtPosition, default(ChessPiece)))
        {
            //this loop will search for every piece and check if it finds the piece at the position, if it does, it will return wich piece it is
            for (int i = 0; i < chessPieces.Count; i++)
            {
                //if this is the chess piece at the found position above
                if (chessPieces[i].myId == chessPieceAtPosition.myId)
                {
                    //chessPieces.RemoveAt(i);
                    //Destroy(chessPieceAtPosition.chessPieceGameObject);   

                    return i;
                }

            }
        }
        //if theres no piece here return -99, error number 
        return -99;
    }

    //this will be called when the player ends his turn
    void Aiturn()
    {
        //if the diffuclty is very easy use the very easy ai
        if (dificulty == Dificulty.VeryEasy)
            AiFunctions.RandomMove(this);

        //if the diffuclty is easy use the easy ai
        else if (dificulty == Dificulty.Easy)
            AiFunctions.RandomMoveExceptIfTheresAKill(this);

        //if the diffuclty is Normal defensive use the Normal defensive ai
        else if (dificulty == Dificulty.NormalDefensive)
            //this will play with one depth move, ut it will only defend and counter attack, it will repeat a lot of moves
            AiFunctions.OneDephtDefensiveMoveCheckMyMoveThenPlayerMove(this);

        //if the diffuclty is Normal use the Normal ai
        else if (dificulty == Dificulty.Normal)
            //this will play with one depth move, but will play normally, and will not repeat itself, every time is different
            AiFunctions.OneDephtNormalMoveCheckMyMoveThenPlayerMove(this);

        //AiFunctions.TwoDephtMoveCheckMyMoveThenPlayerMove(this);
    }

    //this will see where the ai index is, since the array of chesspieces is by order, and we need to know were the ai starts for us to be able to go trough less loops
    public int ReturnIndexOfCurrenStartOfAiPieces()
    {
        //if the midle of this array is an ai, search backwards
        if(chessPieces[chessPieces.Count/2].isAi==true)
        {
            //say this is the first ai pice, and then we will check if theres any other piece before, if there is thats the one that is ai
            IndexOfcurrenStartOfAiPieces = chessPieces.Count / 2;
            //if the middle of the array is not an ai, search forward, since the first ai is forward on the array
            for (int i = chessPieces.Count / 2; i >= 0; i--)
            {
                //if the current chess pice is ai, just say that its the first ai, and then break, to leave the loop and return the index of the ai
                if (chessPieces[i].isAi)
                {
                    IndexOfcurrenStartOfAiPieces = i;
                    continue;
                }
                //if the cxurrent position is not ai leave the for loop
                else
                {
                    break;
                }
            }
        }
        //if the middle piece is not ai search forwards, since the order of the array is non ai and then ai 
        else
        {
            //if the middle of the array is not an ai, search forward, since the first ai is forward on the array
            for (int i = chessPieces.Count/2; i < chessPieces.Count; i++)
            {
                //if the current chess pice is ai, just say that its the first ai, and then break, to leave the loop and return the index of the ai
                if(chessPieces[i].isAi)
                {
                    IndexOfcurrenStartOfAiPieces = i;
                }
                else
                {
                    break;
                }
            }
        }
        
        return IndexOfcurrenStartOfAiPieces;
    }

    // Update is called once per frame
    void Update()
    {
        int a = ReturnIndexOfCurrenStartOfAiPieces();
        if (Input.GetMouseButtonDown(0) && gameFinished==false)
        {
            MouseClickFunction();
        }

    }
}
