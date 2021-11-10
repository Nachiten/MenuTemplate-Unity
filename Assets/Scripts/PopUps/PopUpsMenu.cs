using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
// ReSharper disable SwitchStatementHandlesSomeKnownEnumValuesWithDefault
// ReSharper disable SwitchStatementMissingSomeEnumCasesNoDefault

public enum PopUpMenu
{
    None = 4,
    ConfirmacionBorrado = 0,
    BorradoSatisfactorio = 1,
    BorradoCancelado = 2,
    ConfirmacionSalida = 3
}

public class PopUpsMenu : MonoBehaviour
{
    [FormerlySerializedAs("Textura")] 
    public Texture[] texturas;
    
    private static bool variablesSeteadas = false;

    private static RawImage simbolo;
    private static GameObject botonNo, popUp;
    private static TMP_Text textoPrincipal, textoBanner, botonSiTexto;
    
    private static Image panelPopUpImage;
    private static GameObject panelPopUp;
    
    private static PopUpMenu popUpOpen = PopUpMenu.None;
    private static int currentImage;
    
    private const float tiempoAnimacion = 0.18f;
    
    /* -------------------------------------------------------------------------------- */

    private void Awake()
    {
        if (variablesSeteadas)
            return;
        
        panelPopUp = GameObject.Find("PanelPopUp");
        panelPopUpImage = panelPopUp.GetComponent<Image>();

        popUp = GameObject.Find("Pop Up");
        botonNo = GameObject.Find("Boton No");

        textoBanner = GameObject.Find("Texto Banner").GetComponent<TMP_Text>();
        botonSiTexto = GameObject.Find("BotonSiTexto").GetComponent<TMP_Text>();
        textoPrincipal = GameObject.Find("Texto Principal").GetComponent<TMP_Text>();

        simbolo = GameObject.Find("Icono").GetComponent<RawImage>();
        
        Assert.IsNotNull(panelPopUp);
        Assert.IsNotNull(panelPopUpImage);
        
        Assert.IsNotNull(popUp);
        Assert.IsNotNull(botonNo);
        
        Assert.IsNotNull(textoBanner);
        Assert.IsNotNull(botonSiTexto);
        Assert.IsNotNull(textoPrincipal);
        
        Assert.IsNotNull(simbolo);
        
        variablesSeteadas = true;
        
        // ReSharper disable once MergeIntoNegatedPattern
        if (texturas == null || texturas.Length != 5) {
            Debug.LogError("[PopUpsMenu] Faltan las texturas correctas");
        }
    }

    /* -------------------------------------------------------------------------------- */

    private void Start()
    {
        popUp.SetActive(false);
        panelPopUp.SetActive(false);
    }

    /* -------------------------------------------------------------------------------- */

    public void abrirPopUp(PopUpMenu popUpOpened) {

        popUpOpen = popUpOpened;

        animarApertura();

        // Configs default
        botonNo.SetActive(false);
        currentImage = 0;
        botonSiTexto.text = "Ok";

        switch (popUpOpened)
        {
            case PopUpMenu.ConfirmacionBorrado:
                textoBanner.text = "Confirmación de Borrado";
                textoPrincipal.text = "Estas seguro de borrar TODO el progreso actual del juego?";
                botonSiTexto.text = "Si";
                botonNo.SetActive(true);
                break;
            
            case PopUpMenu.BorradoSatisfactorio:
                textoBanner.text = "Aviso Importante";
                textoPrincipal.text = "Todo el progreso fue borrado correctamente.\nSe reiniciara la escena para efectuar los cambios.";
                break;
            
            case PopUpMenu.BorradoCancelado:
                textoBanner.text = "Acción Cancelada";
                textoPrincipal.text = "La accion fue cancelada con exito.";
                currentImage = 4;
                break;
            
            case PopUpMenu.ConfirmacionSalida:
                textoBanner.text = "Confirmacion";
                textoPrincipal.text = "Estas seguro que deseas salir?";
                currentImage = 4;
                botonSiTexto.text = "Si";
                botonNo.SetActive(true);
                break;
        }

        simbolo.texture = texturas[currentImage];
    }

    /* -------------------------------------------------------------------------------- */

    private Color32 blanco = new Color32(255, 255, 255, 0);
    
    private void animarApertura()
    {
        // Pongo el panel del pop up
        panelPopUp.SetActive(true);
        LeanTween.value(0, 100, tiempoAnimacion).setOnUpdate(actualizarColor);
        
        // Muevo el pop up
        LeanTween.moveLocalX(popUp, -1500, 0f).setOnComplete(_ => popUp.SetActive(true));
        LeanTween.moveLocalX(popUp, 0, tiempoAnimacion);
    }

    /* -------------------------------------------------------------------------------- */

    private void actualizarColor(float value)
    {
        blanco.a = (byte)value;
        panelPopUpImage.color = blanco;
    }

    public void cerrarPopUp( bool accionUsada) // TRUE = si FALSE = no
    {
        // Quito el panel del pop up
        LeanTween.value(100, 0, tiempoAnimacion).setOnUpdate(actualizarColor).setOnComplete(() => panelPopUp.SetActive(false));
        
        LeanTween.moveLocalX(popUp, 1500, tiempoAnimacion).setOnComplete(_ => realizarAccionAlCerrar(accionUsada));
    }

    /* -------------------------------------------------------------------------------- */

    private void borrarTodasLasKeys()
    {
        Debug.LogWarning("[PopUpsMenu] BORRANDO TODAS LAS KEYS !!!!");
        PlayerPrefs.DeleteAll();
    }

    /* -------------------------------------------------------------------------------- */

    private void realizarAccionAlCerrar(bool accionUsada) 
    {
        // Cierro el popup
        popUp.SetActive(false);

        switch (popUpOpen) 
        {
            case PopUpMenu.ConfirmacionBorrado:
                if (accionUsada)
                {
                    borrarTodasLasKeys();
                    abrirPopUp(PopUpMenu.BorradoSatisfactorio);
                }
                else
                    abrirPopUp(PopUpMenu.BorradoCancelado);
                break;

            case PopUpMenu.BorradoSatisfactorio:
                LevelLoaderSingleton.singleton.cargarNivel(0);
                break;
            
            case PopUpMenu.ConfirmacionSalida:
                if (accionUsada)
                    LevelLoaderSingleton.singleton.salir();
                break;
        }
    }
}

