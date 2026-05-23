using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// UIManager — Gerencia toda a interface visual do Duelo do Saber.
/// REFATORADO: suporte a temporizador visual, pontuação e novos eventos do GameManager.
/// </summary>
public class UIManager : MonoBehaviour
{
    // ─────────────────────────────────────────
    // SINGLETON
    // ─────────────────────────────────────────
    public static UIManager Instance { get; private set; }

    // ─────────────────────────────────────────
    // VIDAS
    // ─────────────────────────────────────────
    [Header("Vidas — Textos (opcional)")]
    public TextMeshProUGUI textoVidasJogador;
    public TextMeshProUGUI textoVidasOponente;

    [Header("Vidas — Barras HP (opcional)")]
    public Slider barraHPJogador;
    public Slider barraHPOponente;

    [Header("Vidas — Ícones de coração (opcional)")]
    public Image[] iconesVidaJogador;
    public Image[] iconesVidaOponente;

    // ─────────────────────────────────────────
    // PERGUNTA
    // ─────────────────────────────────────────
    [Header("Pergunta *")]
    public TextMeshProUGUI textoPergunta;
    public GameObject painelPergunta;

    // ─────────────────────────────────────────
    // MATÉRIA
    // ─────────────────────────────────────────
    [Header("Matéria da Pergunta (opcional)")]
    [Tooltip("Texto que mostra ex: 'Matemática' ou 'História'")]
    public TextMeshProUGUI textoMateria;

    // ─────────────────────────────────────────
    // CARTAS
    // ─────────────────────────────────────────
    [Header("Cartas de Resposta *")]
    public GameObject prefabCarta;
    public Transform containerCartas;

    // ─────────────────────────────────────────
    // CARTAS ESPECIAIS
    // ─────────────────────────────────────────
    [Header("Cartas Especiais (opcional)")]
    public Button botaoEliminarAlternativa;
    public Button botaoPularPergunta;
    public Button botaoDanoDuplo;

    // ─────────────────────────────────────────
    // PONTUAÇÃO
    // ─────────────────────────────────────────
    [Header("Pontuação (opcional)")]
    public TextMeshProUGUI textoPontuacao;

    // ─────────────────────────────────────────
    // TEMPORIZADOR
    // ─────────────────────────────────────────
    [Header("Temporizador (opcional)")]
    [Tooltip("Slider que representa o tempo restante visualmente")]
    public Slider barraTemporizador;
    [Tooltip("Texto que exibe os segundos restantes")]
    public TextMeshProUGUI textoTemporizador;
    [Tooltip("Cor normal da barra de tempo")]
    public Color corTempoNormal  = Color.green;
    [Tooltip("Cor quando o tempo está acabando (< 30%)")]
    public Color corTempoPerigo  = Color.red;

    private Image fillTemporizador; // Referência ao fill da barra para mudar cor

    // ─────────────────────────────────────────
    // STATUS / NÍVEL / RESULTADO
    // ─────────────────────────────────────────
    [Header("Status de Turno (opcional)")]
    public TextMeshProUGUI textoStatusTurno;

    [Header("Nível (opcional)")]
    public TextMeshProUGUI textoNivel;

    [Header("Resultado Rápido (opcional)")]
    public TextMeshProUGUI textoResultadoRapido;

    // ─────────────────────────────────────────
    // INTERNO
    // ─────────────────────────────────────────
    private List<CardController> cartasAtivas = new List<CardController>();

    // ─────────────────────────────────────────
    // AWAKE
    // ─────────────────────────────────────────
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // ─────────────────────────────────────────
    // ON ENABLE / DISABLE — inscrição nos eventos
    // ─────────────────────────────────────────
    void OnEnable()
    {
        GameManager.EventoVidaAlterada      += AtualizarVidas;
        GameManager.EventoTurnoMudou        += AoMudarTurno;
        GameManager.EventoFimDeJogo         += MostrarResultado;
        GameManager.EventoPontuacaoAlterada += AtualizarPontuacao;
        GameManager.EventoTemporizador      += AtualizarTemporizador;
        QuestionManager.EventoNovaPergunta  += ExibirPergunta;
    }

