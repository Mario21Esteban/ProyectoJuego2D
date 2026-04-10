using UnityEngine;

public class Obstaculo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().RecibirGolpe();
        }
    }
}