using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro; // Asegúrate de tener el paquete TextMeshPro instalado

public class Logica_FullScreen : MonoBehaviour
{
    public Toggle toggle;
    public TMP_Dropdown resolucionesDropdown;
    Resolution[] resoluciones;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Screen.fullScreen)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
        RevisarResolucion();

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ActivarPantallaCompleta(bool pantallaCompleta)
    {
        Screen.fullScreen = pantallaCompleta;
        // Guardar la preferencia en PlayerPrefs
        PlayerPrefs.SetInt("PantallaCompleta", pantallaCompleta ? 1 : 0);
    }
    public void RevisarResolucion()
    {
        resoluciones = Screen.resolutions;
        resolucionesDropdown.ClearOptions();
        List<string> opciones = new List<string>();
        int resolucionActual = PlayerPrefs.GetInt("resolucionActual", 0);

        for (int i = 0; i < resoluciones.Length; i++)
        {
            string opcion = resoluciones[i].width + "x" + resoluciones[i].height;
            opciones.Add(opcion);
            if (Screen.fullScreen && resoluciones[i].width == Screen.currentResolution.width && resoluciones[i].height == Screen.currentResolution.height)
            {
                resolucionActual = i;

            }

        }
        resolucionesDropdown.AddOptions(opciones);
        resolucionesDropdown.value = resolucionActual;
        resolucionesDropdown.RefreshShownValue();
    }
    public void CambiarResolucion(int indiceResolucion)
    {
        PlayerPrefs.SetInt("numeroResolucion", resolucionesDropdown.value);
        Resolution resolucion = resoluciones[indiceResolucion];
        Screen.SetResolution(resolucion.width, resolucion.height, Screen.fullScreen);
        
    
    }
}
