using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float smoothTime = 0.2f; // suavizado del movimiento
    [SerializeField] private Vector2 deadZone = new Vector2(2f, 1f); 
    [SerializeField] private Color gizmoColor = new Color(1f, 0f, 0f, 0.3f);
    // ancho y alto de la zona muerta (en unidades del mundo)

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 cameraPos = transform.position;
        Vector3 targetPos = player.position;

        // calculamos la diferencia entre jugador y cámara
        Vector3 offset = targetPos - cameraPos;

        // comprobamos si el jugador salió de la "zona muerta" en X
        if (Mathf.Abs(offset.x) > deadZone.x)
        {
            cameraPos.x = Mathf.SmoothDamp(
                cameraPos.x,
                targetPos.x,
                ref velocity.x,
                smoothTime
            );
        }

        transform.position = new Vector3(cameraPos.x, cameraPos.y, transform.position.z);
    }


    private void OnDrawGizmos()
    {
        if (Camera.current == null) return;

        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(transform.position, new Vector3(deadZone.x * 2, deadZone.y * 2, 0));
    }
}