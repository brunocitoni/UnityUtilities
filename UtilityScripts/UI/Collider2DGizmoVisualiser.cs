using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Collider2DGizmoVisualizer : MonoBehaviour
{

#if UNITY_EDITOR
    private Collider2D thisCollider2d;

    void OnDrawGizmos()
    {
        thisCollider2d = GetComponent<Collider2D>(); // Always get the latest reference

        Gizmos.color = Color.green;

        if (thisCollider2d is BoxCollider2D box)
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawWireCube(box.offset, box.size);
        }
        else if (thisCollider2d is CircleCollider2D circle)
        {
            Gizmos.DrawWireSphere((Vector2)transform.position + circle.offset, circle.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y));
        }
        else if (thisCollider2d is PolygonCollider2D polygon)
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Vector2[] points = polygon.points;
            for (int i = 0; i < points.Length; i++)
            {
                Vector2 currentPoint = points[i];
                Vector2 nextPoint = points[(i + 1) % points.Length];
                Gizmos.DrawLine(currentPoint, nextPoint);
            }
        }
        else if (thisCollider2d is EdgeCollider2D edge)
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Vector2[] points = edge.points;
            for (int i = 0; i < points.Length - 1; i++)
            {
                Gizmos.DrawLine(points[i], points[i + 1]);
            }
        }
    }

#endif
}
