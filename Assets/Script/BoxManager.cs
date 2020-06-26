using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public int boxDimension;

    private Cube[,,] box;

    [HideInInspector] public List<Cube> outsideCube;
    public List<Piece> allPieces;

    public GameObject brasPrefab;

    void Start()
    {
        box = new Cube[boxDimension,boxDimension,boxDimension];
        foreach (Piece piece in allPieces)
        {
            piece.PutOutsideOnStart();
            StoreCube(piece);
        }
    }

    void PlaceCube(Cube cube)
    {
       cube.transform.position = cube.currentPosition.ToWorldCoordinate();
    }

    public bool IsCubeInFront(BoxCoordinate coord, BoxCoordinate coordDirection)
    {
        bool isCubeInFront = true;

        if (coord.x + coordDirection.x < boxDimension
            && coord.y + coordDirection.y < boxDimension
            && coord.z + coordDirection.z < boxDimension
            && coord.x + coordDirection.x >= 0
            && coord.y + coordDirection.y >= 0
            && coord.z + coordDirection.z >= 0
            && box[coord.x + coordDirection.x, coord.y + coordDirection.y, coord.z + coordDirection.z] == null)
        {
            isCubeInFront = false;
        }

        return isCubeInFront;
    }
    public bool IsAtTheEnd(Piece piece)
    {
        bool atTheEnd = false;
        switch (piece.cubeDirection)
        {
            case Direction.Xplus:
                if (piece.currentPosition.x == boxDimension)
                {
                    atTheEnd = true;
                }
                break;
            case Direction.Xmoins:
                if (piece.currentPosition.x == 0)
                {
                    atTheEnd = true;
                }
                break;
            case Direction.Yplus:
                if (piece.currentPosition.y == boxDimension)
                {
                    atTheEnd = true;
                }
                break;
            case Direction.Ymoins:
                if (piece.currentPosition.y == 0)
                {
                    atTheEnd = true;
                }
                break;
            case Direction.Zplus:
                if (piece.currentPosition.z == boxDimension)
                {
                    atTheEnd = true;
                }
                break;
            case Direction.Zmoins:
                if (piece.currentPosition.z == 0)
                {
                    atTheEnd = true;
                }
                break;
        }

        return atTheEnd;
    }

    public void StoreCube(Cube cube)
    {
        if (cube.currentPosition.x < boxDimension && cube.currentPosition.y < boxDimension && cube.currentPosition.z < boxDimension 
            && cube.currentPosition.x >= 0 && cube.currentPosition.y >= 0 && cube.currentPosition.z >= 0)
        {
            box[cube.currentPosition.x, cube.currentPosition.y, cube.currentPosition.z] = cube;
        }
        else
        {
            outsideCube.Add(cube);
        }
    }

    public void RemoveCube(Cube cube)
    {
        box[cube.currentPosition.x, cube.currentPosition.y, cube.currentPosition.z] = null;
    }

    public void CreateBrasOnPiece(Piece piece)
    {
        CreateBras(piece.currentPosition, piece.cubeDirection);
    }

    public void CreateBras(BoxCoordinate coord, Direction direction)
    {
        GameObject bras = Instantiate(brasPrefab, coord.ToWorldCoordinate(), Quaternion.identity);
        Bras scriptBras = bras.GetComponent<Bras>();
        scriptBras.currentPosition = coord;
        if (direction == Direction.Xplus || direction == Direction.Xmoins)
        {
            bras.transform.rotation = Quaternion.Euler(45, 0, 0);
        }
        else if (direction == Direction.Yplus || direction == Direction.Ymoins)
        {
            bras.transform.rotation = Quaternion.Euler(0, 45, 90);
        }
        else if (direction == Direction.Zplus || direction == Direction.Zmoins)
        {
            bras.transform.rotation = Quaternion.Euler(45, 90, 0);
        }
        StoreCube(scriptBras);
    }

    public void DestroyBras(BoxCoordinate coord)
    {
        Destroy(box[coord.x, coord.y, coord.z].gameObject);
        box[coord.x, coord.y, coord.z] = null;
    }

    public Cube GetCubeFromBox(BoxCoordinate coord)
    {
        return box[coord.x, coord.y, coord.z];
    }
}
