using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{


    Piece selectedPiece;
    float clickTime;
    public BoxManager boxManager;
    public float maxDoubleClickTime;
    public float maxClickTime;
    float doubleClickTime;
    bool hasClicked;

    void Start()
    {
        doubleClickTime = 0;
    }

    void Update()
    {
        CheckClickOnPiece();
       
    }

    private void CheckClickOnPiece()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickTime = 0;
            SelectPiece();
        }

        if (Input.GetMouseButton(0))
        {
            clickTime += Time.deltaTime;
        }

        doubleClickTime += Time.deltaTime;

        if(selectedPiece != null)
        {
            if (hasClicked && doubleClickTime > maxDoubleClickTime)
            {
                if(IsSingleMovement())
                {
                    MoveSelectedPiece();
                }
                hasClicked = false;
            }

            if (Input.GetMouseButtonUp(0) && clickTime < maxClickTime)
            {
                if (doubleClickTime > maxDoubleClickTime)
                {
                    hasClicked = true;
                }
                else
                {
                    hasClicked = false;

                    if (selectedPiece.noBras == false && selectedPiece.isOutside == false && IsSingleMovement())
                    {
                        CallBackBras();
                    }

                }
                doubleClickTime = 0;
            }
        }
    }

    private bool IsSingleMovement()
    {
        bool canMove = true;

        foreach (Piece piece in boxManager.allPieces)
        {
            if (piece.isMoving == true)
            {
                canMove = false;
            }
        }
        if(canMove == false)
        {
            selectedPiece = null;
        }
        return canMove;
    }

    private void SelectPiece()
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 500, LayerMask.GetMask("Piece"));
        if (hit.collider != null)
        {
            selectedPiece = hit.collider.GetComponent<Piece>();
            
        }
    }
    private void MoveSelectedPiece()
    {
        if(selectedPiece.isOutside == true)
        {
            StartCoroutine(selectedPiece.Push());
        }
        else if(selectedPiece.noBras == false)
        {
            StartCoroutine(selectedPiece.CallBack());
        }
        else
        {
            StartCoroutine(selectedPiece.BrasGrabPiece());
        }

        selectedPiece = null;
    }

    private void CallBackBras()
    {
        StartCoroutine(selectedPiece.CallBackOnlyBrasFromCoord(selectedPiece.currentPosition));
        selectedPiece = null;
    }
}
