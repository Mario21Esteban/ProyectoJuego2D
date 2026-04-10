using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform target;

    [Header("Límites del nivel")]
    public float minX = 0f;
    public float maxX = 56f;

    void LateUpdate()
    {
        if (target == null) return;

        float clampedX = Mathf.Clamp(target.position.x, minX, maxX);

        transform.position = new Vector3(
            clampedX,
            transform.position.y,
            transform.position.z
        );
    }
}