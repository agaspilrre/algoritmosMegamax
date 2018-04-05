using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player
{
    public Image panel;
    public Text text;
    public Button button;
}

[System.Serializable]
public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
}

public class GameController : MonoBehaviour {


    public GameObject gameOverPanel;
    public Text gameOverText;
    public GameObject restartButton;
    public GameObject startInfo;

    public Player playerX;
    public Player playerO;
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;

    public AI ai;

    private string activePlayer;

    public Text[,] buttonList;
    public GameObject tablero4;
    public GameObject prefabSpace;
    public byte rows, columns;
    EvaluationMatrixMannager evMatrixCross;
    private EvaluationMatrixMannager evMatrix;

  
    //desactiva los botones de elegir jugar la informacion inicial el panel de game over y el restart.
    void Awake () {

        SetPlayerButtons(false);
        startInfo.SetActive(false);
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);      

    }

    //ESTABLECE EL TABLERO Y CASILLAS Y PERMITE ELEGIR EL JUGADOR  
    //funcion llamada desde el sizeBoard
    public void selectPlayer()
    {
        //activamos la seleccion del player que empieza primero
        SetPlayerButtons(true);
        startInfo.SetActive(true);

        //activePlayer = "X";
        //SetPlayerColors(playerX, playerO);
        // SetGameControllerReferenceOnButtons();

        buttonList = new Text[rows, columns];

        //contruye el tablero en la escena
        for (byte row = 0; row < rows; row++)
        {
            for (byte column = 0; column < columns; column++)
            {
                GameObject gameObject = (GameObject)Instantiate(prefabSpace, new Vector3(0, 0, 0), Quaternion.identity);
                gameObject.transform.SetParent(tablero4.transform);
                buttonList[row, column] = (Text)gameObject.GetComponentInChildren(typeof(Text));
                Space space = buttonList[row, column].GetComponentInParent<Space>();
                space.SetGameControllerReference(this);
                space.SetRowColumn(row, column);
            }
        }

        //pasamos el array de textos a la ia
        ai.SetButtonList(buttonList);
        //le pasamos a la IA el controlador de juego
        ai.SetGameController(this);

        //ai.setZobrichKeys(rows, columns);
    }

    //funcion para dar tamaño al tablero es llamada desde el sizeBoard
    public void setBoardSize(byte _rows,byte _columns)
    {
        rows = _rows;
        columns = _columns;
        //establecer el grill layaout
        tablero4.GetComponent<GridLayoutGroup>().constraintCount = _columns;
        //establezco la matriz de evaluacion dependiendo del tamaño del tablero.
        
        evMatrixCross = GameObject.Find("EvaluationMManager").GetComponent<EvaluationMatrixMannager>();
        evMatrixCross.u(rows, columns);

    }
	
    public void FillColumn (byte _row, byte _column)
    {
        //cuando colocamos una ficha del jugador
        if (activePlayer == "X")
        {
            //indicamos a la matriz de ocupacion q la casilla ha sido ocupada por una ficha del jugador
            evMatrixCross.setOccupationValue(_row, _column);
            //le pasamos la casilla a la matriz de evaluacion para que modifique sus valores para el comportamiento de la IA
            evMatrixCross.changeEvaluationMatrixPlayer(_row, _column);

        }
        Text buttonText = buttonList[_row, _column];
        if (buttonText.text == "")
        {
            Button button = buttonText.GetComponentInParent<Button>();
            buttonText.text = GetActivePlayer();
            button.interactable = false;               
        }       
    }

    public string GetActivePlayer ()
    {
        return activePlayer;
    }

    public void EndTurn()
    {
        if (IsBoardFull() || IsDraw())
        {
            gameOverText.text = "¡Empate!";
            SetPlayerColorsInactive();
            GameOver();
        } else
        if (IsWinState())
        {
            gameOverText.text = "¡Gana " + activePlayer + "!";
            GameOver();
        }     
        else
        {
            ChangeSides();
            if (activePlayer == "O")
            {
                ai.Play("O");
            }
/*
            else if (activePlayer == "X")
            {
                ai.Play("X");
            }
            */
        }
    }

    bool IsBoardFull ()
    {
        for (byte row = 0; row < rows; row++)
        {
            for (byte column = 0; column < columns; column++)
            {
                Text buttonText = buttonList[row, column];
                if (buttonText.text == "") return false;
            }
        }
        return true;
    }

    bool IsDraw()
    {
        Board checkBoard = new Board(rows, columns);
        for (byte row = 0; row < rows; row++)
        {
            for (byte column = 0; column < columns; column++)
            {
                Text spaceText = buttonList[row, column];
                checkBoard.spaces[row, column] = spaceText.text;
            }
        }
        checkBoard.activePlayer = this.activePlayer;



        if (checkBoard.Draw(activePlayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool IsWinState ()
    {
        Board checkBoard = new Board (rows, columns);
        for (byte row = 0; row < rows; row++)
        {
            for (byte column = 0; column < columns; column++)
            {
                Text spaceText = buttonList[row, column];
                checkBoard.spaces[row, column] = spaceText.text;
            }
        }
        checkBoard.activePlayer = this.activePlayer;

        

        if (checkBoard.IsWinningPosition(activePlayer)) 
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void ChangeSides ()
    {
        if (activePlayer == "X")
        {
            activePlayer = "O";
            SetPlayerColors(playerO, playerX);
        }
        else
        {
            activePlayer = "X";
            SetPlayerColors(playerX, playerO);
        }
    }
        
    void GameOver()
    {
        DeactivateSpaces();
        gameOverPanel.SetActive(true);
        restartButton.SetActive(true);
    }

    void DeactivateSpaces ()
    {
        SetSpacesInteractable(false);
    }

    void RestartSpaces()
    {
        EmptySpaces();
        SetSpacesInteractable(true);
    }

    void EmptySpaces ()
    {
        for (byte row = 0; row < rows; row++)
        {
            for (byte column = 0; column < columns; column++)
            {
                Text buttonText = buttonList[row, column];
                buttonText.text = "";
            }
        }
    }

    void SetSpacesInteractable (bool toggle)
    {
        for (byte row = 0; row < rows; row++)
        {
            for (byte column = 0; column < columns; column++)
            {
                Text buttonText = buttonList[row, column];
                Button button = buttonText.GetComponentInParent<Button>();
                button.interactable = toggle;
            }
        }
    }

    void SetPlayerColors (Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;
        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }

    public void SetStartingSide (string startingSide)
    {
        activePlayer = startingSide;
        if (activePlayer == "X")
        {
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            SetPlayerColors(playerO, playerX);
        }
        StartGame();
    }

    void SetPlayerColorsInactive ()
    {
        playerX.panel.color = inactivePlayerColor.panelColor;
        playerX.text.color = inactivePlayerColor.textColor;
        playerO.panel.color = inactivePlayerColor.panelColor;
        playerO.text.color = inactivePlayerColor.textColor;
    }

    //MODIFICADA
    public void RestartGame()
    {
        gameOverPanel.SetActive(false);
        //playerSide = "X";
        //SetPlayerColors(playerX, playerO);
        restartButton.SetActive(false);
        SetPlayerButtons(true);
        SetPlayerColorsInactive();
        EmptySpaces();
        ai.resetPreviousScore();
        startInfo.SetActive(true);
        //reseteamos los valores de la matriz de evaluacion
        evMatrixCross.resetMatrix();
    }

    void StartGame ()
    {
        RestartSpaces();
        SetPlayerButtons(false);
        startInfo.SetActive(false);
        if (activePlayer == "O")
        {
           ai.Play("O");
        }
    }

    void SetPlayerButtons (bool toggle)
    {
        playerX.button.interactable = toggle;
        playerO.button.interactable = toggle;
    }

}
