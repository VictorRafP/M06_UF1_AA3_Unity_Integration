using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuInicioRegresar : MonoBehaviour
{
    public void RegresarMenuClick()
    {
        SceneManager.LoadScene("MenuInicio");
    }
}
