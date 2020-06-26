using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyConfiguration : MonoBehaviour
{
    public BoxManager boxmanager;
    void Start()
    {
        
    }

    
    void Update()
    {
        CheckIsFullConfig();
    }

    void CheckIsFullConfig()
    {
        bool isfull = true;
        bool moving = false;
        foreach (Piece piece in boxmanager.allPieces)
        {
            if (piece.inConfig == false)
            {
                isfull = false;
            }
            if (piece.isMoving)
            {
                moving = true;
            }
        }
        if (isfull && moving == false)
        {
            
            foreach (Piece piece in boxmanager.allPieces)
            {
                if(piece)
                StartCoroutine(piece.CallBackOnlyBrasFromCoord(piece.currentPosition));
            }
        }
    }
}
