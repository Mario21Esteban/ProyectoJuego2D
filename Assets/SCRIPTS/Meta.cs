using UnityEngine;
using UnityEngine.SceneManagement;

public class Meta : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance.TieneSuficientesEstrellas())
            {
                GameManager.Instance.DetenerCronometro();
                int puntos = GameManager.Instance.CalcularPuntuacion();
                float tiempo = GameManager.Instance.tiempoTranscurrido;

                Debug.Log($"¡Nivel completado! Tiempo: {tiempo:F2} seg | Puntuación: {puntos}");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                Debug.Log("¡Necesitas más estrellas!");
            }
        }
    }
}