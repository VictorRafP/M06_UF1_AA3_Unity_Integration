using UnityEngine;
using UnityEngine.SceneManagement;

public class menuDebug : MonoBehaviour
{
    public void MenuInicioClick()
    {
        SceneManager.LoadScene("MenuInicio");
    }


    public void OleadaTutorial()
    {
        SceneManager.LoadScene("TutorialOleada");
    }  
    public void JefeTutorial()
    {
       SceneManager.LoadScene("TutorialJefe");
    }

    public void JugarClick()
    {
        SceneManager.LoadScene("CambiarOleadas");
    }
    public void JefeFuego()
    {
        SceneManager.LoadScene("JefeFuego");
    }

    public void Oleada2()
    {
        SceneManager.LoadScene("Oleada2");
    }
    public void JefePlanta()
    {
        SceneManager.LoadScene("JefePlanta");
    }

    public void Oleada3()
    {
        SceneManager.LoadScene("Oleada3");
    }
    public void JefeRayo()
    {
        SceneManager.LoadScene("JefeRayo");
    }

    public void OleadaFinal()
    {
        SceneManager.LoadScene("OleadaFinal");
    }
    //public void JefeFinal()
    //{
    //    SceneManager.LoadScene("JefeFinal");
    //}
}
