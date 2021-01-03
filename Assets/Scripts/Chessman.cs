using UnityEngine;

public class Chessman : MonoBehaviour
{
    public GameObject controller;
    public GameObject movePlate;

    private int xBoard = -1;
    private int yBoard = -1;

    private string player;

    // Sprites
    public Sprite s_sah, s_vezir, s_fil, s_at, s_kale, s_piyon;
    public Sprite b_sah, b_vezir, b_fil, b_at, b_kale, b_piyon;

    public void Activate(){
        controller = GameObject.FindGameObjectWithTag("GameController");

        setCoords();

        switch(this.name){
            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = s_vezir; player = "black"; break;
            case "black_king" : this.GetComponent<SpriteRenderer>().sprite = s_sah; player = "black"; break;
            case "black_bishop" : this.GetComponent<SpriteRenderer>().sprite = s_fil; player = "black"; break;
            case "black_knight" : this.GetComponent<SpriteRenderer>().sprite = s_at; player = "black"; break;
            case "black_pawn" : this.GetComponent<SpriteRenderer>().sprite = s_piyon; player = "black"; break;
            case "black_rook" : this.GetComponent<SpriteRenderer>().sprite = s_kale; player = "black"; break;

            case "white_queen": this.GetComponent<SpriteRenderer>().sprite = b_vezir; player = "white"; break;
            case "white_king" : this.GetComponent<SpriteRenderer>().sprite = b_sah; player = "white"; break;
            case "white_bishop" : this.GetComponent<SpriteRenderer>().sprite = b_fil; player = "white"; break;
            case "white_knight" : this.GetComponent<SpriteRenderer>().sprite = b_at; player = "white"; break;
            case "white_pawn" : this.GetComponent<SpriteRenderer>().sprite = b_piyon; player = "white"; break;
            case "white_rook" : this.GetComponent<SpriteRenderer>().sprite = b_kale; player = "white"; break;
        }
    }

    public void setCoords(){
        float x = xBoard;
        float y = yBoard;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        this.transform.position = new Vector3(x,y,-1.0f);
    }

    public int getXBoard(){
        return xBoard;
    }

    public int getYBoard(){
        return yBoard;
    }

    public void setXBoard(int x){
        xBoard = x;
    }

    public void setYBoard(int y){
        yBoard = y;
    }

    //İkinci Ders ChessMan
    private void OnMouseUp()
    {
        /*3. Ders*/
        if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player){
            DestroyMovePlates();
            InitiateMovePlates();
        }

        /*3. Ders*/
    }

    public void DestroyMovePlates(){
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        foreach (GameObject movePlate in movePlates)
        {
            Destroy(movePlate);
        }
    }

    public void InitiateMovePlates(){
        switch (this.name)
        {
            case "black_queen":            
            case "white_queen":            
                LineMovePlate(1,0);
                LineMovePlate(0,1);
                LineMovePlate(1,1);
                LineMovePlate(-1,0);
                LineMovePlate(0,-1);
                LineMovePlate(-1,-1);
                LineMovePlate(-1,1);
                LineMovePlate(1,-1);
            break;
            case "black_knight":
            case "white_knight":
                LMovePlate();
            break;
            case "black_bishop":            
            case "white_bishop":
                LineMovePlate(1,1);
                LineMovePlate(1,-1);
                LineMovePlate(-1,1);
                LineMovePlate(-1,-1);
            break;
            case "black_king":
            case "white_king":
                SurroundMovePlate();
            break;
            case "black_rook":
            case "white_rook":
                LineMovePlate(1,0);
                LineMovePlate(0,1);
                LineMovePlate(-1,0);
                LineMovePlate(0,-1);
            break;
            case "black_pawn":
                PawnMovePlate(xBoard, yBoard -1);
            break;
            case "white_pawn":
                PawnMovePlate(xBoard, yBoard +1);
            break;
        }
    }

    public void LineMovePlate(int xInc, int yInc){
        Game sc = controller.GetComponent<Game>();

        int x = xBoard + xInc;
        int y = yBoard + yInc;
        
        while (sc.PositionOnBoard(x,y) && sc.GetPosition(x,y) == null)
        {
            MovePlateSpawn(x,y);
            x += xInc;
            y += yInc;
        }

        if (sc.PositionOnBoard(x,y) && sc.GetPosition(x,y).GetComponent<Chessman>().player != player){
            MovePlateSpawn(x,y,true); //atacking
        }
    }

    public void LMovePlate(){
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard -1);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
    }

    public void SurroundMovePlate(){
        PointMovePlate(xBoard, yBoard +1);
        PointMovePlate(xBoard, yBoard -1);
        PointMovePlate(xBoard -1, yBoard -1);
        PointMovePlate(xBoard -1, yBoard -0);
        PointMovePlate(xBoard -1, yBoard +1);
        
        PointMovePlate(xBoard +1, yBoard -1);
        PointMovePlate(xBoard +1, yBoard -0);
        PointMovePlate(xBoard +1, yBoard +1);
    }

    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x,y)){

            GameObject cp = sc.GetPosition(x,y);

            if (cp == null)
            {
                MovePlateSpawn(x,y);
            }else if (cp.GetComponent<Chessman>().player != player)
            {
                MovePlateSpawn(x,y,true); //atacking
            }
        }
    }

    public void PawnMovePlate(int x, int y){
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x,y))
        {
            if (sc.GetPosition(x,y) == null)
            {
                MovePlateSpawn(x,y);
            }

            if (sc.PositionOnBoard(x+1 , y) && sc.GetPosition(x+1, y) != null && sc.GetPosition(x+1,y).GetComponent<Chessman>().player != player){
                MovePlateSpawn(x+1, y,true); //atacking
            }

            if (sc.PositionOnBoard(x-1 , y) && sc.GetPosition(x-1, y) != null && sc.GetPosition(x-1,y).GetComponent<Chessman>().player != player){
                MovePlateSpawn(x-1, y,true); //atacking
            }
        }
    }

    public void MovePlateSpawn(int matrixX,int matrixY, bool isAtacking = false){
        float x = matrixX;
        float y = matrixY;


        x *= 0.66f;
        y *= 0.66f;
        x += -2.3f;
        y += -2.3f; 

        Vector3 moveCoords = new Vector3(x, y, -3.0f);

        GameObject mp = Instantiate(movePlate, moveCoords, Quaternion.identity);


        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = isAtacking;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX,matrixY);
    }

}
