using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Assertions;

public class Controllers : MonoBehaviour
{
    public AudioMixer mixerMusica, mixerSonidos;

    private TMP_Text textoVolumenMusica, textoVolumenSonidos;

    private TMP_Dropdown seleccionarMusica;

    private Scrollbar scrollMusica, scrollSonidos;

    /* -------------------------------------------------------------------------------- */

    private void Awake()
    {
        textoVolumenMusica = GameObject.Find("NumeroVolumenMusica").GetComponent<TMP_Text>();
        textoVolumenSonidos = GameObject.Find("NumeroVolumenSonidos").GetComponent<TMP_Text>();
        
        scrollMusica = GameObject.Find("VolumenMusicaScroll").GetComponent<Scrollbar>();
        scrollSonidos = GameObject.Find("VolumenSonidosScroll").GetComponent<Scrollbar>();

        seleccionarMusica = GameObject.Find("SeleccionarMusica").GetComponent<TMP_Dropdown>();
        
        Assert.IsNotNull(textoVolumenMusica);
        Assert.IsNotNull(textoVolumenSonidos);
        
        Assert.IsNotNull(scrollMusica);
        Assert.IsNotNull(scrollSonidos);
        
        Assert.IsNotNull(seleccionarMusica);
    }

    /* -------------------------------------------------------------------------------- */

    private void Start()
    {
        // Seteo la cancion elegida previamente
        if (PlayerPrefs.HasKey("ChosenSong")) 
        {
            int cancionElegida = PlayerPrefs.GetInt("ChosenSong");

            // Si hay alguna cancion elegida, la reproduczo
            if (cancionElegida != 0)
                AudioManagerSingleton.singleton.reproducirMusica(cancionElegida - 1);

            seleccionarMusica.value = cancionElegida;
        }

        // Seteo el sonido elegido previamente
        if (PlayerPrefs.HasKey("SoundLevel")) 
        {
            float soundLevel = PlayerPrefs.GetFloat("SoundLevel");

            setearValorSonido(soundLevel);
            scrollSonidos.value = soundLevel;
        }

        // Seteo la musica elegida previamente
        // ReSharper disable once InvertIf
        if (PlayerPrefs.HasKey("MusicLevel"))
        {
            float musicLevel = PlayerPrefs.GetFloat("MusicLevel");

            setearValorMusica(musicLevel);
            scrollMusica.value = musicLevel;
        }
    }

    /* -------------------------------------------------------------------------------- */

    public void setMusicLevel(float valorSlider)
    {
        PlayerPrefs.SetFloat("MusicLevel", valorSlider);

        setearValorMusica(valorSlider);
    }

    /* -------------------------------------------------------------------------------- */

    private bool waiting = false;
    private const float waitTime = 0.55f;
    
    public void setSoundLevel(float valorSlider)
    {
        PlayerPrefs.SetFloat("SoundLevel", valorSlider);

        setearValorSonido(valorSlider);

        if (waiting || !Input.GetMouseButton(0))
            return;
        
        waiting = true;
        AudioManagerSingleton.singleton.reproducirSonido(0);
        LeanTween.value(0, waitTime, waitTime).setOnComplete(terminarEspera);
    }

    /* -------------------------------------------------------------------------------- */

    private void terminarEspera()
    {
        waiting = false;
    }

    /* -------------------------------------------------------------------------------- */

    private void setearValorSonido(float valorSonido) 
    {
        textoVolumenSonidos.text = (valorSonido * 100).ToString("F0");

        valorSonido = valorSonido * 0.9999f + 0.0001f;

        mixerSonidos.SetFloat("Volume", Mathf.Log10(valorSonido) * 20);
    }

    /* -------------------------------------------------------------------------------- */

    private void setearValorMusica(float valorMusica) 
    {
        textoVolumenMusica.text = (valorMusica * 100).ToString("F0");

        valorMusica = valorMusica * 0.9999f + 0.0001f;

        mixerMusica.SetFloat("Volume", Mathf.Log10(valorMusica) * 20);
    }

    /* -------------------------------------------------------------------------------- */

    public void cambiarCancionA(int cancion)
    {
        PlayerPrefs.SetInt("ChosenSong", cancion);

        if (cancion != 0) 
            AudioManagerSingleton.singleton.reproducirMusica(cancion - 1);
        else
            AudioManagerSingleton.singleton.pararMusica();
    }
}