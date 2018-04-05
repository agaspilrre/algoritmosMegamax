using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//script para el funcionamiento de los botones que establecen el tamaño del tablero
public class SizeBoard : MonoBehaviour {

    public bool IsRow;
    public bool IsColumn;

    public GameObject buttonRows;
    public GameObject buttonColumns;
    public GameObject buttonIncrement;
    public GameObject buttonDecrement;
    public GameObject buttonAccept;

    public GameController gameController;

    byte rows = 0;
    byte columns = 0;

	// Use this for initialization
	void Awake () {

        //buttonRows = GameObject.Find("sizeRows ");
        //buttonColumns = GameObject.Find("sizeColumns");

	}
	
	
    //detecta si hemos clicado el boton de filas
    //deja seleccionado este boton a traves del bool para que podamos incrementar o decrementar las filas
    public void OnRowsClick()
    {
        IsRow = true;
        IsColumn = false;
    }

    //detecta si hemos clicado el boton de columnas
    //deja seleccionado este boton a traves del bool para que podamos incrementar o decrementar el numero de columnas
    public void OnColumnsClick()
    {
        IsRow = false;
        IsColumn = true;
    }

    //detecta si hemos pulsado el boton +
    //incrementa filas o columnas dependiendo del boton de filas o columnas que hayamos seleccionado
    public void OnIncrementClick()
    {
        if (IsRow)
        {
            rows++;
            buttonRows.GetComponentsInChildren<Text>()[0].text = "Rows: " + rows;
        }

        else if (IsColumn)
        {
            columns++;
            buttonColumns.GetComponentsInChildren<Text>()[0].text = "Columns: "+columns;
        }
    }
    //detecta si hemos pulsado el boton -
    //decrementa filas o columnas dependiendo de que boton este seleccionado
    public void OnDecrementClick()
    {
        if (IsRow)
        {
            rows--;
            buttonRows.GetComponentsInChildren<Text>()[0].text = "Rows: " + rows;
        }

        else if (IsColumn)
        {
            columns--;
            buttonColumns.GetComponentsInChildren<Text>()[0].text = "Columns: " + columns;
        }
    }

    //detecta si el boton de aceptar ha sido pulsado
    //le pasa al gamecontroler las filas y columnas que ha elegido el jugador
    public void OnAcceptClick()
    {
        gameController.setBoardSize(rows, columns);
        gameController.selectPlayer();
        //desactivar los botones de seleccion de tamaño de tablero
        buttonColumns.SetActive(false);
        buttonRows.SetActive(false);
        buttonDecrement.SetActive(false);
        buttonIncrement.SetActive(false);
        buttonAccept.SetActive(false);
    }

   


}
