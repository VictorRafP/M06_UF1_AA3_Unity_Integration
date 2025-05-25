using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_Bar : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void CambiarVidaMax(float vidaMax)
    {
        if (slider != null)
        {
            slider.maxValue = vidaMax;
        }
    }

    public void CambiarVidaActual(float vidaActual)
    {
        if (slider != null)
        {
            slider.value = vidaActual; 
        }
    }

    public void InicializarBarraVida(float vidaMax)
    {
        CambiarVidaMax(vidaMax);
        CambiarVidaActual(vidaMax);
    }
}