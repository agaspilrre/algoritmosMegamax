using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase encargada de gestionar los valores de la matriz de evaluacion
public class EvaluationMatrixMannager : MonoBehaviour {

    


    public byte rows, columns;
    private byte[,] originalMatrixValue;
    private byte[,] evaluationMatrixPlayer;
    private bool[,] occupation;
  
    //metodo inicial o constructor
    public void u(byte _rows, byte _columns)
    {

       
        rows = _rows;
        columns = _columns;
        
        //matriz el cual la IA va examinar para obtener sus valores
        evaluationMatrixPlayer = new byte[rows, columns];
        //matriz de evaluacion que siempre guarda los valores inciales
        originalMatrixValue = new byte[rows, columns];



        //matriz para comprobar si existen contiguas
        occupation = new bool[rows, columns];

        //inicializamos todas las matrices
        //inicialmente a las esquinas del tablero se le dara un valor bajo de 1 para que la IA no vaya a los sitios q es mas facil de cerrar
        //los margenes de tablero se le asignara un dos, ya que es necesario 3 movimientos para cerrar a la IA por lo que
        //interesa mas que colocar en las esquinas pero seguira siendo mas favorable para ella colocar fichas en el centro del tablero
        //el resto de casillas del tablero tendra el valor mas alto un 3.
        for (byte row = 0; row < rows; row++)
        {
            for (byte column = 0; column < columns; column++)
            {
                //esquina superior derecha
                if (row == 0 && column == 0)
                {
                    evaluationMatrixPlayer[row, column] = 1;
                    originalMatrixValue[row, column] = 1;

                }
                //esquina superior izquierda
                else if (row == 0 && column == columns - 1)
                {
                    evaluationMatrixPlayer[row, column] = 1;
                    originalMatrixValue[row, column] = 1;
                }
                //esquina inferior derecha
                else if (row == rows - 1 && column == 0)
                {
                    evaluationMatrixPlayer[row, column] = 1;
                    originalMatrixValue[row, column] = 1;
                }
                //esquina inferior izquierda
                else if (row == rows - 1 && column == columns - 1)
                {
                    evaluationMatrixPlayer[row, column] = 1;
                    originalMatrixValue[row, column] = 1;
                }
                //primera fila
                else if (row == 0 && column != 0 && column != columns - 1)
                {
                    evaluationMatrixPlayer[row, column] = 2;
                    originalMatrixValue[row, column] = 2;
                }
                //ultima fila
                else if (row == rows - 1 && column != 0 && column != columns - 1)
                {
                    evaluationMatrixPlayer[row, column] = 2;
                    originalMatrixValue[row, column] = 2;
                }
                //primera columna
                else if (column == 0 && row != 0 && row != rows - 1)
                {
                    evaluationMatrixPlayer[row, column] = 2;
                    originalMatrixValue[row, column] = 2;
                }
                //ultima columna
                else if (column == columns - 1 && row != 0 && row != rows - 1)
                {
                    evaluationMatrixPlayer[row, column] = 2;
                    originalMatrixValue[row, column] = 2;
                }

                //resto de casos
                else
                {
                    evaluationMatrixPlayer[row, column] = 3;
                    originalMatrixValue[row, column] = 3;
                }

                //como no hay ninguna casilla ocupada ya que es el inicio inicializamos matriz de ocupacion a false.
                occupation[row, column] = false;
            }
        }
    }

    //devuelve el valor que guarda la matriz de evaluacion en la casilla especificada
    public byte getValueEvaluationMatrix(byte _row, byte _column)
    {
        return evaluationMatrixPlayer[_row, _column];
    }

    //establece a true en la matriz de evaluacion la casilla que le indiquemos 
    public void setOccupationValue(byte _row, byte _column)
    {
        occupation[_row, _column] = true;
    }

