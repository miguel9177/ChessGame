using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiFunctions : MonoBehaviour
{
    //this struct will store every move possible, its associated piece and the points it makes
    struct AllMovesStorer
    {
        //this is going to store the move
        public Vector2Int moves;
        //this is going to store how many points it makes 
        public int points;
        //this is going to store wich piece has this move
        public ChessPiece chesspiece;
    }

    //choose a random move without any inteligense
    public static void RandomMove(BoardConstructor bCScript)
    {
        //this is going to create a list that is going to store every info about the move
        List<AllMovesStorer> allMovesStorer = new List<AllMovesStorer>();

        //go through every ai chess piece
        foreach (ChessPiece aiChessPiece_ in bCScript.chessPieces)
        {
            if (aiChessPiece_.isAi == true)
            {
                //create a temporary possible moves 
                List<Vector2Int> tempPossibleMoves = new List<Vector2Int>();
                //add every possible move to this temporary possible moves variable
                tempPossibleMoves.AddRange(aiChessPiece_.ReturnPossibleMoves(new Vector2Int((int)aiChessPiece_.chessPieceGameObject.transform.position.x,
                                                                (int)aiChessPiece_.chessPieceGameObject.transform.position.y),
                                                                bCScript.chessPieces));
                //this foreach is gonna go throught every possible move of the current aichesspiece
                foreach (Vector2Int possibleMove_ in tempPossibleMoves)
                {
                    //Create a temporary all moves storer for me to be able to add this to the list later since i dont know how to add a item to an list of structs without a struct with the same type
                    AllMovesStorer tempAllMovesStorer;
                    //get the chess piece that is going to be used to populate the list of  all moves storer
                    tempAllMovesStorer.chesspiece = aiChessPiece_;
                    //get the move that is going to be used to populate the list of  all moves storer
                    tempAllMovesStorer.moves = possibleMove_;
                    //MIGUEL EDIT THESE I NEED A FUNCTION THAT GIVES THE POINTS TO THE AI FOR HIM TO KNOW IF ITS GOOD OR NOT
                    tempAllMovesStorer.points = 0;

                    //populate the all moves storer with the move, the chess piece that has that move and how many points it has
                    allMovesStorer.Add(tempAllMovesStorer);
                }
            }
        }

        //this is going to store a random move that the ai is going to do
        int a = Random.Range(0, allMovesStorer.Count - 1);
        //kill the piece on this place if theres any piece there
        bCScript.KillPieceIfBoolIsTrue(true, allMovesStorer[a].moves);
        //move the ai
        allMovesStorer[a].chesspiece.chessPieceGameObject.transform.position = (Vector2)allMovesStorer[a].moves;

    }

    //choose a random move but if any of the moves can kill any piece, it will move the piece that gives more points 
    public static void RandomMoveExceptIfTheresAKill(BoardConstructor bCScript)
    {
        //this is going to create a move storer that is going to store every info about the most powerful move
        AllMovesStorer mostPowerfullMove = new AllMovesStorer();
        mostPowerfullMove.points = 0;
        //this is going to create a list that is going to store every info about the move
        List<AllMovesStorer> allMovesStorer = new List<AllMovesStorer>();

        //this will be true if we find a move that has more then 0 points
        bool weFoundAPowerFullMove = false;

        //go through every ai chess piece
        foreach (ChessPiece aiChessPiece_ in bCScript.chessPieces)
        {
            //if this chess piece is not ai, go to the next iteration. PROVAVELMENTE DA PARA OTIMIZAR, POIS POSSO ANTES DO LOOP CRIAR UMA LISTA SO COM AS AI CHESS PIECE, E ASSIM N TENNHO QUE PERCORRER AS PECAS Q N SAO AI, ISTO SO DEVE SER UTIL QUANDO ESTE FOR SEJA PESADO
            if (aiChessPiece_.isAi != true)
                continue;

            //create a temporary possible moves of the current piece
            List<Vector2Int> tempPossibleMovesOfThisPiece = new List<Vector2Int>();
            //add every possible move to this temporary possible moves variable
            tempPossibleMovesOfThisPiece.AddRange(aiChessPiece_.ReturnPossibleMoves(new Vector2Int((int)aiChessPiece_.chessPieceGameObject.transform.position.x,
                                                            (int)aiChessPiece_.chessPieceGameObject.transform.position.y),
                                                            bCScript.chessPieces));

            //this foreach is gonna go throught every possible move of the current aichesspiece
            foreach (Vector2Int possibleMove_ in tempPossibleMovesOfThisPiece)
            {
                //this is going to store the points of the current move (from 0 to 900)
                int pointsOfCurrentMove = bCScript.KillPieceIfBoolIsTrue(false, possibleMove_);
                //if the current move is stronger then the last most powerful move, store this current move instead, since we want to play the best move
                if (pointsOfCurrentMove > mostPowerfullMove.points)
                {
                    //store the chess piece that can do this move
                    mostPowerfullMove.chesspiece = aiChessPiece_;
                    //store the move position
                    mostPowerfullMove.moves = possibleMove_;
                    //store how many points this move gives
                    mostPowerfullMove.points = pointsOfCurrentMove;
                    //warn the code to not add more items to the list since we dont need it, since we already found a move that gives more points then 0
                    weFoundAPowerFullMove = true;
                }
                //if we find a move that kills, we dont need to add the moves to the list, since we already a good move
                else if (weFoundAPowerFullMove == false)
                {
                    //this will create a temporary variable to be able to add the item to the list
                    AllMovesStorer tempThisMove;
                    tempThisMove.chesspiece = aiChessPiece_;
                    tempThisMove.moves = possibleMove_;
                    tempThisMove.points = pointsOfCurrentMove;
                    allMovesStorer.Add(tempThisMove);
                }
            }

        }

        //if the current move points is NOT 0, its because we need to kill the piece at the move position
        if (mostPowerfullMove.points != 0)
        {
            //kill the piece at the move position
            bCScript.KillPieceIfBoolIsTrue(true, mostPowerfullMove.moves);
            //move the piece to the best move position
            mostPowerfullMove.chesspiece.chessPieceGameObject.transform.position = (Vector2)mostPowerfullMove.moves;
        }
        //if we dont have a killable piece (points different equal to 0)
        else
        {
            //this is going to store a random move that the ai is going to do
            int a = Random.Range(0, allMovesStorer.Count - 1);
            //move the ai
            allMovesStorer[a].chesspiece.chessPieceGameObject.transform.position = (Vector2)allMovesStorer[a].moves;
        }

    }

    //Check my move then the player and then decide if that move is good
    /*public static void OneDphtMoveCheckMyMoveThenPlayerMove(BoardConstructor bCScript)
    {
        //this is going to create a move storer that is going to store every info about the most powerful move
        AllMovesStorer mostPowerfullMove = new AllMovesStorer();
        mostPowerfullMove.points = -999;
        
        //this is going to create a move storer that is going to store every info about the worsrt move out of the current ai piece (since we always espect the worst outcome)
        AllMovesStorer worstMoveOfAI = new AllMovesStorer();
        //i give -999, so that the  first move enters
        worstMoveOfAI.points = -999;
       
 
        //this is going to store the best ai move
        int bestMove=-999;
        //this is ghoing to store the worse consequence of the ai play
        int worsePlayerMove=0;
 
 
        //this will store where the first ai chess piece is , i need this so that i can reduce the loop amounts
        int IndexOfcurrenStartOfAiPieces = bCScript.ReturnIndexOfCurrenStartOfAiPieces();
        //if the ai piece returned from the current start of ai pices, isnt a ai is becaus ethat function is bugging, so we return a error
        if(bCScript.chessPieces[IndexOfcurrenStartOfAiPieces].isAi==false)
        {
            Debug.LogError("THE FUNCTION ReturnIndexOfCurrenStartOfAiPieces(), RETURNED A VALUE THAT ISNT A AI PIECE, IT SHOULD RETURN THE FIRST AI PIECE");
        }
 
        //go through every AI chess piece
        for (int i=IndexOfcurrenStartOfAiPieces; i< bCScript.chessPieces.Count; i++)
        {
            //create a temporary possible moves of the current piece
            List<Vector2Int> tempPossibleMovesOfThisPiece = new List<Vector2Int>();
 
            //this will add every possible move of the current chess piece
            tempPossibleMovesOfThisPiece.AddRange(bCScript.chessPieces[i].ReturnPossibleMoves(new Vector2Int((int)bCScript.chessPieces[i].chessPieceGameObject.transform.position.x,
                                                           (int)bCScript.chessPieces[i].chessPieceGameObject.transform.position.y),
                                                           bCScript.chessPieces));
 
            //this foreach is gonna go throught every possible move of the current aichesspiece
            for (int i_ = 0; i_<tempPossibleMovesOfThisPiece.Count; i_++)
            {
                //this is going to store the points of the current move (from 0 to 900)
                int pointsOfCurrentMove = bCScript.KillPieceIfBoolIsTrue(false, tempPossibleMovesOfThisPiece[i_]);
               
                //this is going to store the old position, since we want to move the piece to be able to search for the future, with the ai pice moved, by the current ai piece move
                Vector2 oldPos = bCScript.chessPieces[i].chessPieceGameObject.transform.position;
 
                //this will move the ai piece to the 
                bCScript.chessPieces[i].chessPieceGameObject.transform.position = new Vector2(tempPossibleMovesOfThisPiece[i_].x, tempPossibleMovesOfThisPiece[i_].y);
 
                //this will get the current worse outcome possible for the current move
                int worsePlayerMoveForCurrentMove = GoTroughEveryPlayerMoveAndReturnWorseOutcome(IndexOfcurrenStartOfAiPieces, bCScript);
 
 
                //MIGUEL THE PROBLEM IS THIS IF
                //if the ponts of this move minus the outcome of the worst playr move is bigger then the saved best moved minus the saved worst outcome of that move
                if ((pointsOfCurrentMove - worsePlayerMoveForCurrentMove) > (bestMove - worsePlayerMove))
                {
                    //since the points of this current move is bertter then the saved best move, change the best move to the current one
                    bestMove = pointsOfCurrentMove;
                    //update the worst move aswell
                    worsePlayerMove = worsePlayerMoveForCurrentMove;
                    //store the chesspice that is going to make the move
                    worstMoveOfAI.chesspiece = bCScript.chessPieces[i];
                    //store the move
                    worstMoveOfAI.moves = tempPossibleMovesOfThisPiece[i_];
                    //store the points of the move (from -999 to 999)
                    worstMoveOfAI.points = bestMove - worsePlayerMove;
                }
               
 
 
                //this will reset the old position since we moved the peace for us to be able to see in the future
                bCScript.chessPieces[i].chessPieceGameObject.transform.position = oldPos;
 
           
            }
           
            //if the current move is stronger then the last most powerful move, store this current move instead, since we want to play the best move
            if (mostPowerfullMove.points < worstMoveOfAI.points)
            {
                //the chess piece is equal to the i since thats the move of the ai, and we are seing into the future, and the i is the ai piece that gets this future
                mostPowerfullMove.chesspiece = worstMoveOfAI.chesspiece;
                //store the move position
                mostPowerfullMove.moves = worstMoveOfAI.moves;
                //store how many points this move gives
                mostPowerfullMove.points = worstMoveOfAI.points;
            }
 
        }
 
        //if the current move points is NOT 0, its because we need to kill the piece at the move position
        if (mostPowerfullMove.points != -999)
        {
            //kill the piece at the move position
            bCScript.KillPieceIfBoolIsTrue(true, mostPowerfullMove.moves);
            //move the piece to the best move position
            mostPowerfullMove.chesspiece.chessPieceGameObject.transform.position = (Vector2)mostPowerfullMove.moves;
            Debug.Log(mostPowerfullMove.points);
        }
        //if we dont have a killable piece (points different equal to 0)
        else
        {
            Debug.Log("Theres NO move capable of killing");
            
        }
 
    }*/

    //this is going to count how many times the loop runs
    static int bigOCounterOfOneDephtMove = 0;

    //Check my move then the player and then decide if that move is good, one depth defensive play
    public static void OneDephtDefensiveMoveCheckMyMoveThenPlayerMove(BoardConstructor bCScript)
    {
        //this is going to create a move storer that is going to store every info about the most powerful move
        AllMovesStorer mostPowerfullMove = new AllMovesStorer();
        mostPowerfullMove.points = -999;

        //this will store where the first ai chess piece is , i need this so that i can reduce the loop amounts
        int IndexOfcurrenStartOfAiPieces = bCScript.ReturnIndexOfCurrenStartOfAiPieces();
        //if the ai piece returned from the current start of ai pices, isnt a ai is becaus ethat function is bugging, so we return a error
        if (bCScript.chessPieces[IndexOfcurrenStartOfAiPieces].isAi == false)
        {
            Debug.LogError("THE FUNCTION ReturnIndexOfCurrenStartOfAiPieces(), RETURNED A VALUE THAT ISNT A AI PIECE, IT SHOULD RETURN THE FIRST AI PIECE");
        }

        //go through every AI chess piece
        for (int i = IndexOfcurrenStartOfAiPieces; i < bCScript.chessPieces.Count; i++)
        {
            //create a temporary possible moves of the current piece
            List<Vector2Int> tempPossibleMovesOfThisPiece = new List<Vector2Int>();

            //this will add every possible move of the current chess piece
            tempPossibleMovesOfThisPiece.AddRange(bCScript.chessPieces[i].ReturnPossibleMoves(new Vector2Int((int)bCScript.chessPieces[i].chessPieceGameObject.transform.position.x,
                                                           (int)bCScript.chessPieces[i].chessPieceGameObject.transform.position.y),
                                                           bCScript.chessPieces));

            //this ism gonna store the best move of this ai piece
            AllMovesStorer bestMoveOfThisPiece = GoTroughEveryAiMoveAndReturnBestOutcome(IndexOfcurrenStartOfAiPieces, bCScript, tempPossibleMovesOfThisPiece, i);

            //if the best move of this piece is better then the current most powerfull move, store the data on most powerfull move
            if (bestMoveOfThisPiece.points > mostPowerfullMove.points)
            {
                mostPowerfullMove.chesspiece = bestMoveOfThisPiece.chesspiece;
                mostPowerfullMove.moves = bestMoveOfThisPiece.moves;
                mostPowerfullMove.points = bestMoveOfThisPiece.points;
            }
        }
        //if the current move points is initialized, this means everything work out great, and we move to that move
        if (mostPowerfullMove.points != -999)
        {
            //kill the piece at the move position
            bCScript.KillPieceIfBoolIsTrue(true, mostPowerfullMove.moves);
            //move the piece to the best move position
            mostPowerfullMove.chesspiece.chessPieceGameObject.transform.position = (Vector2)mostPowerfullMove.moves;
            Debug.Log("this move points is: " + mostPowerfullMove.points);
        }
        //if most powerfull move isnt intialized, is because we made an error
        else
        {
            Debug.Log("theres no move intitialized, ERROR");
        }
        Debug.Log("the big o of this move is " + bigOCounterOfOneDephtMove);
        //since we stoped the one depth search we can stop the counter
        bigOCounterOfOneDephtMove = 0;
    }



    //Check my move then the player and then decide if that move is good, one depth normal play
    public static void OneDephtNormalMoveCheckMyMoveThenPlayerMove(BoardConstructor bCScript)
    {
        //this is going to create a move storer that is going to store every info about the most powerful move
        AllMovesStorer mostPowerfullMove = new AllMovesStorer();
        mostPowerfullMove.points = -999;

        //this will have the list with all 0 moves
        List<AllMovesStorer> movesWith0Points = new List<AllMovesStorer>();

        //this will store where the first ai chess piece is , i need this so that i can reduce the loop amounts
        int IndexOfcurrenStartOfAiPieces = bCScript.ReturnIndexOfCurrenStartOfAiPieces();
        //if the ai piece returned from the current start of ai pices, isnt a ai is becaus ethat function is bugging, so we return a error
        if (bCScript.chessPieces[IndexOfcurrenStartOfAiPieces].isAi == false)
        {
            Debug.LogError("THE FUNCTION ReturnIndexOfCurrenStartOfAiPieces(), RETURNED A VALUE THAT ISNT A AI PIECE, IT SHOULD RETURN THE FIRST AI PIECE");
        }

        //go through every AI chess piece
        for (int i = IndexOfcurrenStartOfAiPieces; i < bCScript.chessPieces.Count; i++)
        {
            //create a temporary possible moves of the current piece
            List<Vector2Int> tempPossibleMovesOfThisPiece = new List<Vector2Int>();

            //this will add every possible move of the current chess piece
            tempPossibleMovesOfThisPiece.AddRange(bCScript.chessPieces[i].ReturnPossibleMoves(new Vector2Int((int)bCScript.chessPieces[i].chessPieceGameObject.transform.position.x,
                                                           (int)bCScript.chessPieces[i].chessPieceGameObject.transform.position.y),
                                                           bCScript.chessPieces));

            //this ism gonna store the best move of this ai piece
            AllMovesStorer bestMoveOfThisPiece = GoTroughEveryAiMoveAndReturnBestOutcome(IndexOfcurrenStartOfAiPieces, bCScript, tempPossibleMovesOfThisPiece, i);

            //if the best move of this piece is better then the current most powerfull move, store the data on most powerfull move
            if (bestMoveOfThisPiece.points > mostPowerfullMove.points)
            {
                mostPowerfullMove.chesspiece = bestMoveOfThisPiece.chesspiece;
                mostPowerfullMove.moves = bestMoveOfThisPiece.moves;
                mostPowerfullMove.points = bestMoveOfThisPiece.points;
            }
            //if the current move has 0 points, and the most powerfull move is 0 aswell, add it to the 0 points list
            if (bestMoveOfThisPiece.points == 0 && mostPowerfullMove.points == 0)
            {
                movesWith0Points.Add(bestMoveOfThisPiece);
            }
        }
        //if the current move points is initialized, this means everything work out great, and we move to that move
        if (mostPowerfullMove.points != -999)
        {
            //if the move doesnt have 0 points, move the most powerfull move
            if (mostPowerfullMove.points != 0)
            {
                //kill the piece at the move position
                bCScript.KillPieceIfBoolIsTrue(true, mostPowerfullMove.moves);
                //move the piece to the best move position
                mostPowerfullMove.chesspiece.chessPieceGameObject.transform.position = (Vector2)mostPowerfullMove.moves;
            }
            //if the move has 0 points, move a random move with 0 points
            else
            {
                //this will store a random index for the moves with 0 points
                int i = Random.Range(0, movesWith0Points.Count - 1);
                //kill the piece at the move position
                bCScript.KillPieceIfBoolIsTrue(true, movesWith0Points[i].moves);
                //move the piece to the random 0 ponints position
                movesWith0Points[i].chesspiece.chessPieceGameObject.transform.position = (Vector2)movesWith0Points[i].moves;
            }
            Debug.Log("this move points is: " + mostPowerfullMove.points);
        }
        //if most powerfull move isnt intialized, is because we made an error
        else
        {
            Debug.Log("theres no move intitialized, ERROR");
        }
        Debug.Log("the big o of this move is " + bigOCounterOfOneDephtMove);
        //since we stoped the one depth search we can stop the counter
        bigOCounterOfOneDephtMove = 0;
    }

    //this will go trough every ai move and return the best outcome, it will call the GoTroughEveryPlayerMoveAndReturnWorseOutcome loop aswell
    static AllMovesStorer GoTroughEveryAiMoveAndReturnBestOutcome(int IndexOfcurrenStartOfAiPieces, BoardConstructor bCScript, List<Vector2Int> tempPossibleMovesOfThisPiece, int currentPiece)
    {
        //this is going to create a move storer that is going to store every info about the most powerful move
        AllMovesStorer currentBestAiMove = new AllMovesStorer();
        currentBestAiMove.points = -999;

        //this is going to store the old position, since we want to move the piece to be able to search for the future, with the ai pice moved, by the current ai piece move
        Vector2 oldPos = bCScript.chessPieces[currentPiece].chessPieceGameObject.transform.position;
        //this foreach is gonna go throught every possible move of the current aichesspiece
        for (int i_ = 0; i_ < tempPossibleMovesOfThisPiece.Count; i_++)
        {
            //this is going to store the points of the current move (from 0 to 900)
            int pointsOfCurrentMove = bCScript.KillPieceIfBoolIsTrue(false, tempPossibleMovesOfThisPiece[i_]);
            //if the move can kill any piece, we move it away so that in the next search we can check the player moves without this piece
            int pieceToKillOnThisMove = bCScript.FakeKillPieceIfBoolIsTrue(tempPossibleMovesOfThisPiece[i_]);

            //this is going to store the possible kill player chesspiece position of this move
            Vector2 oldPlayerChessPiecePosition = new Vector2(-1, -1);
            //if theres a kill we move the piece out of the board, so that it doesnt affect the the future search
            if (pieceToKillOnThisMove != -99)
            {
                //store the old position of this player piece (this piece is killed on this move, if its a good move)
                oldPlayerChessPiecePosition = bCScript.chessPieces[pieceToKillOnThisMove].chessPieceGameObject.gameObject.transform.position;
                //move the player piece out of the way so that the GoTroughEveryPlayerMoveAndReturnWorseOutcome loop can check without this piece in front, since in this move we suposed to kill this piece
                bCScript.chessPieces[pieceToKillOnThisMove].chessPieceGameObject.gameObject.transform.position = new Vector2(-1, -1);
            }

            //this will move the ai piece to the 
            bCScript.chessPieces[currentPiece].chessPieceGameObject.transform.position = new Vector2(tempPossibleMovesOfThisPiece[i_].x, tempPossibleMovesOfThisPiece[i_].y);

            //this will get the current worse outcome possible for the current move
            int worsePlayerMoveForCurrentMove = GoTroughEveryPlayerMoveAndReturnWorseOutcome(IndexOfcurrenStartOfAiPieces, bCScript, currentBestAiMove.points, pointsOfCurrentMove);

            pointsOfCurrentMove = pointsOfCurrentMove - worsePlayerMoveForCurrentMove;

            if (pointsOfCurrentMove > currentBestAiMove.points)
            {
                currentBestAiMove.chesspiece = bCScript.chessPieces[currentPiece];
                currentBestAiMove.moves = tempPossibleMovesOfThisPiece[i_];
                currentBestAiMove.points = pointsOfCurrentMove;
            }
            //if this move kills someone we want to move the player piece back to the initial position, since we moved it away before checking the player moves, so that it can check without this piece in front
            if (pieceToKillOnThisMove != -99)
            {
                bCScript.chessPieces[pieceToKillOnThisMove].chessPieceGameObject.gameObject.transform.position = oldPlayerChessPiecePosition;
            }
        }
        //this will reset the old position since we moved the peace for us to be able to see in the future
        bCScript.chessPieces[currentPiece].chessPieceGameObject.transform.position = oldPos;
        return currentBestAiMove;

    }

    //this will go trough every player piece and return the worse outcome, this function is called from the GoTroughEveryAiMoveAndReturnBestOutcome loop for example
    static int GoTroughEveryPlayerMoveAndReturnWorseOutcome(int IndexOfcurrenStartOfAiPieces, BoardConstructor bCScript, int currentBestMoveOfAi, int pointsOfCurrentAiMove)
    {
        //this will store the worst possible move 
        int worseMove = 0;

        //go trough every PLAYER piece
        for (int x = 0; x < IndexOfcurrenStartOfAiPieces; x++)
        {
            //create a temporary possible moves of the current ai piece
            List<Vector2Int> tempPossibleMovesOfThisPlayerPiece = new List<Vector2Int>();

            //this will add every possible move of the current chess piece
            tempPossibleMovesOfThisPlayerPiece.AddRange(bCScript.chessPieces[x].ReturnPossibleMoves(new Vector2Int((int)bCScript.chessPieces[x].chessPieceGameObject.transform.position.x,
                                                   (int)bCScript.chessPieces[x].chessPieceGameObject.transform.position.y),
                                                   bCScript.chessPieces));

            //store the old player piece position
            Vector2 oldPlayerPos = bCScript.chessPieces[x].chessPieceGameObject.transform.position;

            //this for is gonna go trhough every possible move of the current playerChessPiece
            for (int x_ = 0; x_ < tempPossibleMovesOfThisPlayerPiece.Count; x_++)
            {
                //this is gonna store the points of the current player move (one depht, seing the future)
                int pointsOfCurrentPlayerMove = bCScript.KillPieceIfBoolIsTrue(false, tempPossibleMovesOfThisPlayerPiece[x_]);

                //if this move is worse then the last one, return -999 Alpha beta Pruning
                if ((pointsOfCurrentAiMove - pointsOfCurrentPlayerMove) < currentBestMoveOfAi)
                {
                    return 999;
                }

                //if the current move is worse then the previos worse move store this one instead
                if (pointsOfCurrentPlayerMove > worseMove)
                {
                    worseMove = pointsOfCurrentPlayerMove;
                }

                //increase the big o counter
                bigOCounterOfOneDephtMove += 1;
            }

        }
        //Debug.Log("Worse Player Move " + (-worseMove));
        return worseMove;
    }
}