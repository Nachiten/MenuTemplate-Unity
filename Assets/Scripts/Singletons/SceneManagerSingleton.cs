using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    Inicio = 0,
    Nivel1 = 1,
    LevelSelector = 2
}

public class SceneManagerSingleton : MonoBehaviour
{
    private SceneName actualSceneName;
    
    /* ------------------------------ SINGLETON -------------------------------------------- */
    
    public static SceneManagerSingleton singleton = null;
    
    private void inicializeInstance()
    {
        if (singleton == null)
            singleton = this;
        
        else if (singleton != this)
            Destroy(this);
    }
    
    /* ------------------------------------------------------------------------------------- */
    
    private void Awake()
    {
        inicializeInstance();
    }
    
    public bool currentSceneIs(SceneName sceneName)
    {
        setActualScene();
        return actualSceneName == sceneName;
    }

    public SceneName getCurrentScene()
    {
        setActualScene();
        return actualSceneName;
    }
    
    public string getCurrentSceneName()
    {
        setActualScene();
        return SceneManager.GetSceneByBuildIndex((int)actualSceneName).name;
    }

    private bool sceneIsBetween(SceneName minInclusive, SceneName maxInclusive, SceneName sceneName)
    {
        setActualScene();
        return sceneName >= minInclusive && sceneName <= maxInclusive;
    }

    private void setActualScene()
    {
        actualSceneName = (SceneName)SceneManager.GetActiveScene().buildIndex;
    }
}
