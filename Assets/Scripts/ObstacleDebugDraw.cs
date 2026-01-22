using UnityEngine;

//Luke script
public class ObstacleDebugDraw : MonoBehaviour
{
    [Header("Box Settings")]
    [SerializeField] private Color boxColor = Color.green;
    [SerializeField] private Vector3 boxSize = new Vector3(2, 2, 2);
    [SerializeField] private bool showOnlyWhenSelected = false;

    // This draws the box always as long as Gizmos are enabled in the scene view
    private void OnDrawGizmos()
    {
        if (showOnlyWhenSelected) return;
        DrawBox();
    }

    // This draws the box only when you click on the object
    private void OnDrawGizmosSelected()
    {
        if (showOnlyWhenSelected)
        {
            DrawBox();
        }
    }

    private void DrawBox()
    {
        Gizmos.color = boxColor;
        //Apply the object's rotation and position to the gizmo
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.matrix = rotationMatrix;
        //Draw the wireframe box (lines only)
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}
