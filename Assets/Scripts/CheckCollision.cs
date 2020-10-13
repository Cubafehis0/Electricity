using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CheckCollision
{

    public static RaycastHit2D CheckUpCollison(Rigidbody2D rig, int layerMask,float distance)
    {
        Collider2D collider = rig.GetComponent<Collider2D>();
        Vector2 collider2DCenter =collider.bounds.center;
        Vector2 offSet = collider.bounds.extents;//坐标偏移量,右上角
        Vector2 ray =collider2DCenter + offSet;//射线射出位置
        RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.up, distance , 1 << layerMask);
        if (hit.collider != null)
        {
            return hit;
        }
        else
        {
            offSet = new Vector2(-collider.bounds.extents.x, collider.bounds.extents.y);
            ray = collider2DCenter + offSet;//左上角的射线
            hit = Physics2D.Raycast(ray, Vector2.up, distance, 1 << layerMask);
            if (hit.collider != null)
            {
                return hit;
            }
            else
            {
                offSet = new Vector2(0, collider.bounds.extents.y);
                ray = collider2DCenter + offSet;//左上角的射线
                hit = Physics2D.Raycast(ray, Vector2.up, distance, 1 << layerMask);
                return hit;
            }
        }
       
    }

    public static RaycastHit2D CheckDownCollison(Rigidbody2D rig, int layerMask, float distance)
    {
        Collider2D collider = rig.GetComponent<Collider2D>();
        Vector2 collider2DCenter = collider.bounds.center;
        Vector2 offSet = new Vector2(-collider.bounds.extents.x,- collider.bounds.extents.y); ;//坐标偏移量,左下角
        Vector2 ray = collider2DCenter + offSet;//射线射出位置
        RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.down, distance, 1 << layerMask);
        if (hit.collider != null)
        {
            return hit;
        }
        else
        {
            offSet = new Vector2(collider.bounds.extents.x, -collider.bounds.extents.y);
            ray = collider2DCenter + offSet;//右下角的射线
            hit = Physics2D.Raycast(ray, Vector2.down, distance, 1 << layerMask);
            if (hit.collider != null)
            {
                return hit;
            }
            else
            {
                offSet = new Vector2(0, -collider.bounds.extents.y);
                ray = collider2DCenter + offSet;//右下角的射线
                hit = Physics2D.Raycast(ray, Vector2.down, distance, 1 << layerMask);
                return hit;
            }
        }
    }
    public static RaycastHit2D CheckLeftCollison(Rigidbody2D rig, int layerMask, float distance)
    {
        Collider2D collider = rig.GetComponent<Collider2D>();
        Vector2 collider2DCenter = collider.bounds.center;
        Vector2 offSet = new Vector2(-collider.bounds.extents.x, -collider.bounds.extents.y+0.01f); ;//坐标偏移量,左下角
        Vector2 ray = collider2DCenter + offSet;//射线射出位置
        RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.left, distance, 1 << layerMask);
        if (hit.collider != null)
        {
            return hit;
        }
        else
        {
            offSet = new Vector2(-collider.bounds.extents.x, collider.bounds.extents.y-0.01f);
            ray = collider2DCenter + offSet;//角的射线
            hit = Physics2D.Raycast(ray, Vector2.left, distance, 1 << layerMask);
            return hit;
        }
    }

    public static RaycastHit2D CheckRightCollison(Rigidbody2D rig, int layerMask, float distance)
    {
        Collider2D collider = rig.GetComponent<Collider2D>();
        Vector2 collider2DCenter = collider.bounds.center;
        Vector2 offSet = new Vector2(collider.bounds.extents.x, -collider.bounds.extents.y + 0.01f); ;//坐标偏移量,右下角
        Vector2 ray = collider2DCenter + offSet;//射线射出位置
        RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.right, distance, 1 << layerMask);
        if (hit.collider != null)
        {
            return hit;
        }
        else
        {
            offSet = new Vector2(collider.bounds.extents.x, collider.bounds.extents.y - 0.01f);
            ray = collider2DCenter + offSet;//右下角的射线
            hit = Physics2D.Raycast(ray, Vector2.right, distance, 1 << layerMask);
            return hit;
        }
    }

}
