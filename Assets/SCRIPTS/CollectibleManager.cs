using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Estrellas")]
    public int estrellasRecolectadas = 0;
    public int estrellasTotal = 3;

    [Header("Tiempo")]
    public float tiempoTranscurrido = 0f;
    private bool cronometroActivo = true;

    [Header("Rangos de puntuación")]
    public float tiempoOroPuntos = 30f;      // 30 seg o menos
    public float tiempoPlata = 60f;          // 31 a 60 seg
    public int puntosOro = 1000;
    public int puntosPlata = 500;
    public int puntosBronce = 100;           // 61 seg o más

    private int puntuacionFinal = 0;

    [Header("Vida")]
    public int vidaMaxima = 3;
    public int vidaActual;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (cronometroActivo)
        {
            tiempoTranscurrido += Time.deltaTime;
        }
    }

    public void AgregarEstrella()
    {
        estrellasRecolectadas++;
        Debug.Log($"Estrellas: {estrellasRecolectadas} / {estrellasTotal}");
    }

    public bool TieneSuficientesEstrellas()
    {
        return estrellasRecolectadas >= estrellasTotal;
    }

    public int CalcularPuntuacion()
    {
        if (tiempoTranscurrido <= tiempoOroPuntos)
            puntuacionFinal = puntosOro;
        else if (tiempoTranscurrido <= tiempoPlata)
            puntuacionFinal = puntosPlata;
        else
            puntuacionFinal = puntosBronce;

        return puntuacionFinal;
    }

    public void DetenerCronometro()
    {
        cronometroActivo = false;
    }

    private void Start()
    {
        vidaActual = vidaMaxima;
    }

    public void RecibirDanio()
    {
        if (vidaActual <= 0) return;

        vidaActual--;
        HUDManager.Instance.ActualizarCorazones(vidaActual);

        if (vidaActual <= 0)
            Morir();
    }

    private void Morir()
    {
        Debug.Log("El jugador murió");
        StartCoroutine(MorirCoroutine());
    }

    private System.Collections.IEnumerator MorirCoroutine()
    {
        // Notifica al jugador para activar animación
        FindFirstObjectByType<PlayerController>().ActivarMuerte();

        // Espera que termine la animación antes de reiniciar
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}