using UnityEngine;

public class LevelGoal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡Nivel completado!");
            Time.timeScale = 0f;
        }
    }
}