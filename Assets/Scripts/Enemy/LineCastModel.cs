using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;

public class LineCastModel {

    public Transform MainObject { get; set; }
    public Vector2 Start { get; set; }
    public Vector2 End { get; set; }
    public Action Invoker { get; set; }
    public LayerMask Mask { get; set; }

    private bool hit;

    public void Cast(bool faceRight)
    {
        int dir = faceRight ? 1 : -1;
        #region Debug
        Debug.DrawLine((Vector2) MainObject.position + Start, (Vector2)MainObject.position +  Start + End * dir, Color.red);
        #endregion
        hit = Physics2D.Linecast((Vector2)MainObject.position +  Start, (Vector2)MainObject.position +  Start + End * dir,Mask);
    }

    public void Flip()
    {
        Start += new Vector2(-2 * Start.x, 0);
        End += new Vector2(-2 * End.x, 0);
    }

    public bool CheckAndReset()
    {
        bool priorHit = hit;
        hit = false;
        return priorHit;
    }
}
