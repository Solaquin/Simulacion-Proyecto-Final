using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Codigo_Volumen : MonoBehaviour
{
    public Slider slider;              // Referencia al slider de volumen
    public float sliderValue;         // Valor actual del slider
    public Image imagenMute;          // Imagen para mostrar si está en mute

    void Start()
    {
        // Carga el valor guardado en PlayerPrefs (0.5 por defecto)
        sliderValue = PlayerPrefs.GetFloat("volumenAudio", 0.5f);
        slider.value = sliderValue;

        // Ajusta el volumen global
        AudioListener.volume = sliderValue;

        // Muestra u oculta el icono de mute
        RevisarSiEstoyMute();
    }

    public void ChangeSlider(float valor)
    {
        // Actualiza el valor
        sliderValue = valor;

        // Guarda el volumen
        PlayerPrefs.SetFloat("volumenAudio", sliderValue);

        // Cambia el volumen del juego
        AudioListener.volume = sliderValue;

        // Revisa si debe mostrar mute
        RevisarSiEstoyMute();
    }

    public void RevisarSiEstoyMute()
    {
        if (sliderValue == 0)
        {
            imagenMute.enabled = true;
        }
        else
        {
            imagenMute.enabled = false;
        }
    }
}
