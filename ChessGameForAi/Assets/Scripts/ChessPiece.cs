using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour
{
    // this variable will store the points of this chess piece and it has private set so that i can edit inside this class only
    public int points { get; private set; }

    //this will create a unique id so that we can destroy an object of this class, and like this we have an identifier
    //the static id will increment by chesspiece object and the public variable will save this object id
    private static int ID = -1;
    public  int myId = 0;

    //this will store all of the types of chessPieces
    public enum typeOfChessPiece { King, Queen, Rook, Bishop, Knight, Pawns};

    //this will store this chess piece type
    public typeOfChessPiece thisChessPiece { get; }

    //this will store the game object of this piece
    public GameObject chessPieceGameObject { get; set; }

    //this will store if its an ai or not
    public bool isAi;

    //create a list of the available moves
    private List<Vector2Int> availableMoves = new List<Vector2Int>();

    

    // Constructor that takes what chess piece it his has an argument:
    public ChessPiece(typeOfChessPiece thisChessPieceType_, GameObject chessPieceGameObject_, bool isAi_, Vector2Int initialPos)
    {
        //i increase 1 id to the total items and assign the id of this item
        ID++;
        this.myId = ID;

        //spawn the object and move the gameobject to the initial position of the piece
        chessPieceGameObject = Instantiate(chessPieceGameObject_, (Vector2)initialPos, Quaternion.identity);
        chessPieceGameObject.transform.position = (Vector2)initialPos;
        //storing forever if im ai or not
        isAi = isAi_;
        //storing forever which piece am i
        thisChessPiece = thisChessPieceType_;
        
        //this switch case will decide wich "constructor" this piece has
        switch (thisChessPieceType_)
        {
            case typeOfChessPiece.King:
                King();
                break;
            case typeOfChessPiece.Bishop:
                Bishop();
                break;
            case typeOfChessPiece.Knight:
                Knight();
                break;
            case typeOfChessPiece.Pawns:
                Pawn();
                break;
            case typeOfChessPiece.Queen:
                Queen();
                break;
            case typeOfChessPiece.Rook:
                Rook();
                break;
        }
            
       

    }


    private void King()
    {
        //i got the points from this website https://www.freecodecamp.org/news/simple-chess-ai-step-by-step-1d55a9266977/
        points = 900;

        //add every possible move to the pawn in this case one up move
        availableMoves.AddRange(new List<Vector2Int>
        {
            new Vector2Int(0, +1),
            new Vector2Int(+1, +1),
            new Vector2Int(+1, +0),
            new Vector2Int(+1, -1),
            new Vector2Int(+0, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, +1)

        });
    }

    private void Bishop()
    {
        //i got the points from this website https://www.freecodecamp.org/news/simple-chess-ai-step-by-step-1d55a9266977/
        points = 30;

        //add every possible move to the pawn in this case one up move
        availableMoves.AddRange(new List<Vector2Int>
        {
            //bottom Left Diagonals
            new Vector2Int(-1, -1),
            new Vector2Int(-2, -2),
            new Vector2Int(-3, -3),
            new Vector2Int(-4, -4),
            new Vector2Int(-5, -5),
            new Vector2Int(-6, -6),
            new Vector2Int(-7, -7),
            
            //top Left Diagonals
            new Vector2Int(-1, +1),
            new Vector2Int(-2, +2),
            new Vector2Int(-3, +3),
            new Vector2Int(-4, +4),
            new Vector2Int(-5, +5),
            new Vector2Int(-6, +6),
            new Vector2Int(-7, +7),

            
            //top right Diagonals
            new Vector2Int(+1, +1),
            new Vector2Int(+2, +2),
            new Vector2Int(+3, +3),
            new Vector2Int(+4, +4),
            new Vector2Int(+5, +5),
            new Vector2Int(+6, +6),
            new Vector2Int(+7, +7),


            //bottom right Diagonals
            new Vector2Int(+1, -1),
            new Vector2Int(+2, -2),
            new Vector2Int(+3, -3),
            new Vector2Int(+4, -4),
            new Vector2Int(+5, -5),
            new Vector2Int(+6, -6),
            new Vector2Int(+7, -7),
        });
    }

    private void Knight()
    {
        //i got the points from this website https://www.freecodecamp.org/news/simple-chess-ai-step-by-step-1d55a9266977/
        points = 30;

        //add every possible move to the pawn in this case one up move
        availableMoves.AddRange(new List<Vector2Int>
        {
            //top left
            new Vector2Int(-1, +2),
            //left a litle bit top
            new Vector2Int(-2, +1),

            //top right
            new Vector2Int(+1, +2),
            //right a litle bit top
            new Vector2Int(+2, +1),

            //bottom right
            new Vector2Int(+1, -2),
            //right a litle bit bottom
            new Vector2Int(+2, -1),

            //bottom left
            new Vector2Int(-1, -2),
            //left a litle bit bottom
            new Vector2Int(-2, -1),
        });
    }

    private void Pawn()
    {
        //i got the points from this website https://www.freecodecamp.org/news/simple-chess-ai-step-by-step-1d55a9266977/
        points = 10;

        //if its a player pawns
        if(isAi==false)
        {
            //add every possible move to the pawn in this case one up move
            availableMoves.AddRange(new List<Vector2Int>
            {
                new Vector2Int(0, +1),
                new Vector2Int(0, +2),
                //top left
                new Vector2Int(-1, +1),
                //top right
                new Vector2Int(+1, +1)
                
            });
        }
        //if its an ai pawn
        else
        {
            //add every possible move to the pawn in this case one up move
            availableMoves.AddRange(new List<Vector2Int>
            {
                new Vector2Int(0, -1),
                new Vector2Int(0, -2),
                //top left
                new Vector2Int(-1, -1),
                //top right
                new Vector2Int(+1, -1)
            });
        }
        
    }

    private void Queen()
    {
        //i got the points from this website https://www.freecodecamp.org/news/simple-chess-ai-step-by-step-1d55a9266977/
        points = 90;

        //add every possible move to the pawn in this case one up move
        availableMoves.AddRange(new List<Vector2Int>
        {
            //bottom Left Diagonals
            new Vector2Int(-1, -1),
            new Vector2Int(-2, -2),
            new Vector2Int(-3, -3),
            new Vector2Int(-4, -4),
            new Vector2Int(-5, -5),
            new Vector2Int(-6, -6),
            new Vector2Int(-7, -7),
            
            //top Left Diagonals
            new Vector2Int(-1, +1),
            new Vector2Int(-2, +2),
            new Vector2Int(-3, +3),
            new Vector2Int(-4, +4),
            new Vector2Int(-5, +5),
            new Vector2Int(-6, +6),
            new Vector2Int(-7, +7),

            
            //top right Diagonals
            new Vector2Int(+1, +1),
            new Vector2Int(+2, +2),
            new Vector2Int(+3, +3),
            new Vector2Int(+4, +4),
            new Vector2Int(+5, +5),
            new Vector2Int(+6, +6),
            new Vector2Int(+7, +7),


            //bottom right Diagonals
            new Vector2Int(+1, -1),
            new Vector2Int(+2, -2),
            new Vector2Int(+3, -3),
            new Vector2Int(+4, -4),
            new Vector2Int(+5, -5),
            new Vector2Int(+6, -6),
            new Vector2Int(+7, -7),


             //Move top
            new Vector2Int(0, +1),
            new Vector2Int(0, +2),
            new Vector2Int(0, +3),
            new Vector2Int(0, +4),
            new Vector2Int(0, +5),
            new Vector2Int(0, +6),
            new Vector2Int(0, +7),


            //Move bottom
            new Vector2Int(0, -1),
            new Vector2Int(0, -2),
            new Vector2Int(0, -3),
            new Vector2Int(0, -4),
            new Vector2Int(0, -5),
            new Vector2Int(0, -6),
            new Vector2Int(0, -7),

            //Move right
            new Vector2Int(+1, 0),
            new Vector2Int(+2, 0),
            new Vector2Int(+3, 0),
            new Vector2Int(+4, 0),
            new Vector2Int(+5, 0),
            new Vector2Int(+6, 0),
            new Vector2Int(+7, 0),


            
            //Move left
            new Vector2Int(-1, 0),
            new Vector2Int(-2, 0),
            new Vector2Int(-3, 0),
            new Vector2Int(-4, 0),
            new Vector2Int(-5, 0),
            new Vector2Int(-6, 0),
            new Vector2Int(-7, 0)
        });
    }

    private void Rook()
    {
        //i got the points from this website https://www.freecodecamp.org/news/simple-chess-ai-step-by-step-1d55a9266977/
        points = 50;

        //add every possible move to the pawn in this case one up move
        availableMoves.AddRange(new List<Vector2Int>
        {
            //Move top
            new Vector2Int(0, +1),
            new Vector2Int(0, +2),
            new Vector2Int(0, +3),
            new Vector2Int(0, +4),
            new Vector2Int(0, +5),
            new Vector2Int(0, +6),
            new Vector2Int(0, +7),


            //Move bottom
            new Vector2Int(0, -1),
            new Vector2Int(0, -2),
            new Vector2Int(0, -3),
            new Vector2Int(0, -4),
            new Vector2Int(0, -5),
            new Vector2Int(0, -6),
            new Vector2Int(0, -7),

            //Move right
            new Vector2Int(+1, 0),
            new Vector2Int(+2, 0),
            new Vector2Int(+3, 0),
            new Vector2Int(+4, 0),
            new Vector2Int(+5, 0),
            new Vector2Int(+6, 0),
            new Vector2Int(+7, 0),


            
            //Move left
            new Vector2Int(-1, 0),
            new Vector2Int(-2, 0),
            new Vector2Int(-3, 0),
            new Vector2Int(-4, 0),
            new Vector2Int(-5, 0),
            new Vector2Int(-6, 0),
            new Vector2Int(-7, 0)
        });
    }


    //this will return all possible moves
    public List<Vector2Int> ReturnPossibleMoves(Vector2Int currentPos, List<ChessPiece> chessPieces)
    {
        //this will store every possible move
        List<Vector2Int> movesPossibleToMAke = new List<Vector2Int>(); 

        //if this piece is a bishop
        if(thisChessPiece==typeOfChessPiece.Bishop)
        {
            /*this 4 bools will be true if theres some piece on front of the diogonals,
            if there are we cannot let him move trought them so we block the moves that continue from that diagonal*/
            bool blockBottomLeft = false;
            bool blockUpLeft = false;
            bool blockUpRight = false;
            bool blockBottomRight = false;

            //loop trough every available move and check if i can move there
            foreach (Vector2Int moves in availableMoves)
            {
                //if this move is bottom left and we blocked it (some piece is on the way) dont let him move here
                if (blockBottomLeft == true && moves.x < 0 && moves.y < 0)
                    continue;
                //if this move is up left and we blocked it (some piece is on the way) dont let him move here
                else if (blockUpLeft == true && moves.x < 0 && moves.y > 0)
                    continue;
                //if this move is up right and we blocked it (some piece is on the way) dont let him move here
                else if (blockUpRight == true && moves.x > 0 && moves.y > 0)
                    continue;
                //if this move is bottom right and we blocked it (some piece is on the way) dont let him move here
                else if (blockBottomRight == true && moves.x > 0 && moves.y < 0)
                    continue;


                if (currentPos.x + moves.x < 0 || currentPos.x + moves.x > 7 || currentPos.y + moves.y < 0 || currentPos.y + moves.y > 7)
                {
                    //NAO POSSO MOVER AQUI
                }
                else
                {
                    bool isAnyPieceHere = false;
                    //AQUI VERIFICAR SE TENHO ALGUMA PECA CA
                    foreach (ChessPiece chessPieces_ in chessPieces)
                    {
                        //if this piece has the same position has the move we are testing, we cant move there
                        if (chessPieces_.chessPieceGameObject.transform.position.x == moves.x + currentPos.x && chessPieces_.chessPieceGameObject.transform.position.y == moves.y + currentPos.y)
                        {
                            //if the move was bottom left block every bottom left move
                            if(moves.x<0 && moves.y<0)
                            {
                                blockBottomLeft = true;
                            }
                            //if the move was top left block every top left move
                            else if (moves.x < 0 && moves.y > 0)
                            {
                                blockUpLeft = true;
                            }
                            //if the move was top right block every top right move
                            else if (moves.x > 0 && moves.y > 0)
                            {
                                blockUpRight = true;
                            }
                            //if the move was bottom right block every bottom right move
                            else if (moves.x > 0 && moves.y < 0)
                            {
                                blockBottomRight = true;
                            }

                            //if the chesspiece that is on the move path is the same team has us dont had it has a playable move
                            if(chessPieces_.isAi==false && isAi==false)
                            {
                                isAnyPieceHere = true;
                                break;
                            }
                            else if (chessPieces_.isAi == true && isAi == true)
                            {
                                isAnyPieceHere = true;
                                break;
                            }
                                
                        }
                            
                    }
                    if (isAnyPieceHere == false)
                    {
                        //since im not out of bounds i can move here
                        movesPossibleToMAke.Add(new Vector2Int(moves.x + currentPos.x, moves.y + currentPos.y));
                    }

                }
            }
        }
        //if this piece is a Rook
        else if (thisChessPiece == typeOfChessPiece.Rook)
        {
            /*this 4 bools will be true if theres some piece on front of the top left right or bottom,
            if there are we cannot let him move trought them so we block the moves that continue from that direction*/
            bool blockBottom = false;
            bool blockUp = false;
            bool blockRight = false;
            bool blockLeft = false;

            //loop trough every available move and check if i can move there
            foreach (Vector2Int moves in availableMoves)
            {
                //if this move is bottom and we blocked it (some piece is on the way) dont let him move here
                if (blockBottom == true && moves.x == 0 && moves.y < 0)
                    continue;
                //if this move is up and we blocked it (some piece is on the way) dont let him move here
                else if (blockUp == true && moves.x == 0 && moves.y > 0)
                    continue;
                //if this move is right and we blocked it (some piece is on the way) dont let him move here
                else if (blockRight == true && moves.x > 0 && moves.y == 0)
                    continue;
                //if this move is left and we blocked it (some piece is on the way) dont let him move here
                else if (blockLeft == true && moves.x < 0 && moves.y == 0)
                    continue;


                if (currentPos.x + moves.x < 0 || currentPos.x + moves.x > 7 || currentPos.y + moves.y < 0 || currentPos.y + moves.y > 7)
                {
                    //NAO POSSO MOVER AQUI
                }
                else
                {
                    bool isAnyPieceHere = false;
                    //AQUI VERIFICAR SE TENHO ALGUMA PECA CA
                    foreach (ChessPiece chessPieces_ in chessPieces)
                    {
                        //if this piece has the same position has the move we are testing, we cant move there
                        if (chessPieces_.chessPieceGameObject.transform.position.x == moves.x + currentPos.x && chessPieces_.chessPieceGameObject.transform.position.y == moves.y + currentPos.y)
                        {
                            //if the move was bottom block every bottom move
                            if (moves.x == 0 && moves.y < 0)
                            {
                                blockBottom = true;
                            }
                            //if the move was top block every top move
                            else if (moves.x == 0 && moves.y > 0)
                            {
                                blockUp = true;
                            }
                            //if the move was right block every right move
                            else if (moves.x > 0 && moves.y == 0)
                            {
                                blockRight = true;
                            }
                            //if the move was left block every left move
                            else if (moves.x < 0 && moves.y == 0)
                            {
                                blockLeft = true;
                            }

                            //if the chesspiece that is on the move path is the same team has us dont had it has a playable move
                            if (chessPieces_.isAi == false && isAi == false)
                            {
                                isAnyPieceHere = true;
                                break;
                            }
                            else if (chessPieces_.isAi == true && isAi == true)
                            {
                                isAnyPieceHere = true;
                                break;
                            }
                            
                        }

                    }
                    if (isAnyPieceHere == false)
                    {
                        //since im not out of bounds i can move here
                        movesPossibleToMAke.Add(new Vector2Int(moves.x + currentPos.x, moves.y + currentPos.y));
                    }

                }
            }
        }
        //if this piece is a queen
        else if (thisChessPiece == typeOfChessPiece.Queen)
        {
            /*this 4 bools will be true if theres some piece on front of the top left right or bottom,
            if there are we cannot let him move trought them so we block the moves that continue from that direction*/
            bool blockBottom = false;
            bool blockUp = false;
            bool blockRight = false;
            bool blockLeft = false;

            /*this 4 bools will be true if theres some piece on front of the diogonals,
           if there are we cannot let him move trought them so we block the moves that continue from that diagonal*/
            bool blockBottomLeft = false;
            bool blockUpLeft = false;
            bool blockUpRight = false;
            bool blockBottomRight = false;

            //loop trough every available move and check if i can move there
            foreach (Vector2Int moves in availableMoves)
            {
                //if this move is bottom and we blocked it (some piece is on the way) dont let him move here
                if (blockBottom == true && moves.x == 0 && moves.y < 0)
                    continue;
                //if this move is up and we blocked it (some piece is on the way) dont let him move here
                else if (blockUp == true && moves.x == 0 && moves.y > 0)
                    continue;
                //if this move is right and we blocked it (some piece is on the way) dont let him move here
                else if (blockRight == true && moves.x > 0 && moves.y == 0)
                    continue;
                //if this move is left and we blocked it (some piece is on the way) dont let him move here
                else if (blockLeft == true && moves.x < 0 && moves.y == 0)
                    continue;

                //if this move is bottom left and we blocked it (some piece is on the way) dont let him move here
                else if (blockBottomLeft == true && moves.x < 0 && moves.y < 0)
                    continue;
                //if this move is up left and we blocked it (some piece is on the way) dont let him move here
                else if (blockUpLeft == true && moves.x < 0 && moves.y > 0)
                    continue;
                //if this move is up right and we blocked it (some piece is on the way) dont let him move here
                else if (blockUpRight == true && moves.x > 0 && moves.y > 0)
                    continue;
                //if this move is bottom right and we blocked it (some piece is on the way) dont let him move here
                else if (blockBottomRight == true && moves.x > 0 && moves.y < 0)
                    continue;

                if (currentPos.x + moves.x < 0 || currentPos.x + moves.x > 7 || currentPos.y + moves.y < 0 || currentPos.y + moves.y > 7)
                {
                    //NAO POSSO MOVER AQUI
                }
                else
                {
                    bool isAnyPieceHere = false;
                    //AQUI VERIFICAR SE TENHO ALGUMA PECA CA
                    foreach (ChessPiece chessPieces_ in chessPieces)
                    {
                        //if this piece has the same position has the move we are testing, we cant move there
                        if (chessPieces_.chessPieceGameObject.transform.position.x == moves.x + currentPos.x && chessPieces_.chessPieceGameObject.transform.position.y == moves.y + currentPos.y)
                        {
                            //if the move was bottom block every bottom move
                            if (moves.x == 0 && moves.y < 0)
                            {
                                blockBottom = true;
                            }
                            //if the move was top block every top move
                            else if (moves.x == 0 && moves.y > 0)
                            {
                                blockUp = true;
                            }
                            //if the move was right block every right move
                            else if (moves.x > 0 && moves.y == 0)
                            {
                                blockRight = true;
                            }
                            //if the move was left block every left move
                            else if (moves.x < 0 && moves.y == 0)
                            {
                                blockLeft = true;
                            }

                            //if the move was bottom left block every bottom left move
                            else if (moves.x < 0 && moves.y < 0)
                            {
                                blockBottomLeft = true;
                            }
                            //if the move was top left block every top left move
                            else if (moves.x < 0 && moves.y > 0)
                            {
                                blockUpLeft = true;
                            }
                            //if the move was top right block every top right move
                            else if (moves.x > 0 && moves.y > 0)
                            {
                                blockUpRight = true;
                            }
                            //if the move was bottom right block every bottom right move
                            else if (moves.x > 0 && moves.y < 0)
                            {
                                blockBottomRight = true;
                            }

                            //if the chesspiece that is on the move path is the same team has us dont had it has a playable move
                            if (chessPieces_.isAi == false && isAi == false)
                            {
                                isAnyPieceHere = true;
                                break;
                            }
                            else if (chessPieces_.isAi == true && isAi == true)
                            {
                                isAnyPieceHere = true;
                                break;
                            }
                            
                        }

                    }
                    if (isAnyPieceHere == false)
                    {
                        //since im not out of bounds i can move here
                        movesPossibleToMAke.Add(new Vector2Int(moves.x + currentPos.x, moves.y + currentPos.y));
                    }

                }
            }
        }
        //if this piece is a pawn
        else if (thisChessPiece== typeOfChessPiece.Pawns)
        {
            //this will be true if theres a piece on the 1 move of the pawn and the pawn is on the begining
            bool blockPawnMoveThatMoves2Houses = false;
            //loop trough every available move and check if i can move there
            foreach (Vector2Int moves in availableMoves)
            {
                //if these piece is a pawn and it has been moved once block the move that moves 2 houses 
                if (currentPos.y != 1 && moves.y > 1)
                {
                    continue;
                }
                //if these piece is a pawn and ai
                if (currentPos.y != 6 && moves.y < -1 && isAi == true)
                    continue;
                //if the pawn is on the biging and theres a piece on the first move, dont let him move 2 houses forward
                if (blockPawnMoveThatMoves2Houses == true && moves.x==0)
                    continue;

                //this will be true if theres an piece on the path
                bool isAnyPieceHere = false;

                if (currentPos.x + moves.x < 0 || currentPos.x + moves.x > 7 || currentPos.y + moves.y < 0 || currentPos.y + moves.y > 7)
                {
                    //NAO POSSO MOVER AQUI
                }
                else
                {
                    //if this is a diagonal move, tell the code that theres a piece here so that he doesnt add the move, and on the foreach below if theres
                    //an enemy piece there i put the bool false again so that i can add it to the lis, i do this so that this move only show if theres a piece there
                    if(moves.x!=0)
                    { isAnyPieceHere = true; }
                    //AQUI VERIFICAR SE TENHO ALGUMA PECA CA
                    foreach (ChessPiece chessPieces_ in chessPieces)
                    {
                        //if this piece has the same position has the move we are testing, we cant move there
                        if (chessPieces_.chessPieceGameObject.transform.position.x == moves.x + currentPos.x && chessPieces_.chessPieceGameObject.transform.position.y == moves.y + currentPos.y)
                        {

                            //if the chesspiece that is on the move path is the same team has us dont had it has a playable move
                            if (chessPieces_.isAi == false && isAi == false)
                            {
                                isAnyPieceHere = true;
                            }
                            else if (chessPieces_.isAi == true && isAi == true)
                            {
                                isAnyPieceHere = true;
                            }
                            //if we are moving up don let him move, because we only let him move to the enemy piece position if the move is diagonal
                            else if (moves.x == 0)
                            {
                                isAnyPieceHere = true;
                            }
                            //if the move is a diagonal and theres a enemy piece there tell the code to add the move
                            else if (moves.x != 0)
                                isAnyPieceHere = false;

                            //if we find an piece on the first move, block the next moves
                            if (currentPos.y == 1 && moves.y == 1)
                                blockPawnMoveThatMoves2Houses = true;
                            else if (currentPos.y == 6 && moves.y == -1)
                                blockPawnMoveThatMoves2Houses = true;
                        }
                    }

                    if (isAnyPieceHere == false)
                    {
                        //since im not out of bounds i can move here
                        movesPossibleToMAke.Add(new Vector2Int(moves.x + currentPos.x, moves.y + currentPos.y));
                    }

                }

                
            }
        }
        //if the piece is any other
        else
        {
            //loop trough every available move and check if i can move there
            foreach (Vector2Int moves in availableMoves)
            {
                //this will be true if theres an piece on the path
                bool isAnyPieceHere = false;
              
                if (currentPos.x + moves.x < 0 || currentPos.x + moves.x > 7 || currentPos.y + moves.y < 0 || currentPos.y + moves.y > 7)
                {
                    //NAO POSSO MOVER AQUI
                }
                else
                {   
                    
                    //AQUI VERIFICAR SE TENHO ALGUMA PECA CA
                    foreach (ChessPiece chessPieces_ in chessPieces)
                    {
                        //if this piece has the same position has the move we are testing, we cant move there
                        if (chessPieces_.chessPieceGameObject.transform.position.x == moves.x + currentPos.x && chessPieces_.chessPieceGameObject.transform.position.y == moves.y + currentPos.y)
                        {
                            
                            //if the chesspiece that is on the move path is the same team has us dont had it has a playable move
                            if (chessPieces_.isAi == false && isAi == false)
                            {
                                isAnyPieceHere = true;
                            }
                            else if (chessPieces_.isAi == true && isAi == true)
                            {
                                isAnyPieceHere = true;
                            }

                        }
                    }

                    if (isAnyPieceHere == false)
                    {
                        //since im not out of bounds i can move here
                        movesPossibleToMAke.Add(new Vector2Int(moves.x + currentPos.x, moves.y + currentPos.y));
                    }

                }

            }
        }

       
        //return all the possible moves
        return movesPossibleToMAke;
    }



    /* Method that overrides the base class (System.Object) implementation. https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/types/classes
    this code fixes the problem of every item base being null, now it will show the gameobject name
     */
    public override string ToString()
    {
        return chessPieceGameObject.name;
    }

    ~ChessPiece()
    {
        Debug.Log("class was destroyd");
    }
}
