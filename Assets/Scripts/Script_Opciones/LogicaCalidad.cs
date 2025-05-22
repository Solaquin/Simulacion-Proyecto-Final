using UnityEngine;
using TMPro; // Asegúrate de tener el paquete TextMeshPro instalado
public class LogicaCalidad : MonoBehaviour
{
    public TMP_Dropdown calidadDropdown;
    public int calidad;// Referencia al dropdown de calidad
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        calidad = PlayerPrefs.GetInt("numeroDeCalidad", 4);
        calidadDropdown.value = calidad;
        AjustarCalidad();

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AjustarCalidad()
    {
        QualitySettings.SetQualityLevel(calidadDropdown.value);
        PlayerPrefs.SetInt("numeroDeCalidad", calidadDropdown.value);
        calidad= calidadDropdown.value;
    }
}
