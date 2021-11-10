using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class EditorTools : EditorWindow
{
    // Mostrar Ventana
    [MenuItem("Window/[EditorTools]")]
    public static void showWindow()
    {
        GetWindow<EditorTools>("EditorTools");
    }

    // --------------------------------------------------------------------------------

    private bool menuViajarEscenasAbierto = false;
    private bool menuGanarNivelAbierto = false;

    private int cantidadEscenas;

    private void OnEnable()
    {
        cantidadEscenas = SceneManager.sceneCountInBuildSettings;
    }

    // Codigo de la Vetana
    private void OnGUI()
    {
        if (!Application.isPlaying)
        {
            EditorGUILayout.LabelField("----------------------------------------------------------------------");
            EditorGUILayout.LabelField("------ Debes comenzar a jugar para ver las opciones de este menu.  ------");
            EditorGUILayout.LabelField("----------------------------------------------------------------------");
            return;
        }

        EditorGUILayout.LabelField("Viajar hacia escena:");

        string textoBoton = "Abrir Menu";
        if (menuViajarEscenasAbierto)
            textoBoton = "Cerrar Menu";

        if (GUILayout.Button(textoBoton))
        {
            menuViajarEscenasAbierto = !menuViajarEscenasAbierto;
        }

        if (menuViajarEscenasAbierto)
        {
            mostrarMenuViajarAEscena();
        }

        EditorGUILayout.LabelField("Ganar nivel especifico:");

        string textoBoton2 = "Abrir Menu";
        if (menuGanarNivelAbierto)
            textoBoton2 = "Cerrar Menu";

        if (GUILayout.Button(textoBoton2))
        {
            menuGanarNivelAbierto = !menuGanarNivelAbierto;
        }

        if (menuGanarNivelAbierto) 
        {
            mostrarMenuGanarNivel(false);
        }

        EditorGUILayout.LabelField("Borrar Todas las Keys:");

        if (GUILayout.Button("BORRAR TODO"))
        {
            PlayerPrefs.DeleteAll();

            if (SceneManager.GetActiveScene().buildIndex == 12)
            {
                LevelLoaderSingleton.singleton.cargarNivel(SceneName.LevelSelector);
            }
        }

        EditorGUILayout.LabelField("Ganar todos los niveles:");

        if (GUILayout.Button("Ganar Todo"))
        {
            mostrarMenuGanarNivel(true);
        }
    }

    private void mostrarMenuViajarAEscena() 
    {
        for (int i = 0; i < cantidadEscenas; i++)
        {
            string nombreEscena = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));

            if (GUILayout.Button("Ir a escena: [" + nombreEscena + "]"))
                LevelLoaderSingleton.singleton.cargarNivel((SceneName)i);
            
        }
    }

    private void mostrarMenuGanarNivel(bool ganaDirecto) 
    {
        for (int i = 0; i < cantidadEscenas; i++)
        {
            if (i == 0 || i == 12)
                continue;

            string nombreEscena = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));

            if (!GUILayout.Button("Ganar Nivel: [" + nombreEscena + "]") && !ganaDirecto) 
                continue;
            
            PlayerPrefs.SetString(i.ToString(), "Ganado");
            PlayerPrefs.SetFloat("Time_" + i, 25000f);
            PlayerPrefs.SetInt("Movements_" + i, 137);

            if (SceneManager.GetActiveScene().buildIndex == 12 && !ganaDirecto)
                LevelLoaderSingleton.singleton.cargarNivel(SceneName.LevelSelector);
            
        }

        if (SceneManager.GetActiveScene().buildIndex == 12 && ganaDirecto)
            LevelLoaderSingleton.singleton.cargarNivel(SceneName.LevelSelector);
        
    }
}

