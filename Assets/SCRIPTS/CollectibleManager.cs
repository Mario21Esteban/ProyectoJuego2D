using UnityEngine;

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
}