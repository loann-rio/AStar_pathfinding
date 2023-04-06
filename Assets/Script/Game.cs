using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Game : MonoBehaviour
{

    private static int screen_width = 64;
    private static int screen_height = 48;
    private static int sizeSquare = 16;


    private float timer;

    public bool placing = true;

    // start and end pos:
    private int startPosX = 3;
    private int startPosY = 3;

    private int endPosX = 50;
    private int endPosY = 30;


    Cell[,] grid = new Cell[screen_width, screen_height];

    // list of possible next step, should always be sorted by weight
    List<Cell> possibleWay = new List<Cell>();

    // Start is called before the first frame update
    void Start()
    {
        initCells();    
    }

    // Update is called once per frame
    void Update()
    {
        if (placing)
        {
            // placing mode:
            placeCell();
        }
        else
        {
            getPath();
            placing = true;
        }
    }
    
    void initCells()
    {
        for (int y=0; y<screen_height; y++) 
        {
            for (int x=0; x<screen_width; x++)
            {
                Cell cell = Instantiate(Resources.Load("Prefab/Cell", typeof(Cell)), new Vector2(x, y), Quaternion.identity) as Cell;
                
                cell.SetWall(false);
                cell.setPos(x, y);

                cell.set_start_end_pos(startPosX, startPosY, endPosX, endPosY);
                grid[x, y] = cell;
                
            }
        }
    }

    void reinitialiseCells()
    {
        for (int y=0; y<screen_height; y++) 
        {
            for (int x=0; x<screen_width; x++)
            {
                grid[x, y].reinitialise();
            }
        }
    }

    void placeCell() 
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            float x = (mousePos.x/sizeSquare);
            grid[ (int) mousePos.x/sizeSquare, (int) mousePos.y/sizeSquare ].SetWall(true);
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 mousePos = Input.mousePosition;
            float x = (mousePos.x/sizeSquare);
            grid[ (int) mousePos.x/sizeSquare, (int) mousePos.y/sizeSquare ].SetWall(false);
        }
    }

    void addToListCell(Cell cell)
    {
        if (possibleWay.Count == 0)
        {
            possibleWay.Add(cell);
            return;
        }

        bool f = false;
        for (int i=0; i<possibleWay.Count; i++)
        {
            Debug.Log(i + " " + possibleWay.Count);
            if (possibleWay[i].getWeight() > cell.getWeight())
            {
                possibleWay.Insert(i, cell);
                f = true;
                break;
            }
        }

        if (!f)
        {
            possibleWay.Add(cell);
        }

    }

    void getPath()
    {

        reinitialiseCells();

        possibleWay =  new List<Cell>() {grid[startPosX, startPosY]};

        //possibleWay.Add();

        bool done = false;
        
        while (!done)
        {
            
            Cell currentCell = possibleWay[0];
            possibleWay.RemoveAt(0);

            done = (done||checkNeighbors(currentCell,  1,  0));
            done = (done||checkNeighbors(currentCell, -1,  0));
            done = (done||checkNeighbors(currentCell,  0,  1));
            done = (done||checkNeighbors(currentCell,  0, -1));

            if (possibleWay.Count == 0)
            {
                Debug.Log("no possible way");
                return;
            }
        }

        int[] pos = new int[] {endPosX, endPosY};
        done = false;
        while (!done)
        {
            pos = grid[pos[0], pos[1]].setAsPath();

            if ((pos[1] == startPosY) && (pos[0] == startPosX))
            {
                done = true;
            }
        }
    }

    bool checkNeighbors(Cell currentCell, int x, int y)
    {
        if ((currentCell.getX()+x>=0) && (currentCell.getY()+y>=0) && (currentCell.getX()+x<screen_width) && (currentCell.getY()+y<screen_height))
        {
            if ((!grid[currentCell.getX()+x, currentCell.getY()+y].isWall) && (!grid[currentCell.getX()+x, currentCell.getY()+y].tested))
            {
                grid[currentCell.getX()+x, currentCell.getY()+y].processPath( currentCell.getX(), currentCell.getY() );
                addToListCell(grid[currentCell.getX()+x, currentCell.getY()+y]);
            }

            if (currentCell.getX()+x == endPosX && currentCell.getY()+y == endPosY)
            {
                Debug.Log("end");
                return true;
            }
        }
        return false;
    }

    
}