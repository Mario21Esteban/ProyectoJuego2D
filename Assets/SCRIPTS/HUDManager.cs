using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [Header("Referencias UI")]
    public TextMeshProUGUI tiempoText;
    public TextMeshProUGUI coinText;

    [Header("Corazones")]
    public UnityEngine.UI.Image[] corazones;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        ActualizarTiempo();
        ActualizarEstrellas();
    }

    private void ActualizarTiempo()
    {
        float tiempo = GameManager.Instance.tiempoTranscurrido;

        int minutos = Mathf.FloorToInt(tiempo / 60f);
        int segundos = Mathf.FloorToInt(tiempo % 60f);

        tiempoText.text = $"{minutos:00}:{segundos:00}";
    }

    private void ActualizarEstrellas()
    {
        int recolectadas = GameManager.Instance.estrellasRecolectadas;
        int total = GameManager.Instance.estrellasTotal;

        coinText.text = $"{recolectadas} / {total}";
    }

    public void ActualizarCorazones(int vidaActual)
    {
        float porCorazon = 1f / corazones.Length;
        float fillTotal = (float)vidaActual / corazones.Length;

        for (int i = 0; i < corazones.Length; i++)
        {
            float fillEsteCorazon = Mathf.Clamp01(fillTotal - porCorazon * i) / porCorazon;
            corazones[i].fillAmount = fillEsteCorazon;
        }
    }

}