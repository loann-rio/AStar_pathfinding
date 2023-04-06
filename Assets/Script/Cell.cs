
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    // wall or empty space
    public bool isWall = false;

    // pos of the cell
    int x, y;


    // start and end pos
    int startX, startY;
    int endX, endY;

    public bool isStart = false;
    public bool isEnd = false;


    
    // weight of the empty space
    public float weight;
    public bool tested = false;

    // position of the cell that activated this
    private int activatorX;
    private int activatorY;


    public void setPos(int nx, int ny)
    {
        x = nx;
        y = ny;
    }

    public void set_start_end_pos(int sx, int sy, int ex, int ey)
    {
        startX = sx;
        startY = sy;

        endX = ex;
        endY = ey;

        if (x == startX && y == startY)
        {
            tested = true;
            isStart = true;
            GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.green);
            GetComponent<SpriteRenderer>().enabled = true;

        }

        if (x == endX && y == endY)
        {
            isEnd = true;
            GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.red);
            GetComponent<SpriteRenderer>().enabled = true;

        }
    }

    public void SetWall (bool wall) 
    {
        if (!isStart && !isEnd) // if the point is not start or end
        {
            // change if wall or empty
            isWall = wall;
            
            if (wall) // if wall: change visibility
            {
                // set color of object to black
                GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.black);
                GetComponent<SpriteRenderer>().enabled = true;
            } 
            else
            {
                GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    public void processPath(int actx, int acty)
    {
        // save pos activator
        if (!isEnd)
        {
            GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.grey);
            GetComponent<SpriteRenderer>().enabled = true;
        }

        // save activator cell
        activatorX = actx;
        activatorY = acty;

        tested = true;

        // get weight from dist to start + to end
        weight = Mathf.Sqrt(Mathf.Pow((float)(startX-x), 2) + Mathf.Pow((float)(startY-y), 2)) + Mathf.Sqrt(Mathf.Pow((float)(endX-x), 2) + Mathf.Pow((float)(endY-y), 2));
        
    }

    public int[] setAsPath()
    {
        if (!isEnd)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.blue);
        }
        return new int[] {activatorX, activatorY};
    }

    public int getX()
    {
        return x;
    }
    
    public int getY()
    {
        return y;
    }

    public float getWeight()
    {
        return weight;
    }

    public void reinitialise()
    {
        tested = false;

        if (!isWall && !isStart && !isEnd)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    

}
