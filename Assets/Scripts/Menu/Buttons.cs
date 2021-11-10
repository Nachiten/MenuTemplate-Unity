using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    [HideInInspector]
    public SceneName nivelACargar = SceneName.Nivel1;

    private static PopUpsMenu codigoPopUpsMenu;
    private static ManejarMenu codigoManejarMenu;

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

    // Setup que se hace por unica vez
    private void Awake()
    {
        codigoPopUpsMenu = GameObject.Find("Pop Up").GetComponent<PopUpsMenu>();

        Assert.IsNotNull(codigoPopUpsMenu);
    }

    /* -------------------------------------------------------------------------------- */

    // Se llama cuando una nueva escena se carga
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        setupInicial();
    }

    /* -------------------------------------------------------------------------------- */

    // Setup que se hace en cada nueva escena cargada
    private void setupInicial()
    {
        codigoManejarMenu = GameObject.Find("GameManager").GetComponent<ManejarMenu>();

        Assert.IsNotNull(codigoManejarMenu);
    }

    #region Botones

    /* --------------------------------------------------------------------------------- */
    /* ------------------------------------ BOTONES ------------------------------------ */
    /* --------------------------------------------------------------------------------- */

    public void botonComenzar()
    {
        if (!puedeClickear())
            return;
        
        reproducirSonidoClickBoton();

        if (SceneManagerSingleton.singleton.currentSceneIs(SceneName.Inicio))
        {
            PlayerPrefs.SetString("YaJugoAntes", "Si");
            loadLevel(nivelACargar);
        }
        else
            codigoManejarMenu.manejarMenu();
    }

    public void botonOpciones()
    {
        if (!puedeClickear())
            return;
        
        reproducirSonidoClickBoton();

        codigoManejarMenu.manejarOpciones();
    }

    public void botonSalir()
    {
        if (!puedeClickear())
            return;
        
        reproducirSonidoClickBoton();

        codigoPopUpsMenu.abrirPopUp(PopUpMenu.ConfirmacionSalida);
    }

    public void botonVolverAInicio()
    {
        if (!puedeClickear())
            return;
        
        reproducirSonidoClickBoton();

        loadLevel(0);
    }

    public void botonBorrarProgreso()
    {
        if (!puedeClickear())
            return;
        
        reproducirSonidoClickBoton();

        codigoPopUpsMenu.abrirPopUp(PopUpMenu.ConfirmacionBorrado);
    }

    public void botonCreditos()
    {
        if (!puedeClickear())
            return;
        
        reproducirSonidoClickBoton();

        codigoManejarMenu.manejarCreditos();
    }

    public void botonSeleccionarNivel()
    {
        if (!puedeClickear())
            return;
        
        reproducirSonidoClickBoton();

        throw new NotImplementedException("Falta implementar este boton!!!");
    }

    #endregion

    /* ------------------------------------------------------------------------------------ */
    /* ------------------------------------ AUXILIARES ------------------------------------ */
    /* ------------------------------------------------------------------------------------ */

    
    private bool habilitado = true;
    private const float retardo = 0.3f;
    
    private bool puedeClickear()
    {
        // Deshabilito para evitar mas de un click
        if (!habilitado)
            return false;
        
        habilitado = false;
        
        // Vuelvo a habilitar botones despues de un retardo
        LeanTween.value(0, retardo, retardo).setOnComplete(() => habilitado = true);

        return true;
    }
    
    private void loadLevel(SceneName index) 
    {
        LevelLoaderSingleton.singleton.cargarNivel(index); 
    }

    /* ------------------------------------------------------------------------------------ */

    private void reproducirSonidoClickBoton()
    {
        AudioManagerSingleton.singleton.reproducirSonido(1);
    }
}