using UnityEngine;

public class WindmillManager : MonoBehaviour
{
    [SerializeField] private Windmill[] windmills = new Windmill[3]; // Am Anfang 3 Windräder
    [SerializeField] private GameObject colorTarget; // Leinwand für Mischfarbe

    private int currentActiveIndex = 0;
    private Color mixedColor = Color.black;
    private float totalSpeed = 0f;

    private void Start()
    {
        // Alle Windräder mit dem Manager verbinden & deaktivieren
        for (int i = 0; i < windmills.Length; i++)
        { 
            if (windmills[i] != null)
            {
                windmills[i].SetManager(this, i);
                windmills[i].SetActiveState(false);
            }
        }

        // Nur das erste Windrad aktivieren
        if (windmills.Length > 0 && windmills[0] != null)
        {
            windmills[0].SetActiveState(true);
        }
    }

    public void NotifyWindmillLocked(int index, Color color, float speed)
    {
        mixedColor += color * speed; // Farbgewichtung anhand der Geschwindigkeit
        totalSpeed += speed;

        UpdateMixedColor();

        // Nächstes Windrad aktivieren, falls vorhanden
        currentActiveIndex++;
        if (currentActiveIndex < windmills.Length && windmills[currentActiveIndex] != null)
        {
            windmills[currentActiveIndex].SetActiveState(true);
        }
    }

    private void UpdateMixedColor()
    {
        if (colorTarget != null && totalSpeed > 0)
        {
            Color finalColor = mixedColor / totalSpeed; // Normierte Mischfarbe berechnen

            Renderer targetRenderer = colorTarget.GetComponent<Renderer>();
            if (targetRenderer != null)
            {
                targetRenderer.material.color = finalColor;
            }
        }
    }
}
