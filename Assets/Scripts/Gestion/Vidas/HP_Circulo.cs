using UnityEngine;
using UnityEngine.SceneManagement;

public class HP_Circulo : MonoBehaviour
{
    public static HP_Circulo Instance;

    [SerializeField] private float vida;
    [SerializeField] private float maxVida = 100;
    [SerializeField] private HP_Bar hpBar;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        vida = maxVida;
        hpBar.InicializarBarraVida(maxVida);
    }

    public void Curar(float cantidad)
    {
        vida = Mathf.Clamp(vida + cantidad, 0, maxVida);
        hpBar.CambiarVidaActual(vida);
        Debug.Log("Circle curado en " + cantidad + " puntos.");
    }

    public void RecibirDamageCirculo(float damage)
    {
        vida = Mathf.Max(vida - damage, 0);
        hpBar.CambiarVidaActual(vida);

        EventManager.TriggerEvent("CircleDamaged");

        if (vida <= 0)
        {
            CargarMenuInicio();
        }
    }


    private void CargarMenuInicio()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("MenuInicio");
    }
}
