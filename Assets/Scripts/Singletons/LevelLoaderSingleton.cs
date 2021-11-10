using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Assertions;

public class LevelLoaderSingleton : MonoBehaviour
{
    private static GameObject levelLoader, panelCargaColor, restoPanelCarga;
    private static Slider slider;
    private static TMP_Text textoProgreso, textoNivel;

    private static bool variablesAsignadas = false;

    private SceneName escenaACargar;

    /* -------------------------------------------------------------------------------- */

    public static LevelLoaderSingleton singleton = null;
    
    private void inicializeInstance()
    {
        if (singleton == null)
            singleton = this;

        else if (singleton != this)
            Destroy(this);
    }
    
    #region FuncionesInicio

    private void Awake()
    {
        inicializeInstance();
        
        if (variablesAsignadas)
            return;

        // Asignar variables
        slider = GameObject.Find("Barra Carga").GetComponent<Slider>();
        
        levelLoader = GameObject.Find("Panel Carga");
        panelCargaColor = GameObject.Find("PanelColorCarga");
        restoPanelCarga = GameObject.Find("RestoPanelCarga");

        textoProgreso = GameObject.Find("TextoProgreso").GetComponent<TMP_Text>();
        textoNivel = GameObject.Find("Texto Cargando").GetComponent<TMP_Text>();

        Assert.IsNotNull(slider);
        
        Assert.IsNotNull(levelLoader);
        Assert.IsNotNull(panelCargaColor);
        Assert.IsNotNull(restoPanelCarga);
        
        Assert.IsNotNull(textoProgreso);
        Assert.IsNotNull(textoNivel);
            
    }

    /* -------------------------------------------------------------------------------- */

    private void Start()
    {
        if (!variablesAsignadas)
        {
            levelLoader.SetActive(false);
            variablesAsignadas = true;
        }
        else
            quitarPanelCarga();
    }

    #endregion
    
    /* -------------------------------------------------------------------------------- */

    // Llamar a Corutina
    public void cargarNivel(SceneName sceneName)
    {
        escenaACargar = sceneName;
        textoNivel.text = "...";

        ponerPanelCarga();
    }

    public void cargarNivelPorIndex(int index)
    {
        cargarNivel((SceneName) index);
    }

    /* -------------------------------------------------------------------------------- */

    // Iniciar Corutina para cargar nivel en background
    private IEnumerator cargarAsincronizadamente()
    {
        // Iniciar carga de escena
        AsyncOperation operacion = SceneManager.LoadSceneAsync((int)escenaACargar);

        operacion.allowSceneActivation = true;

        Debug.Log("[LevelLoader] Cargando Escena: " + escenaACargar);

        // Desde aca si encuentra la escena correcta (no se pq)
        string nombreEscena = SceneManager.GetSceneByBuildIndex((int)escenaACargar).name;
        //Debug.Log("Escena que se carga: " + nombreEscena);
        textoNivel.text = "Cargando " + nombreEscena + " ...";

        // Mientras la operacion no este terminada
        while (!operacion.isDone)
        {
            // Generar valor entre 0 y 1
            float progress = Mathf.Clamp01(operacion.progress / 0.9f);
            // Modificar Slider
            slider.value = progress;
            // Modificar texto progreso
            textoProgreso.text = progress * 100f + "%";

            yield return null;
        }
    }

    /* -------------------------------------------------------------------------------- */

    public void salir() { Application.Quit(); }

    /* -------------------------------------------------------------------------------- */

    #region AnimacionPonerPanelCarga

    /* ----------------------------------------------------------------------------------------- */
    // ------------------------------ ANIMACION PONER PANEL CARGA ------------------------------ // 
    /* ----------------------------------------------------------------------------------------- */

    private const float tiempoAnimacionColorPanel = 0.3f; // 0.3
    private const float tiempoAnimacionRestoPanel = 0.2f; // 0.2

    private void ponerPanelCarga()
    {
        restoPanelCarga.SetActive(false);

        LeanTween.value(levelLoader, 0, 0, 0f)
            .setOnUpdate(mostrarColorPanelAlfa)
            .setOnComplete(iniciarMostrarPanelCarga);  
    }

    private void mostrarColorPanelAlfa(float value) 
    {
        panelCargaColor.GetComponent<Image>().color = new Color(0.149f, 0.149f, 0.149f, value);
    }

    private void iniciarMostrarPanelCarga() 
    {
        levelLoader.SetActive(true);

        LeanTween.value(levelLoader, 0, 1, tiempoAnimacionColorPanel)
                .setOnUpdate(mostrarColorPanelAlfa)
                .setOnComplete(mostrarPanelCarga);
    }

    private void mostrarPanelCarga()
    {
        LeanTween.scaleY(restoPanelCarga, 0, 0f).setOnComplete(mostrarRestoPanelCarga);
    }

    private void mostrarRestoPanelCarga() 
    {
        restoPanelCarga.SetActive(true);
        LeanTween.scaleY(restoPanelCarga, 1, tiempoAnimacionRestoPanel).setOnComplete(completarCargaNivel);
    }

    private void completarCargaNivel() { StartCoroutine(cargarAsincronizadamente()); }

    #endregion

    #region AnimacionQuitarPanelCarga

    /* ---------------------------------------------------------------------------------------- */
    // ----------------------------- ANIMACION QUITAR PANEL CARGA ----------------------------- // 
    /* ---------------------------------------------------------------------------------------- */

    private void quitarPanelCarga() 
    {
        LeanTween.scaleY(restoPanelCarga, 0, tiempoAnimacionRestoPanel).setOnComplete(ocultarColorPanelAlfa);
    }

    private void ocultarColorPanelAlfa() 
    {
        LeanTween.value(levelLoader, 1, 0, tiempoAnimacionColorPanel)
            .setOnUpdate(mostrarColorPanelAlfa)
            .setOnComplete(esconderPanelCarga);
    }

    private void esconderPanelCarga() { levelLoader.SetActive(false); }

    #endregion
}
