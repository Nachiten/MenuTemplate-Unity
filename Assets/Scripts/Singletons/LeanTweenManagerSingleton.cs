using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class LeanTweenManagerSingleton : MonoBehaviour
{
    #region Variables

    private const float tiempoAnimacionBotonesMenu = 0.2f, tiempoAnimacionPanelMenu = 0.15f, tiempoAnimacionMenus = 0.5f, posicionAfuera = 1920;

    private List<GameObject> botones;

    private static GameObject menu, menuPanel, menuOpciones, menuCreditos, botonesInicio;
    private static GameObject botonComenzar, botonSeleccionarNivel, botonOpciones, botonSalir, botonVolverInicio, textoMenu;

    public bool animacionEnEjecucion = false;

    private static bool variablesSeteadas = false;
    
    private bool esInicio;

    #endregion

    /* -------------------------------------------------------------------------------- */

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /* -------------------------------------------------------------------------------- */

    // Setup que se hace una sola vez
    private void Awake()
    {
        if (variablesSeteadas)
            return;
        
        // Objetos
        menu = GameObject.Find("Menu");
        menuPanel = GameObject.Find("PanelMenu");
        menuOpciones = GameObject.Find("MenuOpciones");
        menuCreditos = GameObject.Find("MenuCreditos");

        // Botones
        botonSalir = GameObject.Find("Salir");
        botonComenzar = GameObject.Find("Comenzar");
        botonOpciones = GameObject.Find("Opciones");
        botonesInicio = GameObject.Find("Botones Inicio");

        // Botonces condicionales
        botonVolverInicio = GameObject.Find("VolverAInicio");
        botonSeleccionarNivel = GameObject.Find("Seleccionar Nivel");
        
        textoMenu = GameObject.Find("TextoMenu");

        Assert.IsNotNull(menu);
        Assert.IsNotNull(menuPanel);
        Assert.IsNotNull(menuOpciones);
        Assert.IsNotNull(menuCreditos);
        
        Assert.IsNotNull(botonSalir);
        Assert.IsNotNull(botonComenzar);
        Assert.IsNotNull(botonOpciones);
        Assert.IsNotNull(botonesInicio);
        
        Assert.IsNotNull(botonVolverInicio);
        Assert.IsNotNull(botonSeleccionarNivel);
        
        variablesSeteadas = true;
       
    }

    /* -------------------------------------------------------------------------------- */

    public static LeanTweenManagerSingleton singleton = null;
    
    private void inicializeInstance()
    {
        if (singleton == null)
            singleton = this;
        
        else if (singleton != this)
            Destroy(this);
    }
    
    // Se llama cuando una nueva escena se carga
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        inicializeInstance();
        
        setupInicial();
    }

    /* -------------------------------------------------------------------------------- */

    // Setup que se hace en cada nueva escena cargada
    private void setupInicial()
    {
        esInicio = SceneManagerSingleton.singleton.currentSceneIs(SceneName.Inicio);

        botones = new List<GameObject> { botonSalir, botonComenzar, botonOpciones };

        // Si estoy en inicio
        if (esInicio)
        {
            botonVolverInicio.SetActive(false);
            botonesInicio.SetActive(true);
            textoMenu.SetActive(false);
        }
        else
        {
            textoMenu.SetActive(true);
            botones.Add(textoMenu);
            botones.Add(botonVolverInicio);
            botones.Add(botonSeleccionarNivel);
            botonesInicio.SetActive(false);
        }
    }

    #region AnimacionAbrirMenu

    /* --------------------------------------------------------------------------------- */
    // ----------------------------- ANIMACION ARBRIR MENU ----------------------------- //
    /* --------------------------------------------------------------------------------- */

    public void abrirMenu()
    {
        menuPanel.SetActive(false);

        foreach (GameObject boton in botones) {
            boton.SetActive(false);
        }

        animacionEnEjecucion = true;

        // Posicion inicial
        LeanTween.scale(menuPanel, new Vector3(0, 0, 1), 0f).setOnComplete(abrirPanel);
    }

    private void abrirPanel()
    {
        menuPanel.SetActive(true);
        LeanTween.scale(menuPanel, new Vector3(1, 1, 1), tiempoAnimacionPanelMenu).setOnComplete(abrirBotones);
    }

    private void abrirBotones()
    {
        int cantidadBotones = botones.Count;

        for (int i = 0; i < cantidadBotones; i++)
        {
            GameObject boton = botones[i];

            bool terminarAnimacion = i == cantidadBotones - 1;

            // Posiciones iniciales
            LeanTween.scale(boton, new Vector3(0, 0.2f, 1), 0f).setOnComplete(_ => abrirBotonEnX(boton, terminarAnimacion));
        }
    }

    private void abrirBotonEnX(GameObject boton, bool terminarAnimacion)
    {
        boton.SetActive(true);
        LeanTween.scaleX(boton, 2.3f, tiempoAnimacionBotonesMenu).setOnComplete(_ => abrirBotonEnY(boton, terminarAnimacion));
    }

    private void abrirBotonEnY(GameObject unBoton, bool terminarAnimacion)
    {
        if (terminarAnimacion)
            LeanTween.scaleY(unBoton, 3.1f, tiempoAnimacionBotonesMenu).setOnComplete(terminarAnimacionAbrir);
        else
            LeanTween.scaleY(unBoton, 3.1f, tiempoAnimacionBotonesMenu);
    }

    private void terminarAnimacionAbrir()
    {
        animacionEnEjecucion = false;
    }
    
    #endregion

    #region AnimacionCerrarMenu

    /* --------------------------------------------------------------------------------- */
    // ----------------------------- ANIMACION CERRAR MENU ----------------------------- // 
    /* --------------------------------------------------------------------------------- */

    public void cerrarMenu()
    {
        animacionEnEjecucion = true;

        int cantidadBotones = botones.Count;

        for (int i = 0; i < cantidadBotones; i++)
        {
            GameObject botonActual = botones[i];

            bool cerrarMenu = i == cantidadBotones - 1;

            // Posiciones iniciales
            LeanTween.scale(botonActual, new Vector3(2.3f, 3.1f, 1), 0f).setOnComplete(_ => cerrarBotonEnY(botonActual, cerrarMenu));
        }

    }

    private void cerrarBotonEnY(GameObject boton, bool cerrarMenu) {
        LeanTween.scaleY(boton, 0.2f, tiempoAnimacionBotonesMenu).setOnComplete(_ => cerrarBotonEnX(boton, cerrarMenu));
    }

    private void cerrarBotonEnX(GameObject unBoton, bool cerrarMenu)
    {
        if (cerrarMenu)
            LeanTween.scaleX(unBoton, 0f, tiempoAnimacionBotonesMenu).setOnComplete(cerrarPanel);
        else
            LeanTween.scaleX(unBoton, 0f, tiempoAnimacionBotonesMenu);
    }

    private void cerrarPanel()
    {
        // Posicion inicial
        LeanTween.scale(menuPanel, new Vector3(1, 1, 1), 0f);

        LeanTween.scale(menuPanel, new Vector3(0, 0, 1), tiempoAnimacionPanelMenu).setOnComplete(terminarAnimacionCerrar);
    }

    private void terminarAnimacionCerrar()
    {
        animacionEnEjecucion = false;
        menu.SetActive(false);
    }

    #endregion

    #region AnimacionesAbrirMenus

    /* ------------------------------------------------------------------------------------- */
    // ------------------------------ ANIMACIONES ABRIR MENUS ------------------------------ // 
    /* ------------------------------------------------------------------------------------- */

    private void abrirMenu(GameObject menuAPoner, int signo)
    {
        animacionEnEjecucion = true;

        if (esInicio)
            LeanTween.moveLocalX(botonesInicio, 0f, 0f).setOnComplete(_ => quitarBotonesInicio(posicionAfuera * signo));
        

        // Posicion Inicial
        LeanTween.moveLocalX(menu, 0f, 0f).setOnComplete(_ => quitarMenu(posicionAfuera * signo));
        LeanTween.moveLocalX(menuAPoner, -posicionAfuera * signo, 0f).setOnComplete(_ => ponerMenu(menuAPoner));
    }

    private void quitarBotonesInicio(float posicion)
    {
        LeanTween.moveLocalX(botonesInicio, posicion, tiempoAnimacionMenus).setOnComplete(ocultarBotonesInicio);
    }

    private void ocultarBotonesInicio()
    {
        botonesInicio.SetActive(false);
    }

    private void quitarMenu(float posicion)
    {
        LeanTween.moveLocalX(menu, posicion, tiempoAnimacionMenus).setOnComplete(ocultarMenu);
    }

    private void ocultarMenu()
    {
        menu.SetActive(false);
        animacionEnEjecucion = false;
    }

    private void ponerMenu(GameObject menuAPoner)
    {
        menuAPoner.SetActive(true);
        LeanTween.moveLocalX(menuAPoner, 0f, tiempoAnimacionMenus);
    }

    public void abrirOpciones()
    {
        abrirMenu(menuOpciones, 1);
    }

    public void abrirCreditos()
    {
        abrirMenu(menuCreditos, -1);
    }

    #endregion

    #region AnimacionesCerrarMenus

    /* -------------------------------------------------------------------------------------- */
    // ------------------------------ ANIMACIONES CERRAR MENUS ------------------------------ // 
    /* -------------------------------------------------------------------------------------- */

    private void cerrarMenu(GameObject menuACerrar, int signo)
    {
        animacionEnEjecucion = true;

        if (esInicio)
            LeanTween.moveLocalX(botonesInicio, posicionAfuera * signo, 0f).setOnComplete(ponerBotonesInicio);
        

        // Posicion Inicial
        LeanTween.moveLocalX(menu, posicionAfuera * signo, 0f).setOnComplete(ponerMenu);
        LeanTween.moveLocalX(menuACerrar, 0, 0f).setOnComplete(_ => quitarMenuOtro(menuACerrar, -posicionAfuera * signo));
    }

    private void ponerBotonesInicio()
    {
        botonesInicio.SetActive(true);
        LeanTween.moveLocalX(botonesInicio, 0, tiempoAnimacionMenus);
    }

    private void ponerMenu()
    {
        menu.SetActive(true);
        LeanTween.moveLocalX(menu, 0, tiempoAnimacionMenus);
    }

    private void quitarMenuOtro(GameObject menuAQuitar, float posicion)
    {
        LeanTween.moveLocalX(menuAQuitar, posicion, tiempoAnimacionMenus).setOnComplete(_ => ocultarMenuOtro(menuAQuitar));
    }

    private void ocultarMenuOtro(GameObject menuAQuitar)
    {
        menuAQuitar.SetActive(false);
        animacionEnEjecucion = false;
    }

    public void cerrarOpciones()
    {
        cerrarMenu(menuOpciones, 1);
    }

    public void cerrarCreditos()
    {
        cerrarMenu(menuCreditos, -1);
    }

    #endregion
}