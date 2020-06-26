using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoxCoordinate 
{
    public int x,y,z;
    public Vector3 ToWorldCoordinate()
    {
        Vector3 coord;

        coord.x = x + 0.5f;
        coord.y = y + 0.5f;
        coord.z = z + 0.5f;

        return coord;
    }

    public BoxCoordinate OutsidePos(Direction direction, BoxManager boxManager)
    {
        BoxCoordinate position = new BoxCoordinate();
        switch (direction)
        {
            case Direction.Xplus:
                position = new BoxCoordinate(-1, y, z);
                break;
            case Direction.Xmoins:
                position = new BoxCoordinate(4, y, z);
                break;
            case Direction.Yplus:
                position = new BoxCoordinate(x, -1, z);
                break;
            case Direction.Ymoins:
                position = new BoxCoordinate(x, 4, z);
                break;
            case Direction.Zplus:
                position = new BoxCoordinate(x, y, -1);
                break;
            case Direction.Zmoins:
                position = new BoxCoordinate(x, y, 4);
                break;
        }
        return position;
    }

    public BoxCoordinate (int _x, int _y, int _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }
    public BoxCoordinate()
    {
        x = 0;
        y = 0;
        z = 0;
    }

    public static BoxCoordinate WorldToBoxCoordinate(Vector3 pos)
    {
        BoxCoordinate coord = new BoxCoordinate();
        coord.x = Mathf.RoundToInt(pos.x - 0.5f);
        coord.y = Mathf.RoundToInt(pos.y - 0.5f);
        coord.z = Mathf.RoundToInt(pos.z - 0.5f);
        return coord;
    }

    public override string ToString()
    {
        return x + "-" + y + "-" + z;
    }

    public static BoxCoordinate operator +(BoxCoordinate a, BoxCoordinate b)
        => new BoxCoordinate(a.x + b.x, a.y + b.y, a.z + b.z);


    public static BoxCoordinate operator -(BoxCoordinate a, BoxCoordinate b)
        => new BoxCoordinate(a.x - b.x, a.y - b.y, a.z - b.z);

    public static bool operator ==(BoxCoordinate a, BoxCoordinate b)
    {
        if(a.x == b.x && a.y == b.y && a.z == b.z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool operator !=(BoxCoordinate a, BoxCoordinate b)
    {
        if (a.x != b.x || a.y != b.y || a.z != b.z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
