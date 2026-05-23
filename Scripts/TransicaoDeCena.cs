using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// TransicaoDeCena — Efeito de fade (preto) entre cenas.
///
/// Como usar no Unity:
/// 1. Crie um GameObject vazio chamado "Transicao De Cena".
/// 2. Anexe este script.
/// 3. Na Hierarchy, crie: Canvas (Screen Space Overlay) → Panel (preto, cobrindo tela inteira).
///    - Defina a cor do Panel como preto (R:0, G:0, B:0, A:255).
///    - Conecte esse Panel no campo "painelFade" no Inspector.
/// 4. Chame TransicaoDeCena.Instance.CarregarCena("NomeDaCena") de qualquer script.
/// </summary>
public class TransicaoDeCena : MonoBehaviour
{
    // ─────────────────────────────────────────
    // SINGLETON
    // ─────────────────────────────────────────
    public static TransicaoDeCena Instance { get; private set; }

    // ─────────────────────────────────────────
    // CONFIGURAÇÕES (Inspector)
    // ─────────────────────────────────────────
    [Header("Painel de Fade (Panel preto cobrindo a tela)")]
    public Image painelFade;

    [Header("Duração do fade em segundos")]
    public float duracaoFade = 0.5f;

    // ─────────────────────────────────────────
    // AWAKE
    // ─────────────────────────────────────────
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // ─────────────────────────────────────────
    // START — faz o fade de entrada (preto → transparente)
    // ─────────────────────────────────────────
    void Start()
    {
        if (painelFade != null)
            StartCoroutine(FadeEntrada());
    }

    // ─────────────────────────────────────────
    // MÉTODO PÚBLICO
    // ─────────────────────────────────────────
    /// <summary>Carrega a cena com efeito de fade. Chame de qualquer script.</summary>
    public void CarregarCena(string nomeCena)
    {
        StartCoroutine(FadeSaida(nomeCena));
    }

    // ─────────────────────────────────────────
    // COROUTINES DE FADE
    // ─────────────────────────────────────────

    /// <summary>Fade de entrada: tela preta → transparente.</summary>
    IEnumerator FadeEntrada()
    {
        DefinirAlpha(1f); // Começa totalmente preto
        float tempo = 0f;

        while (tempo < duracaoFade)
        {
            tempo += Time.deltaTime;
            DefinirAlpha(1f - (tempo / duracaoFade));
            yield return null;
        }

        DefinirAlpha(0f);
        if (painelFade != null)
            painelFade.gameObject.SetActive(false); // Desativa para não bloquear cliques
    }

    /// <summary>Fade de saída: transparente → preto → carrega cena.</summary>
    IEnumerator FadeSaida(string nomeCena)
    {
        if (painelFade != null)
            painelFade.gameObject.SetActive(true);

        DefinirAlpha(0f);
        float tempo = 0f;

        while (tempo < duracaoFade)
        {
            tempo += Time.deltaTime;
            DefinirAlpha(tempo / duracaoFade);
            yield return null;
        }

        DefinirAlpha(1f);

        // Carrega a nova cena
        yield return SceneManager.LoadSceneAsync(nomeCena);

        // Fade de entrada na nova cena
        StartCoroutine(FadeEntrada());
    }

    // ─────────────────────────────────────────
    // UTILITÁRIO
    // ─────────────────────────────────────────
    void DefinirAlpha(float alpha)
    {
        if (painelFade == null) return;
        Color cor = painelFade.color;
        cor.a          = Mathf.Clamp01(alpha);
        painelFade.color = cor;
    }
}