    //devuelve el valor de la matriz de ocupacion en la casilla que le indiquemos.
    public bool getOccupationValue(byte _row, byte _column)
    {
        return occupation[_row, _column];
    }

    //funcion que va modificando los valores de la matriz de evaluacion cada vez que el player pone una ficha
    //si ponemos una ficha en la esquina las dos casillas inmediatas que pueden cerrar a la ficha que acabamos de colocar
    //pasaran a obtener un valor de 12 para que la IA sepa que tiene que ir a atacarnos
    //si ponemos una ficha en los margenes del tablero a sus tres casillas inmediatas se le dara un valor de 11
    //si ponemos una ficha en el resto de casillas se le otorgara una valor de 10 a sus 4 casillas inmediatas
    //antes de establecer estos valores cuando se coloca la ficha se comprueba que en sus casillas inmediatas no haya otra ficha del player
    //esta situacion hace que la IA no pueda cerrar ninguna de estas fichas y por lo tanto las casillas inmediatas de las dos fichas vuelven
    //ha adquirir su valor inicial gracias a la matriz de evaluacion original
    public void changeEvaluationMatrixPlayer(byte _row, byte _column)
    {
        
        //esquina superior derecha
        if(_row==0 && _column == 0)
        {
            //si hay alguna consecutiva donde se ha colocado se mantiene el valor original
            if(occupation[_row,_column+1]==true || occupation[_row+1, _column] == true)
            {
                evaluationMatrixPlayer[_row, _column + 1] = originalMatrixValue[_row, _column + 1];
                evaluationMatrixPlayer[_row + 1, _column] = originalMatrixValue[_row+1, _column ];
            }

            //si no se le añade mas valor para que la IA vaya a cerrar esa ficha
            else
            {
                evaluationMatrixPlayer[_row, _column + 1] = 12;
                evaluationMatrixPlayer[_row + 1, _column] = 12;
            }
            

        }
        //esquina superior izquierda
        else if(_row==0 && _column == columns-1)
        {
            if (occupation[_row, _column -1] == true || occupation[_row + 1, _column] == true)
            {
                evaluationMatrixPlayer[_row, _column -1] = originalMatrixValue[_row, _column -1];
                evaluationMatrixPlayer[_row + 1, _column] = originalMatrixValue[_row + 1, _column];
            }
            else
            {
                evaluationMatrixPlayer[_row, _column - 1] = 12;
                evaluationMatrixPlayer[_row + 1, _column] = 12;
            }
            
        }
        //esquina inferior derecha
        else if(_row==rows-1 && _column == 0)
        {

            if (occupation[_row, _column + 1] == true || occupation[_row -1, _column] == true)
            {
                evaluationMatrixPlayer[_row, _column + 1] = originalMatrixValue[_row, _column + 1];
                evaluationMatrixPlayer[_row - 1, _column] = originalMatrixValue[_row - 1, _column];
            }
            else
            {
                evaluationMatrixPlayer[_row, _column + 1] = 12;
                evaluationMatrixPlayer[_row - 1, _column] = 12;
            }
           
        }
        //esquina inferior izquierda
        else if(_row==rows-1 && _column == columns-1)
        {
            if (occupation[_row, _column -1] == true || occupation[_row - 1, _column] == true)
            {
                evaluationMatrixPlayer[_row, _column - 1] = originalMatrixValue[_row, _column - 1];
                evaluationMatrixPlayer[_row - 1, _column] = originalMatrixValue[_row - 1, _column];
            }
            else
            {
                evaluationMatrixPlayer[_row, _column - 1] = 12;
                evaluationMatrixPlayer[_row - 1, _column] = 12;
            }
            
        }
        //primera fila
        else if(_row==0 && _column!=0 && _column != columns-1)
        {
            if(occupation[_row,_column+1]==true || occupation[_row, _column-1] == true || occupation[_row+1, _column] == true)
            {
                evaluationMatrixPlayer[_row, _column + 1] = originalMatrixValue[_row,_column+1];
                evaluationMatrixPlayer[_row, _column - 1] = originalMatrixValue[_row, _column -1];

                evaluationMatrixPlayer[_row + 1, _column] = originalMatrixValue[_row+1, _column ];
            }
            else
            {
                evaluationMatrixPlayer[_row, _column + 1] = 11;
                evaluationMatrixPlayer[_row, _column - 1] = 11;

                evaluationMatrixPlayer[_row + 1, _column] = 11;
            }
            
        }
        //ultima fila
        else if (_row == rows-1 && _column != 0 && _column != columns-1)
        {
            if (occupation[_row, _column + 1] == true || occupation[_row, _column - 1] == true || occupation[_row - 1, _column] == true)
            {
                evaluationMatrixPlayer[_row, _column + 1] = originalMatrixValue[_row, _column + 1];
                evaluationMatrixPlayer[_row, _column - 1] = originalMatrixValue[_row, _column - 1];

                evaluationMatrixPlayer[_row - 1, _column] = originalMatrixValue[_row - 1, _column];
            }
            else
            {
                evaluationMatrixPlayer[_row, _column + 1] = 11;
                evaluationMatrixPlayer[_row, _column - 1] = 11;

                evaluationMatrixPlayer[_row - 1, _column] = 11;
            }
            
        }
        //primera columna
        else if (_column == 0 && _row!=0 && _row!=rows-1)
        {
            if (occupation[_row+1, _column ] == true || occupation[_row-1, _column] == true || occupation[_row , _column+1] == true)
            {
                evaluationMatrixPlayer[_row+1, _column ] = originalMatrixValue[_row+1, _column ];
                evaluationMatrixPlayer[_row-1, _column ] = originalMatrixValue[_row-1, _column ];

                evaluationMatrixPlayer[_row , _column+1] = originalMatrixValue[_row , _column+1];
            }
            else
            {
                evaluationMatrixPlayer[_row + 1, _column] = 11;
                evaluationMatrixPlayer[_row - 1, _column] = 11;

                evaluationMatrixPlayer[_row, _column + 1] = 11;
            }
           
        }
        //ultima columna
        else if (_column == columns-1 && _row != 0 && _row != rows-1)
        {
            if (occupation[_row + 1, _column] == true || occupation[_row - 1, _column] == true || occupation[_row, _column - 1] == true)
            {
                evaluationMatrixPlayer[_row + 1, _column] = originalMatrixValue[_row + 1, _column];
                evaluationMatrixPlayer[_row - 1, _column] = originalMatrixValue[_row - 1, _column];

                evaluationMatrixPlayer[_row, _column - 1] = originalMatrixValue[_row, _column - 1];
            }
            else
            {
                evaluationMatrixPlayer[_row + 1, _column] = 11;
                evaluationMatrixPlayer[_row - 1, _column] = 11;

                evaluationMatrixPlayer[_row, _column - 1] = 11;
            }
            
        }

        //resto de casos
        else
        {
            if(occupation[_row+1,_column]==true )
            {
                evaluationMatrixPlayer[_row + 1, _column] = originalMatrixValue[_row+1,_column];
                evaluationMatrixPlayer[_row - 1, _column] = originalMatrixValue[_row-1,_column];
                evaluationMatrixPlayer[_row, _column + 1] = originalMatrixValue[_row,_column+1];
                evaluationMatrixPlayer[_row, _column - 1] = originalMatrixValue[_row,_column-1];

                evaluationMatrixPlayer[_row , _column] = originalMatrixValue[_row , _column];
                //comprobacion para que no exceda de rango la matriz
                if (_row + 1 != rows - 1)
                {
                    evaluationMatrixPlayer[_row + 2, _column] = originalMatrixValue[_row + 2, _column];
                }
                
                evaluationMatrixPlayer[_row+1, _column + 1] = originalMatrixValue[_row+1, _column + 1];
                evaluationMatrixPlayer[_row+1, _column - 1] = originalMatrixValue[_row+1, _column - 1];

            }

            else if(occupation[_row - 1, _column] == true)
            {
                evaluationMatrixPlayer[_row + 1, _column] = originalMatrixValue[_row + 1, _column];
                evaluationMatrixPlayer[_row - 1, _column] = originalMatrixValue[_row - 1, _column];
                evaluationMatrixPlayer[_row, _column + 1] = originalMatrixValue[_row, _column + 1];
                evaluationMatrixPlayer[_row, _column - 1] = originalMatrixValue[_row, _column - 1];

                evaluationMatrixPlayer[_row, _column] = originalMatrixValue[_row, _column];
                if (_row - 1 != 0)
                {
                    evaluationMatrixPlayer[_row - 2, _column] = originalMatrixValue[_row - 2, _column];
                }
                
                evaluationMatrixPlayer[_row - 1, _column + 1] = originalMatrixValue[_row-1, _column + 1];
                evaluationMatrixPlayer[_row - 1, _column - 1] = originalMatrixValue[_row-1, _column - 1];
            }

            else if(occupation[_row, _column + 1] == true)
            {

                evaluationMatrixPlayer[_row + 1, _column] = originalMatrixValue[_row + 1, _column];
                evaluationMatrixPlayer[_row - 1, _column] = originalMatrixValue[_row - 1, _column];
                evaluationMatrixPlayer[_row, _column + 1] = originalMatrixValue[_row, _column + 1];
                evaluationMatrixPlayer[_row, _column - 1] = originalMatrixValue[_row, _column - 1];

                evaluationMatrixPlayer[_row, _column] = originalMatrixValue[_row, _column];
                if (_column + 1 != columns - 1)
                {
                    evaluationMatrixPlayer[_row, _column + 2] = originalMatrixValue[_row, _column + 2];
                }
                
                evaluationMatrixPlayer[_row - 1, _column + 1] = originalMatrixValue[_row-1, _column + 1];
                evaluationMatrixPlayer[_row +1, _column + 1] = originalMatrixValue[_row+1, _column + 1];

            }

            else if(occupation[_row, _column - 1] == true)
            {
                evaluationMatrixPlayer[_row + 1, _column] = originalMatrixValue[_row + 1, _column];
                evaluationMatrixPlayer[_row - 1, _column] = originalMatrixValue[_row - 1, _column];
                evaluationMatrixPlayer[_row, _column + 1] = originalMatrixValue[_row, _column + 1];
                evaluationMatrixPlayer[_row, _column - 1] = originalMatrixValue[_row, _column - 1];

                evaluationMatrixPlayer[_row, _column] = originalMatrixValue[_row, _column];
                if (_column - 1 != 0)
                {
                    evaluationMatrixPlayer[_row, _column - 2] = originalMatrixValue[_row, _column - 2];
                }
                
                evaluationMatrixPlayer[_row - 1, _column - 1] = originalMatrixValue[_row - 1, _column - 1];
                evaluationMatrixPlayer[_row + 1, _column - 1] = originalMatrixValue[_row + 1, _column - 1];
            }

            else
            {
                evaluationMatrixPlayer[_row + 1, _column] = 10;
                evaluationMatrixPlayer[_row - 1, _column] = 10;
                evaluationMatrixPlayer[_row, _column + 1] = 10;
                evaluationMatrixPlayer[_row, _column - 1] = 10;
            }
            
        }

       
    }

    //funcion que resetea todas las matrices de esta clase a sus valores iniciales
    //para cuando repetimos partida
    public void resetMatrix()
    {
        for (byte row = 0; row < rows; row++)
        {
            for (byte column = 0; column < columns; column++)
            {
                occupation[row, column] = false;
                evaluationMatrixPlayer[row, column] = originalMatrixValue[row, column];
            }

        }

    }

    
   
}