    void OnDisable()
    {
        GameManager.EventoVidaAlterada      -= AtualizarVidas;
        GameManager.EventoTurnoMudou        -= AoMudarTurno;
        GameManager.EventoFimDeJogo         -= MostrarResultado;
        GameManager.EventoPontuacaoAlterada -= AtualizarPontuacao;
        GameManager.EventoTemporizador      -= AtualizarTemporizador;
        QuestionManager.EventoNovaPergunta  -= ExibirPergunta;
    }

    // ─────────────────────────────────────────
    // START
    // ─────────────────────────────────────────
    void Start()
    {
        if (textoResultadoRapido != null)
            textoResultadoRapido.gameObject.SetActive(false);

        ConfigurarBarrasHP();
        ConfigurarTemporizador();
        ConfigurarCartasEspeciais();
        AtualizarTextoNivel();

        if (textoPontuacao != null) textoPontuacao.text = "0";
    }

    // ─────────────────────────────────────────
    // CONFIGURAÇÃO INICIAL
    // ─────────────────────────────────────────
    void ConfigurarBarrasHP()
    {
        int vidas = GameManager.Instance != null ? GameManager.Instance.vidasIniciais : 5;

        if (barraHPJogador != null)
        {
            barraHPJogador.minValue = 0; barraHPJogador.maxValue = vidas;
            barraHPJogador.value = vidas; barraHPJogador.wholeNumbers = true;
        }
        if (barraHPOponente != null)
        {
            barraHPOponente.minValue = 0; barraHPOponente.maxValue = vidas;
            barraHPOponente.value = vidas; barraHPOponente.wholeNumbers = true;
        }
    }

    void ConfigurarTemporizador()
    {
        if (barraTemporizador != null)
        {
            barraTemporizador.minValue = 0f;
            barraTemporizador.maxValue = 1f;
            barraTemporizador.value    = 1f;

            // Pega o Fill Image para poder mudar a cor
            Transform fill = barraTemporizador.transform.Find("Fill Area/Fill");
            if (fill != null) fillTemporizador = fill.GetComponent<Image>();
            if (fillTemporizador != null) fillTemporizador.color = corTempoNormal;
        }
    }

    void ConfigurarCartasEspeciais()
    {
        botaoEliminarAlternativa?.onClick.AddListener(UsarEliminarAlternativa);
        botaoPularPergunta?.onClick.AddListener(UsarPularPergunta);
        botaoDanoDuplo?.onClick.AddListener(UsarDanoDuplo);
    }

    // ─────────────────────────────────────────
    // ATUALIZAR VIDAS
    // ─────────────────────────────────────────
    void AtualizarVidas(int vidasJogador, int vidasOponente)
    {
        if (textoVidasJogador  != null) textoVidasJogador.text  = $"❤ {vidasJogador}";
        if (textoVidasOponente != null) textoVidasOponente.text = $"❤ {vidasOponente}";
        if (barraHPJogador     != null) barraHPJogador.value    = vidasJogador;
        if (barraHPOponente    != null) barraHPOponente.value   = vidasOponente;

        AtualizarIconesCoracao(iconesVidaJogador,  vidasJogador);
        AtualizarIconesCoracao(iconesVidaOponente, vidasOponente);
    }

    void AtualizarIconesCoracao(Image[] icones, int vidasRestantes)
    {
        if (icones == null) return;
        for (int i = 0; i < icones.Length; i++)
            if (icones[i] != null)
                icones[i].color = i < vidasRestantes ? Color.red : Color.gray;
    }

    // ─────────────────────────────────────────
    // PONTUAÇÃO
    // ─────────────────────────────────────────
    void AtualizarPontuacao(int novaPontuacao)
    {
        if (textoPontuacao != null)
            textoPontuacao.text = novaPontuacao.ToString("N0"); // ex: "1.200"
    }

    // ─────────────────────────────────────────
    // TEMPORIZADOR
    // ─────────────────────────────────────────
    void AtualizarTemporizador(float progresso)
    {
        if (barraTemporizador != null)
            barraTemporizador.value = progresso;

        if (textoTemporizador != null && GameManager.Instance != null)
            textoTemporizador.text = Mathf.CeilToInt(GameManager.Instance.tempoRestante).ToString();

        // Muda cor da barra para vermelho quando restam menos de 30%
        if (fillTemporizador != null)
            fillTemporizador.color = progresso < 0.3f ? corTempoPerigo : corTempoNormal;
    }

