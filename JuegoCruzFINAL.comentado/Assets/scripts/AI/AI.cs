using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AI : MonoBehaviour
{
    public byte MAX_DEPTH = 15;

    public Text[,] buttonList;
    public GameController gameController;

    private const int INFINITE = 10000;
    private const int MINUS_INFINITE = -10000;

    private Board board;
    private string activePlayer;

    public Text moveText, scoreText, timeText;

  

    private int previousScore = INFINITE;
    public int windowRange = 5;


    public Text aspirationPathText;


    // PARA FUNCIONAMIENTO CON HASH
   
    [SerializeField]
    private int hashTableLength = 90000000;
    private int maximumExploredDepth = 0;
    private int globalGuess = INFINITE;
    [SerializeField]
    private int MAX_ITERATIONS = 10;
    public Text MTDPathText;

 
    
 
    public void SetButtonList(Text[,] bl)
    {
        buttonList = bl;
    }

    public void resetPreviousScore()
    {
        previousScore = MINUS_INFINITE;
    }

    public void SetGameController(GameController gc)
    {
        gameController = gc;
    }

    public void Play(string actPlayer)
    {
        ScoringMove move;
        activePlayer = actPlayer;
        ObserveBoard();

      

        
        DateTime DateBefore = DateTime.Now;
        move = AspirationSearch(board);
        DateTime DateAfter = DateTime.Now;
        moveText.text += " // " + "Row: " + move.row + "Column: " + move.row;
        scoreText.text += " // " + move.score;
        timeText.text += "\n // " + (DateAfter - DateBefore).TotalSeconds;
        
        //Debug.Log("Jugador Activo:" + activePlayer + " Jugada Elegida:" + move.move + "/" + move.score);

        Move(move);
    }

    void ObserveBoard()
    {
        board = new Board(gameController.rows, gameController.columns);
        byte rows = gameController.rows;
        byte columns = gameController.columns;

        for (byte row = 0; row < rows; row++)
        {
            for (byte column = 0; column < columns; column++)
            {
                Text spaceText = buttonList[row, column];
                board.spaces[row, column] = spaceText.text;
            }
        }
        board.activePlayer = this.activePlayer;
        //board.CalculateHashValue();
    }

    void Move (ScoringMove scoringMove)
    {
        gameController.FillColumn((byte)scoringMove.row, (byte)scoringMove.column);
        gameController.EndTurn();
    }

    ScoringMove NegamaxAB(Board board, byte depth,int alfa, int beta)
    {

        // Devuelve el score del tablero y la jugada con la que se llega a él.
        int[] bestMove = new int[2];
        int bestScore = 0;
        int currentScore;
        ScoringMove scoringMove; // score, movimiento
        Board newBoard;
        // Comprobar si hemos terminado de hacer recursión
        if (board.IsEndOfGame() || depth == MAX_DEPTH)
        {
            if (depth % 2 == 0)
            {

                scoringMove = new ScoringMove(board.Evaluate(activePlayer), 0, 0);
            }
            else
            {
                scoringMove = new ScoringMove(-board.Evaluate(activePlayer), 0, 0);
            }
        }
        else
        {
            bestScore = MINUS_INFINITE;

            int[,] possibleMoves;
            possibleMoves = board.PossibleMoves();

            //foreach (int move in possibleMoves)

            for(int move = 0; move < possibleMoves.GetLength(0); move++)
            {
                newBoard = board.GenerateNewBoardFromMove(possibleMoves[move, 0], possibleMoves[move, 1]);

                // Recursividad
                scoringMove = NegamaxAB(newBoard, (byte)(depth + 1),-beta,-Math.Max(alfa,bestScore));

                int invertedScore = -scoringMove.score;

                // Actualizar mejor score
                if (invertedScore > bestScore)
                {
                    bestScore = invertedScore;
                    bestMove[0] = possibleMoves[move, 0];
                    bestMove[1] = possibleMoves[move, 1];
                }

                if (bestScore >= beta)
                {
                    scoringMove = new ScoringMove(bestScore, bestMove[0], bestMove[1]);
                    return scoringMove;
                }
            }
            scoringMove = new ScoringMove(bestScore, bestMove[0], bestMove[1]);
        }
        return scoringMove;

    }

    ScoringMove AspirationSearch(Board board)
    {
        int alfa, beta;
        ScoringMove move;
        string aspirationPath="";
        

        if(previousScore!= MINUS_INFINITE)
        {
            alfa = previousScore - windowRange;
            beta = previousScore + windowRange;
            while (true)
            {
                move = NegamaxAB(board, 0, alfa, beta);
                if (move.score <= alfa)
                {
                    aspirationPath += "fails soft alfa.";
                    alfa = MINUS_INFINITE;
                }
                else if (move.score >= beta)
                {
                    aspirationPath += "fails soft beta.";
                    beta = INFINITE;
                }
                else
                {
                    aspirationPath += "Succes";
                    break;
                }             
            }
        }

        else
        {
            aspirationPath += "Normal Negamax";
            move = NegamaxAB(board, 0, MINUS_INFINITE, INFINITE);
            
        }

        aspirationPathText.text = aspirationPath;
        previousScore = move.score;
        return move;
    }

   

}