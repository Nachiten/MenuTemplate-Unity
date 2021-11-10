using UnityEngine;
using UnityEngine.Assertions;
using TMPro;

public class ManejarMenu : MonoBehaviour
{
    #region Variables

    // Flags varios
    private bool menuActivo = false, opcionesActivas = false, creditosActivos = false, desactivado = false;

    // Flag de ya asigne las variables
    private static bool variablesAsignadas = false;

    // GameObjects
    private static GameObject menu, opciones, creditos, panelMenu;

    // Textos varios
    private static TMP_Text textoBoton;

    // Strings utilizados
    private const string continuar = "CONTINUAR", comenzar = "COMENZAR";

    private SceneName escenaActual;
    
    #endregion

    /* -------------------------------------------------------------------------------- */

    #region FuncionStart

    private void Awake()
    {
        if (variablesAsignadas) 
            return;
        
        menu = GameObject.Find("Menu");
        panelMenu = GameObject.Find("PanelMenu");
        opciones = GameObject.Find("MenuOpciones");
        creditos = GameObject.Find("MenuCreditos");

        textoBoton = GameObject.Find("TextoBotonComenzar").GetComponent<TMP_Text>();

        Assert.IsNotNull(menu);
        Assert.IsNotNull(panelMenu);
        Assert.IsNotNull(opciones);
        Assert.IsNotNull(creditos);
        Assert.IsNotNull(textoBoton);
        
        variablesAsignadas = true;
    }

    private bool estoyEnInicio;
    
    /* -------------------------------------------------------------------------------- */

    private void Start()
    {
        escenaActual = SceneManagerSingleton.singleton.getCurrentScene();

        estoyEnInicio = escenaActual == SceneName.Inicio;
        
        // No estoy en la escena "Inicio"
        if (!estoyEnInicio)
        {
            // Al cerrar el menu continuas el nivel
            textoBoton.text = continuar;

            // Oculto las cosas de una patada pq se esta mostrando la pantalla de carga
            menu.SetActive(false);
            panelMenu.SetActive(false);
        }
        // Estoy en la escena "Inicio"
        else
        {
            // El boton comienza el juego
            textoBoton.text = comenzar;
            
            // El menu se muestra en el inicio siempre
            menu.SetActive(true);
            menuActivo = true;
        }

        opciones.SetActive(false);
        creditos.SetActive(false);
    }

    #endregion

    /* -------------------------------------------------------------------------------- */

    #region FuncionUpdate

    private void Update()
    {
        if (estoyEnInicio) 
            return;

        bool animacionEnEjecucion = LeanTweenManagerSingleton.singleton.animacionEnEjecucion;

        if (Input.GetKeyDown("escape") && !animacionEnEjecucion)
            manejarMenu();
    }

    #endregion

    /* -------------------------------------------------------------------------------- */

    public void toggleMenu()
    {
        desactivado = !desactivado;
    }

    public void manejarMenu()
    {
        if (desactivado)
            return;

        menuActivo = !menuActivo;

        // Abro menu
        if (menuActivo)
        {
            menu.SetActive(true);
            LeanTweenManagerSingleton.singleton.abrirMenu();
        }
        // Cierro menu
        else
            LeanTweenManagerSingleton.singleton.cerrarMenu();

        // ReSharper disable once InvertIf
        if (opcionesActivas)
        {
            LeanTweenManagerSingleton.singleton.cerrarOpciones();
            opcionesActivas = false;
        }
    }
    
    /* -------------------------------------------------------------------------------- */

    public void manejarOpciones()
    {
        opcionesActivas = !opcionesActivas;

        if (opcionesActivas)
            LeanTweenManagerSingleton.singleton.abrirOpciones();
        
        else
            LeanTweenManagerSingleton.singleton.cerrarOpciones();
        
    }

    /* -------------------------------------------------------------------------------- */
    public void manejarCreditos()
    {
        creditosActivos = !creditosActivos;

        if (creditosActivos)
            LeanTweenManagerSingleton.singleton.abrirCreditos();
        else
            LeanTweenManagerSingleton.singleton.cerrarCreditos();

    }
}

