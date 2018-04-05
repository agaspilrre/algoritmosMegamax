using System;
using UnityEngine;

public class Board : MonoBehaviour
{

    public string[,] spaces; 
    public string activePlayer;

    public int winningPositions;

    public byte rows, columns;

    private byte[,] evaluationMatrix;

   
    public int hashValue;

    public Board(byte _rows, byte _columns)
    {
        rows = _rows;
        columns = _columns;
        spaces = new string[rows, columns];
       
    }

    public int Evaluate(string player)
    {
        EvaluationMatrixMannager evMatrixCross = GameObject.Find("EvaluationMManager").GetComponent<EvaluationMatrixMannager>();
        //si puede ganar el jugador
        if (IsWinningPosition(player))
        {
            return 1000;
        }
        //si gana la IA
        if (IsWinningPosition(Opponent(player)))
        {
            return -1000;
        }
        //si existe empate
        if (IsBoardFull())
        {
            return 0;
        }

        
        int evaluationSum = 0;
       
        

        //recorro el tablero
        //voy obteniendo los valores de la matriz de evaluacion de todas las casillas del tablero para que la IA planifique la mejor jugada.
                for (byte row = 0; row < rows; row++)
                {
                    for (byte column = 0; column < columns; column++)
                    {

                        
                        if (spaces[row, column] == player)
                        {
                            
                            evaluationSum += evMatrixCross.getValueEvaluationMatrix(row, column);
                        }
                        
                        else if (spaces[row, column] == Opponent(player))
                        {
                            
                            evaluationSum -= evMatrixCross.getValueEvaluationMatrix(row, column);
                        }
                    }
                }
                return evaluationSum;
    }

    public int[,] PossibleMoves()
    {
        int[,] moves;
        int count = 0;

        //pasamos por todas las filas y columnas y contamos el numero de espacios vacios
        for (byte row = 0; row < rows; row++)
        {
            for(byte column = 0; column < columns; column++)
            {
                if (IsEmptySpace(row, column))
                {
                    count++;                   
                }
            }
        }

        //creamos un array bidimensional en el que almacenar columna y fila, tantos como espacios vacios habia
        moves = new int[count, 2];

        count = 0;

        //rellenamos el array con las posiciones vacias
        for (byte row = 0; row < rows; row++)
        {
            for (byte column = 0; column < columns; column++)
            {
                if (IsEmptySpace(row, column))
                {
                    moves[count, 0] = row;
                    moves[count, 1] = column;
                    count++;
                }
            }
        }
        return moves;
    }

   

    string Opponent (string player)
    {
        if (player == "X")
        {
            return "O";
        }
        else
        {
            return "X";
        }
    }

    public bool IsEndOfGame()
    {
        if (IsWinningPosition("X"))
        {
            return true;
        }
        else if (IsWinningPosition("O"))
        {
            return true;
        }
        else if (IsBoardFull())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //comprueba si hay posicion ganadora llamando a la funcion es cruz ganadora
    public bool IsWinningPosition (string player)
    {
        if (IsCrosWinning(player) == 1) return true;
       
        return false;
    }
   
    public bool Draw(string player)
    {
        winningPositions = 0;

        for (byte row = 0; row < rows; row++)
        {
            for (byte column = 0; column < columns; column++)
            {
                if ((column == 0 || spaces[row, column - 1] == player) &&
                     (column == spaces.GetLength(1) - 1 || spaces[row, column + 1] == player) &&
                     (row == spaces.GetLength(0) - 1 || spaces[row + 1, column] == player) &&
                     (row == 0 || spaces[row - 1, column] == player) &&
                     (spaces[row, column] != "") &&
                     (spaces[row, column] != player))
                {
                    winningPositions++;
                }
                if ((column == 0 || spaces[row, column - 1] == Opponent(player)) &&
                     (column == spaces.GetLength(1) - 1 || spaces[row, column + 1] == Opponent(player)) &&
                     (row == spaces.GetLength(0) - 1 || spaces[row + 1, column] == Opponent(player)) &&
                     (row == 0 || spaces[row - 1, column] == Opponent(player)) &&
                     (spaces[row, column] != "") &&
                     (spaces[row, column] != Opponent(player)))
                {
                    winningPositions++;
                }
            }
        }

        print(winningPositions);
        if (winningPositions == 2)
            return true;
        else
            return false;
    }

    //comprueba si  hace una cruz 
    protected int IsCrosWinning(string player)
    {
        winningPositions = 0;

        for (byte row = 0; row < rows; row++)
        {
            for (byte column = 0; column < columns; column++)
            {
                if ((column == 0 || spaces[row, column - 1] == player) &&
                     (column == spaces.GetLength(1) - 1 || spaces[row, column + 1] == player) &&
                     (row == spaces.GetLength(0) - 1 || spaces[row + 1, column] == player) &&
                     (row == 0 || spaces[row - 1, column] == player) &&
                     (spaces[row, column] != "") &&
                     (spaces[row, column] != player))
                {
                    winningPositions++;
                }
            }
        }
        return winningPositions;
    }
 


    protected bool IsBoardFull()
    {
        if (winningPositions == 2)
            return true;

        for (byte row = 0; row < columns; row++)
        {
            for (byte column = 0; column < columns; column++)
            {
                if (IsEmptySpace(row, column)) return false;
            }
        }
        return true;
    }

    public Board GenerateNewBoardFromMove(int row, int column)
    {
        Board newBoard = this.DuplicateBoard();
        newBoard.Move(row, column, activePlayer);
        newBoard.activePlayer = Opponent(newBoard.activePlayer);
        return newBoard;
    }



    public Board DuplicateBoard ()
    {
        Board newBoard = new Board(rows, columns);
        for (byte row = 0; row < rows; row++)
        {
            for (byte column = 0; column < columns; column++)
            {
                newBoard.spaces[row, column] = this.spaces[row, column];
            }
        }
        newBoard.activePlayer = this.activePlayer;
        return newBoard;
    }

    public bool IsEmptySpace(int row, int column)
    {
        if (spaces[row, column] == "") return true;
        else return false;
    }
    public void Move(int row, int column, string player)
    {
        if (IsEmptySpace(row, column))
        {
            spaces[row, column] = player;
        }       
    }
   
}
