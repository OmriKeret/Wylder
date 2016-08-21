using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;

public class LineCastModel {

    public Transform MainObject { get; set; }
    public Transform TargetObject { get; set; }
    public Vector2 Start { get; set; }
    public Vector2 End { get; set; }
    public Action Invoker { get; set; }
    public float Size { get; set; }
    public LayerMask Mask { get; set; }
    
    private bool hit;

    public void Cast(bool faceRight)
    {
        int dir = faceRight ? 1 : -1;
        #region Debug
        if (TargetObject == null)
            Debug.DrawLine((Vector2)MainObject.position + Start, (Vector2)MainObject.position + Start + End * Size * dir, Color.red);
        else
            Debug.DrawLine((Vector2)MainObject.position, (Vector2)TargetObject.position,Color.green);
        #endregion
        RaycastHit2D curHit;
        if (TargetObject == null)
            curHit = Physics2D.Linecast((Vector2)MainObject.position +  Start, (Vector2)MainObject.position +  Start + End * Size * dir,Mask);
        else
            curHit = Physics2D.Linecast((Vector2)MainObject.position, (Vector2)TargetObject.position, Mask);

        hit = TargetObject == null ? curHit : curHit.distance >= Size ;

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
