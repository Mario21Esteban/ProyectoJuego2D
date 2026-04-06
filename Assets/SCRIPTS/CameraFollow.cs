using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Referencia")]
    [SerializeField] private Transform target;

    [Header("Configuración")]
    [SerializeField] private float xOffset = 2f;
    [SerializeField] private float smoothSpeed = 5f;

    private float fixedY;
    private float fixedZ;

    private void Start()
    {
        fixedY = transform.position.y;
        fixedZ = transform.position.z;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        float desiredX = target.position.x + xOffset;

        Vector3 desiredPosition = new Vector3(desiredX, fixedY, fixedZ);

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}