using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : Cube
{
    public BoxCoordinate initialPosition;

    public GameObject prefabPince;

    GameObject pince;

    [HideInInspector] public bool isMoving;

    public Direction cubeDirection;

    public BoxManager boxManager;

    public float movementSpeed;

    public float moveStopDistance;

    [HideInInspector] public bool isOutside;

    [HideInInspector] public BoxCoordinate coordDirection;

    public bool move;

    public bool noBras;

    int currentBrasNumber;

    public BoxCoordinate positionInConfig;

    public bool inConfig;

    void Start()
    {
        coordDirection = PieceDirection();
        isOutside = true;
        currentBrasNumber = 0;
    }

    void Update()
    {
        CheckIsInConfig();
    }

    public void CheckIsInConfig()
    {
        if(currentPosition == positionInConfig)
        {
            inConfig = true;
        }
        else
        {
            inConfig = false;
        }
    }

    public IEnumerator BrasGrabPiece()
    {

        isMoving = true;
        currentBrasNumber++;

        BoxCoordinate coordGrabBras = initialPosition;
        BoxCoordinate coordPince = coordGrabBras + coordDirection;
        pince.transform.position = coordPince.ToWorldCoordinate();

        boxManager.CreateBras(coordGrabBras, cubeDirection);
        yield return new WaitForSeconds(0.3f);

        while (boxManager.IsCubeInFront(coordGrabBras, coordDirection) == false)
        {
            coordGrabBras += coordDirection;
            coordPince = coordGrabBras + coordDirection;
            pince.transform.position = coordPince.ToWorldCoordinate();
            boxManager.CreateBras(coordGrabBras, cubeDirection);
            currentBrasNumber++;
            yield return new WaitForSeconds(0.3f);
        }
        Cube cubeInFront = boxManager.GetCubeFromBox(coordGrabBras + coordDirection);

        if(cubeInFront == this)
        {
            noBras = false;
        }
        else 
        {
            StartCoroutine(CallBackOnlyBrasFromCoord(coordGrabBras+coordDirection));
        }
        isMoving = false;
    }

    public IEnumerator CallBackOnlyBrasFromCoord(BoxCoordinate startCoord)
    {
        isMoving = true;
        noBras = true;

        BoxCoordinate coordRecallBras = startCoord;

        while (currentBrasNumber != 0)
        {
            
            coordRecallBras -= coordDirection;

            pince.transform.position = coordRecallBras.ToWorldCoordinate();

            if (coordRecallBras != initialPosition)
            {

                boxManager.DestroyBras(coordRecallBras);
                
            }
            else
            {
                Bras finalBras = null;
                foreach (Cube cube in boxManager.outsideCube)
                {
                    if (cube.currentPosition == coordRecallBras)
                    {
                        finalBras = cube as Bras;
                    }
                }
                if (finalBras != null)
                {
                    Destroy(finalBras.gameObject);
                }
            }
            currentBrasNumber--;
            yield return new WaitForSeconds(0.3f);
        }

        isMoving = false;
    }
    public IEnumerator Push()
    {
        isMoving = true;
        while (boxManager.IsCubeInFront(currentPosition, coordDirection) == false && boxManager.IsAtTheEnd(this) == false)
        {
            
            if (currentPosition != initialPosition)
            {
                boxManager.RemoveCube(this);
            }
            boxManager.CreateBrasOnPiece(this);
            currentBrasNumber ++;
            isOutside = false;
            currentPosition += coordDirection;
            boxManager.StoreCube(this);
            while(Vector3.Distance(transform.localPosition, currentPosition.ToWorldCoordinate()) > moveStopDistance*Time.deltaTime* movementSpeed)
            {
                transform.localPosition += new Vector3(coordDirection.x, coordDirection.y, coordDirection.z) * movementSpeed * Time.deltaTime;
                pince.transform.position = transform.position;
                yield return new WaitForEndOfFrame();
            }
            transform.localPosition = currentPosition.ToWorldCoordinate();
            yield return new WaitForSeconds(0.1f);
        }

        boxManager.StoreCube(this);
        isMoving = false;
       
    }

    public IEnumerator CallBack()
    {
        isMoving = true;
        boxManager.RemoveCube(this);

        while (currentPosition != initialPosition)
        {
            currentPosition -= coordDirection;

            while (Vector3.Distance(transform.localPosition, currentPosition.ToWorldCoordinate()) > moveStopDistance * Time.deltaTime * movementSpeed)
            {
                transform.localPosition -= new Vector3(coordDirection.x, coordDirection.y, coordDirection.z) * movementSpeed * Time.deltaTime;
                pince.transform.position = transform.position;
                yield return new WaitForEndOfFrame();
            }

            transform.localPosition = currentPosition.ToWorldCoordinate();
            if(currentPosition != initialPosition)
            {
                boxManager.DestroyBras(currentPosition);
                boxManager.RemoveCube(this);
            }
            else
            {
                Bras finalBras = null;
                foreach(Cube cube in boxManager.outsideCube)
                {
                    if(cube.currentPosition == currentPosition)
                    {
                        finalBras = cube as Bras;
                    }
                }
                if (finalBras != null)
                {
                    Destroy(finalBras.gameObject);
                }
            }
            currentBrasNumber--;
            yield return new WaitForSeconds(0.1f);
        }
        isMoving = false;
        isOutside = true;
    }
    public BoxCoordinate PieceDirection()
    {
        BoxCoordinate coord = new BoxCoordinate();

        switch (cubeDirection)
        {
            case Direction.Xplus:
                coord.x = 1;
                break;
            case Direction.Xmoins:
                coord.x = -1;
                break;
            case Direction.Yplus:
                coord.y = 1;
                break;
            case Direction.Ymoins:
                coord.y = -1;
                break;
            case Direction.Zplus:
                coord.z = 1;
                break;
            case Direction.Zmoins:
                coord.z = -1;
                break;
        }

        return coord;
    }

    public void CreatePince()
    {
        Vector3 pinceRotation = Vector3.zero;

        switch (cubeDirection)
        {
            case Direction.Xplus:
                pinceRotation.y = -90;
                break;
            case Direction.Xmoins:
                pinceRotation.y = 90;
                break;
            case Direction.Yplus:
                pinceRotation.x = 90;
                break;
            case Direction.Ymoins:
                pinceRotation.x = -90;
                break;
            case Direction.Zplus:
                pinceRotation.x = -180;
                break;
            case Direction.Zmoins:
                pinceRotation = Vector3.zero;
                break;
        }

        pince = Instantiate(prefabPince, initialPosition.ToWorldCoordinate(), Quaternion.Euler(pinceRotation));
    }
    public void PutOutsideOnStart()
    {
        currentPosition = BoxCoordinate.WorldToBoxCoordinate(transform.position);
        positionInConfig = currentPosition;
        initialPosition = currentPosition.OutsidePos(cubeDirection, boxManager);
        transform.localPosition = initialPosition.ToWorldCoordinate();
        currentPosition = initialPosition;
        CreatePince();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        currentPosition = BoxCoordinate.WorldToBoxCoordinate(transform.position);
        transform.localPosition = currentPosition.ToWorldCoordinate();
        Gizmos.DrawWireCube(currentPosition.OutsidePos(cubeDirection, boxManager).ToWorldCoordinate(), Vector3.one);
    }
}
