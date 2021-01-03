using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{

    public GameObject controller;

    GameObject reference = null;

    //Board Positions
    int matrixX;
    int matrixY;

    //false : movenment, true attacking
    public bool attack = false;

    public void Start(){
        if (attack){
            //Change to red
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f,0.0f,0.0f,1.0f);
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        if (attack){
            GameObject cp  = controller.GetComponent<Game>().GetPosition(matrixX,matrixY);
            
            if (cp.name == "white_king"){
                controller.GetComponent<Game>().Winner("black");
            }
            if (cp.name == "black_king"){
                controller.GetComponent<Game>().Winner("white");
            }
            
            Destroy(cp);
        }

        controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<Chessman>().getXBoard(),
                reference.GetComponent<Chessman>().getYBoard());

        reference.GetComponent<Chessman>().setXBoard(matrixX);
        reference.GetComponent<Chessman>().setYBoard(matrixY);
        reference.GetComponent<Chessman>().setCoords();


        controller.GetComponent<Game>().setPosition(reference);

        /* 3. Ders */
        controller.GetComponent<Game>().NextTurn();

        /* 3. Ders */
        reference.GetComponent<Chessman>().DestroyMovePlates();
    }

    public void SetCoords(int x, int y){
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj){
        reference = obj;
    }

    public GameObject GetReference(){
        return reference;
    }


}