    // ─────────────────────────────────────────
    // EXIBIR PERGUNTA E CARTAS
    // ─────────────────────────────────────────
    void ExibirPergunta(QuestionManager.Pergunta pergunta)
    {
        if (textoPergunta != null)    textoPergunta.text = pergunta.enunciado;
        if (painelPergunta != null)   painelPergunta.SetActive(true);
        if (textoMateria != null)     textoMateria.text  = pergunta.materia;

        LimparCartas();

        for (int i = 0; i < pergunta.alternativas.Length; i++)
        {
            if (prefabCarta == null || containerCartas == null) break;

            GameObject novaCarta = Instantiate(prefabCarta, containerCartas);
            CardController ctrl  = novaCarta.GetComponent<CardController>();

            if (ctrl != null)
            {
                ctrl.Configurar(pergunta.alternativas[i], i);
                cartasAtivas.Add(ctrl);
            }
        }

        // Reseta a barra de tempo visualmente
        if (barraTemporizador != null) barraTemporizador.value = 1f;
        if (fillTemporizador   != null) fillTemporizador.color = corTempoNormal;
    }

    void LimparCartas()
    {
        foreach (var carta in cartasAtivas)
            if (carta != null) Destroy(carta.gameObject);
        cartasAtivas.Clear();
    }

    // ─────────────────────────────────────────
    // CARTAS ESPECIAIS
    // ─────────────────────────────────────────
    void UsarEliminarAlternativa()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsJogoAtivo) return;
        AudioManager.Instance?.TocarEspecial();

        int indice = QuestionManager.Instance.EliminarAlternativaErrada();
        if (indice >= 0 && indice < cartasAtivas.Count)
            cartasAtivas[indice]?.Eliminar();

        botaoEliminarAlternativa?.gameObject.SetActive(false);
    }

    void UsarPularPergunta()
    {
        AudioManager.Instance?.TocarEspecial();
        GameManager.Instance?.UsarPularPergunta();
        botaoPularPergunta?.gameObject.SetActive(false);
    }

    void UsarDanoDuplo()
    {
        AudioManager.Instance?.TocarEspecial();
        GameManager.Instance?.UsarDanoDuplo();
        botaoDanoDuplo?.gameObject.SetActive(false);
    }

    // ─────────────────────────────────────────
    // STATUS DE TURNO
    // ─────────────────────────────────────────
    void AoMudarTurno(GameManager.Turno turno)
    {
        if (textoStatusTurno != null)
            textoStatusTurno.text = turno == GameManager.Turno.Jogador
                ? "Sua vez! Escolha uma carta."
                : "Oponente está jogando...";
    }

    // ─────────────────────────────────────────
    // RESULTADO
    // ─────────────────────────────────────────
    void MostrarResultado(bool jogadorVenceu)
    {
        if (textoResultadoRapido == null) return;
        textoResultadoRapido.text = jogadorVenceu ? "VOCÊ VENCEU! 🏆" : "VOCÊ PERDEU! 💀";
        textoResultadoRapido.gameObject.SetActive(true);
        StartCoroutine(EsconderResultado());
    }

    IEnumerator EsconderResultado()
    {
        yield return new WaitForSeconds(2f);
        if (textoResultadoRapido != null)
            textoResultadoRapido.gameObject.SetActive(false);
    }

    // ─────────────────────────────────────────
    // NÍVEL
    // ─────────────────────────────────────────
    void AtualizarTextoNivel()
    {
        if (textoNivel != null && GameManager.Instance != null)
            textoNivel.text = $"Nível {GameManager.Instance.nivelAtual}";
    }

    public void ExibirMensagemFim(string mensagem)
    {
        if (textoResultadoRapido == null) return;
        textoResultadoRapido.text = mensagem;
        textoResultadoRapido.gameObject.SetActive(true);
        StartCoroutine(EsconderResultado());
    }
}
