using UnityEngine;
using UnityEngine.Assertions;

public class AudioManagerSingleton : MonoBehaviour
{
    public AudioClip[] clipsMusica;
    public AudioClip[] clipsSonido;

    public AudioSource sourceMusica;
    public AudioSource sourceSonido;
    
    /* ------------------------------ SINGLETON -------------------------------------------- */
    
    public static AudioManagerSingleton singleton = null;
    
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
        
        Assert.IsNotNull(sourceSonido);
        Assert.IsNotNull(sourceMusica);
        
        Assert.AreNotEqual(0, clipsMusica.Length);
        Assert.AreNotEqual(0, clipsSonido.Length);
    }

    /* ------------------------------------------------------------------------------------- */
    
    public void reproducirSonido(int sonido)
    {
        sourceSonido.clip = clipsSonido[sonido];
        sourceSonido.Play();
    }
    
    /* ------------------------------------------------------------------------------------- */

    public void reproducirMusica(int musica)
    {
        sourceMusica.Stop();

        sourceMusica.clip = clipsMusica[musica];
        sourceMusica.Play();
    }

    /* ------------------------------------------------------------------------------------- */

    public void pararMusica()
    {
        sourceMusica.Stop();
    }
}