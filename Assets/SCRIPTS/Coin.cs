using UnityEngine;

public class Estrella : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AgregarEstrella();
            Destroy(gameObject);
        }
    }
}