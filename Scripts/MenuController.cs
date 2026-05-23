using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// MenuController — Gerencia o Menu Principal e o Menu de Pausa do Duelo do Saber.
///
/// Como usar no Unity:
/// MENU PRINCIPAL (cena MenuPrincipal):
///   1. Crie um GameObject "Menu Controller" e anexe este script.
///   2. Conecte os botões e painéis do Inspector.
///
/// PAUSA (cena Batalha):
///   1. Crie um segundo GameObject "Menu Controller" na cena Batalha.
///   2. Ative apenas os campos do grupo Pausa no Inspector.
///   3. A pausa é ativada via tecla ESC ou botão botaoPausa na HUD.
/// </summary>
public class MenuController : MonoBehaviour
{
    // ─────────────────────────────────────────
    // MENU PRINCIPAL
    // ─────────────────────────────────────────
    [Header("Menu Principal — Botões")]
    public Button botaoJogar;
    public Button botaoOpcoes;
    public Button botaoSair;

    [Header("Menu Principal — Painel de Opções")]
    public GameObject painelOpcoes;
    public Slider sliderMusica;
    public Slider sliderEfeitos;
    public Button botaoFecharOpcoes;

    // ─────────────────────────────────────────
    // MENU DE PAUSA
    // ─────────────────────────────────────────
    [Header("Pausa — Painel e Botões")]
    public GameObject painelPausa;
    public Button botaoPausa;          // Botão na HUD (ícone ⏸)
    public Button botaoContinuar;
    public Button botaoMenuPrincipal;
    public Button botaoReiniciar;

    // ─────────────────────────────────────────
    // ESTADO
    // ─────────────────────────────────────────
    private bool estaPausado = false;

    // ─────────────────────────────────────────
    // START
    // ─────────────────────────────────────────
    void Start()
    {
        // ── Menu Principal ──
        botaoJogar?.onClick.AddListener(AoClicarJogar);
        botaoOpcoes?.onClick.AddListener(AbrirOpcoes);
        botaoSair?.onClick.AddListener(AoClicarSair);
        botaoFecharOpcoes?.onClick.AddListener(FecharOpcoes);

        // Sliders de volume
        if (sliderMusica != null)
        {
            sliderMusica.minValue = 0f;
            sliderMusica.maxValue = 1f;
            sliderMusica.value    = AudioManager.Instance != null
                ? AudioManager.Instance.volumeTrilha : 0.5f;
            sliderMusica.onValueChanged.AddListener(AoMoverSliderMusica);
        }

        if (sliderEfeitos != null)
        {
            sliderEfeitos.minValue = 0f;
            sliderEfeitos.maxValue = 1f;
            sliderEfeitos.value    = AudioManager.Instance != null
                ? AudioManager.Instance.volumeEfeitos : 1f;
            sliderEfeitos.onValueChanged.AddListener(AoMoverSliderEfeitos);
        }

        // Garante que o painel de opções começa fechado
        if (painelOpcoes != null) painelOpcoes.SetActive(false);

        // ── Pausa ──
        botaoPausa?.onClick.AddListener(AlternarPausa);
        botaoContinuar?.onClick.AddListener(Retomar);
        botaoMenuPrincipal?.onClick.AddListener(IrParaMenuPrincipal);
        botaoReiniciar?.onClick.AddListener(Reiniciar);

        if (painelPausa != null) painelPausa.SetActive(false);

        // Toca trilha de menu se estiver na cena de menu
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MenuPrincipal")
            AudioManager.Instance?.TocarTrilhaMenu();
        else
            AudioManager.Instance?.TocarTrilhaBatalha();
    }

    // ─────────────────────────────────────────
    // UPDATE — detecta ESC para pausar
    // ─────────────────────────────────────────
    void Update()
    {
        if (painelPausa != null && UnityEngine.InputSystem.Keyboard.current != null &&
            UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
            AlternarPausa();
    }

    // ─────────────────────────────────────────
    // MENU PRINCIPAL — AÇÕES
    // ─────────────────────────────────────────
    void AoClicarJogar()
    {
        AudioManager.Instance?.TocarClique();

        if (TransicaoDeCena.Instance != null)
            TransicaoDeCena.Instance.CarregarCena("Batalha");
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene("Batalha");
    }

    void AbrirOpcoes()
    {
        AudioManager.Instance?.TocarClique();
        if (painelOpcoes != null) painelOpcoes.SetActive(true);
    }

    void FecharOpcoes()
    {
        AudioManager.Instance?.TocarClique();
        if (painelOpcoes != null) painelOpcoes.SetActive(false);
    }

    void AoClicarSair()
    {
        AudioManager.Instance?.TocarClique();
        Debug.Log("[MenuController] Saindo do jogo...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // ─────────────────────────────────────────
    // SLIDERS DE VOLUME
    // ─────────────────────────────────────────
    void AoMoverSliderMusica(float valor)
    {
        AudioManager.Instance?.DefinirVolumeTrilha(valor);
    }

    void AoMoverSliderEfeitos(float valor)
    {
        AudioManager.Instance?.DefinirVolumeEfeitos(valor);
    }

    // ─────────────────────────────────────────
    // PAUSA — AÇÕES
    // ─────────────────────────────────────────
    void AlternarPausa()
    {
        if (estaPausado) Retomar();
        else             Pausar();
    }

    void Pausar()
    {
        estaPausado      = true;
        Time.timeScale   = 0f;
        if (painelPausa != null) painelPausa.SetActive(true);
        AudioManager.Instance?.TocarClique();
        Debug.Log("[MenuController] Jogo pausado.");
    }

    void Retomar()
    {
        estaPausado     = false;
        Time.timeScale  = 1f;
        if (painelPausa != null) painelPausa.SetActive(false);
        AudioManager.Instance?.TocarClique();
        Debug.Log("[MenuController] Jogo retomado.");
    }

    void IrParaMenuPrincipal()
    {
        Time.timeScale = 1f; // Restaura antes de sair
        AudioManager.Instance?.TocarClique();

        if (TransicaoDeCena.Instance != null)
            TransicaoDeCena.Instance.CarregarCena("MenuPrincipal");
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuPrincipal");
    }

    void Reiniciar()
    {
        Time.timeScale = 1f;
        AudioManager.Instance?.TocarClique();

        string cenaAtual = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (TransicaoDeCena.Instance != null)
            TransicaoDeCena.Instance.CarregarCena(cenaAtual);
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(cenaAtual);
    }

    // ─────────────────────────────────────────
    // ON DESTROY — garante Time.timeScale = 1
    // ─────────────────────────────────────────
    void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
