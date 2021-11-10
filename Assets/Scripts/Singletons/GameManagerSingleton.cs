using UnityEngine;
using TMPro;
using UnityEngine.Assertions;

public class GameManagerSingleton : MonoBehaviour
{
    #region Variables

    private GameObject boton, bloquePrefab;
    private TMP_Text textoNivel;
    
    // Numero de escena actual
    private SceneName escenaActual;

    #endregion

    /* -------------------------------------------------------------------------------- */

    public static GameManagerSingleton singleton = null;
    
    private void inicializeInstance()
    {
        if (singleton == null)
            singleton = this;
        
        else if (singleton != this)
            Destroy(this);
    }
    
    /* -------------------------------------------------------------------------------- */

    private void Awake()
    {
        inicializeInstance();
        
        GameObject referencia = GameObject.Find("_Reference");
        Assert.IsNotNull(referencia);

        Destroy(referencia);
    }

    /* -------------------------------------------------------------------------------- */

    private void Start()
    {
        escenaActual = SceneManagerSingleton.singleton.getCurrentScene();
        
        string nombreNivel = SceneManagerSingleton.singleton.getCurrentSceneName();

        // Mostrar texto del nivel actual
        textoNivel = GameObject.Find("Nivel").GetComponent<TMP_Text>();
        textoNivel.text = nombreNivel;

        // Asignar variables
        boton = GameObject.Find("Boton");

        boton.SetActive(false);
    }

    /* -------------------------------------------------------------------------------- */
}
