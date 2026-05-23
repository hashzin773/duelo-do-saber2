using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// GameManager — Gerencia o fluxo principal do Duelo do Saber.
/// REFATORADO: temporizador de resposta, pontuação com bônus, 3 níveis de dificuldade,
/// integração com AudioManager e TransicaoDeCena.
/// </summary>
public class GameManager : MonoBehaviour
{
    // ─────────────────────────────────────────
    // SINGLETON
    // ─────────────────────────────────────────
    public static GameManager Instance { get; private set; }

    // ─────────────────────────────────────────
    // ENUMS
    // ─────────────────────────────────────────
    public enum Turno { Oponente, Jogador }

    public enum NivelDificuldade { Facil = 1, Medio = 2, Dificil = 3 }

    // ─────────────────────────────────────────
    // CONFIGURAÇÕES (Inspector)
    // ─────────────────────────────────────────
    [Header("Configurações de Vida")]
    public int vidasIniciais = 5;

    [Header("Configurações de Nível")]
    public int nivelAtual    = 1;
    public int totalDeNiveis = 10;

    [Header("Dificuldade")]
    public NivelDificuldade dificuldade = NivelDificuldade.Facil;

    [Header("Temporizador (segundos por dificuldade)")]
    public float tempoFacil   = 30f;
    public float tempoMedio   = 20f;
    public float tempoDificil = 10f;

    // ─────────────────────────────────────────
    // ESTADO DO JOGO
    // ─────────────────────────────────────────
    public int   vidasJogador   { get; private set; }
    public int   vidasOponente  { get; private set; }
    public bool  IsJogoAtivo    { get; private set; }
    public int   pontuacao      { get; private set; }
    public float tempoRestante  { get; private set; }
    public Turno turnoAtual     { get; private set; }

    private float tempoMaximo;
    private Coroutine coroutineTemporizador;

    // ─────────────────────────────────────────
    // EVENTOS
    // ─────────────────────────────────────────
    public delegate void OnVidaAlterada(int vidasJogador, int vidasOponente);
    public static event OnVidaAlterada EventoVidaAlterada;

    public delegate void OnTurnoMudou(Turno turno);
    public static event OnTurnoMudou EventoTurnoMudou;

    public delegate void OnFimDeJogo(bool jogadorVenceu);
    public static event OnFimDeJogo EventoFimDeJogo;

    public delegate void OnPontuacaoAlterada(int novaPontuacao);
    public static event OnPontuacaoAlterada EventoPontuacaoAlterada;

    /// <summary>Dispara a cada frame durante o turno do jogador. Valor: 0f–1f (progresso do timer).</summary>
    public delegate void OnTemporizador(float progresso);
    public static event OnTemporizador EventoTemporizador;

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
        }
    }

    // ─────────────────────────────────────────
    // START
    // ─────────────────────────────────────────
    void Start()
{
    // Reinicia a partida sempre que a cena Batalha carregar
    if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Batalha")
        IniciarPartida();
}

void OnEnable()
{
    UnityEngine.SceneManagement.SceneManager.sceneLoaded += AoCenarCarregada;
}

void OnDisable()
{
    UnityEngine.SceneManagement.SceneManager.sceneLoaded -= AoCenarCarregada;
}

void AoCenarCarregada(UnityEngine.SceneManagement.Scene cena, UnityEngine.SceneManagement.LoadSceneMode modo)
{
    if (cena.name == "Batalha")
        IniciarPartida();
}
    // ─────────────────────────────────────────
    // INICIAR PARTIDA
    // ─────────────────────────────────────────
    public void IniciarPartida()
    {
        vidasJogador  = vidasIniciais;
        vidasOponente = vidasIniciais;
        IsJogoAtivo   = true;
        pontuacao     = 0;
        turnoAtual    = Turno.Oponente;

        // Define tempo máximo conforme dificuldade
        tempoMaximo = dificuldade switch
        {
            NivelDificuldade.Medio   => tempoMedio,
            NivelDificuldade.Dificil => tempoDificil,
            _                        => tempoFacil
        };

        Debug.Log($"[GameManager] Partida iniciada! Nível {nivelAtual} | Dificuldade: {dificuldade}");

        EventoVidaAlterada?.Invoke(vidasJogador, vidasOponente);
        EventoPontuacaoAlterada?.Invoke(pontuacao);

        Invoke(nameof(TurnoOponenteRevelaPergunta), 0.5f);
    }

    // ─────────────────────────────────────────
    // PROCESSAR RESPOSTA
    // ─────────────────────────────────────────
    public void ProcessarResposta(bool respostaCorreta)
    {
        if (!IsJogoAtivo) return;

        PararTemporizador();

        if (respostaCorreta)
        {
            // Pontuação = 100 + (tempoRestante * 10 * nivelDificuldade)
            int bonus = Mathf.RoundToInt(100 + (tempoRestante * 10f * (int)dificuldade));
            pontuacao += bonus;
            Debug.Log($"[GameManager] CORRETO! +{bonus} pontos. Total: {pontuacao}");

            vidasOponente--;
            AudioManager.Instance?.TocarAcerto();
        }
        else
        {
            Debug.Log("[GameManager] ERRADO! Jogador perde 1 vida.");
            vidasJogador--;
            AudioManager.Instance?.TocarErro();
        }

        EventoVidaAlterada?.Invoke(vidasJogador, vidasOponente);
        EventoPontuacaoAlterada?.Invoke(pontuacao);

        if (VerificarFimDeJogo()) return;

        ProximoTurno();
    }

    // ─────────────────────────────────────────
    // CARTAS ESPECIAIS
    // ─────────────────────────────────────────
    public void UsarDanoDuplo()
    {
        if (!IsJogoAtivo) return;
        vidasOponente = Mathf.Max(0, vidasOponente - 2);
        Debug.Log("[GameManager] Carta especial: DANO DUPLO!");
        AudioManager.Instance?.TocarEspecial();
        EventoVidaAlterada?.Invoke(vidasJogador, vidasOponente);
        VerificarFimDeJogo();
    }

    public void UsarPularPergunta()
    {
        if (!IsJogoAtivo) return;
        Debug.Log("[GameManager] Carta especial: PULAR PERGUNTA!");
        AudioManager.Instance?.TocarEspecial();
        PararTemporizador();
        ProximoTurno();
    }

    // ─────────────────────────────────────────
    // TEMPORIZADOR
    // ─────────────────────────────────────────
    void IniciarTemporizador()
    {
        PararTemporizador();
        tempoRestante = tempoMaximo;
        coroutineTemporizador = StartCoroutine(CoroutineTemporizador());
    }

    void PararTemporizador()
    {
        if (coroutineTemporizador != null)
        {
            StopCoroutine(coroutineTemporizador);
            coroutineTemporizador = null;
        }
    }

    IEnumerator CoroutineTemporizador()
    {
        while (tempoRestante > 0f)
        {
            tempoRestante -= Time.deltaTime;
            tempoRestante  = Mathf.Max(0f, tempoRestante);

            // Dispara progresso (1 = cheio, 0 = zerado)
            EventoTemporizador?.Invoke(tempoRestante / tempoMaximo);

            yield return null;
        }

        // Tempo esgotado — trata como resposta errada
        Debug.Log("[GameManager] Tempo esgotado!");
        ProcessarResposta(false);
    }

    // ─────────────────────────────────────────
    // FLUXO DE TURNOS
    // ─────────────────────────────────────────
    void ProximoTurno()
    {
        turnoAtual = Turno.Oponente;
        Invoke(nameof(TurnoOponenteRevelaPergunta), 1.5f);
    }

    void TurnoOponenteRevelaPergunta()
    {
        if (!IsJogoAtivo) return;

        Debug.Log("[GameManager] Oponente revelou a carta-pergunta!");
        EventoTurnoMudou?.Invoke(Turno.Oponente);
        AudioManager.Instance?.TocarRevelar();

        QuestionManager.Instance?.SortearPergunta();

        Invoke(nameof(LiberarRespostasDoJogador), 1f);
    }

    void LiberarRespostasDoJogador()
    {
        if (!IsJogoAtivo) return;

        turnoAtual = Turno.Jogador;
        Debug.Log("[GameManager] Vez do jogador!");
        EventoTurnoMudou?.Invoke(Turno.Jogador);

        IniciarTemporizador();
    }

    // ─────────────────────────────────────────
    // FIM DE JOGO
    // ─────────────────────────────────────────
    bool VerificarFimDeJogo()
    {
        if (vidasJogador <= 0)  { EncerrarJogo(false); return true; }
        if (vidasOponente <= 0) { EncerrarJogo(true);  return true; }
        return false;
    }

    void EncerrarJogo(bool jogadorVenceu)
    {
        IsJogoAtivo = false;
        PararTemporizador();

        Debug.Log($"[GameManager] Fim de jogo! {(jogadorVenceu ? "VITÓRIA" : "DERROTA")}");

        if (jogadorVenceu && nivelAtual < totalDeNiveis)
            nivelAtual++;

        // Salva resultado para a tela VitoriaDerrota
        PlayerPrefs.SetInt("Venceu", jogadorVenceu ? 1 : 0);
        PlayerPrefs.SetInt("Pontuacao", pontuacao);
        PlayerPrefs.Save();

        EventoFimDeJogo?.Invoke(jogadorVenceu);

        StartCoroutine(IrParaTelaDeResultado());
    }

    IEnumerator IrParaTelaDeResultado()
    {
        yield return new WaitForSeconds(2f);

        // Usa TransicaoDeCena se disponível, senão carrega direto
        if (TransicaoDeCena.Instance != null)
            TransicaoDeCena.Instance.CarregarCena("VitoriaDerrota");
        else
            SceneManager.LoadScene("VitoriaDerrota");
    }

    // ─────────────────────────────────────────
    // UTILIDADE
    // ─────────────────────────────────────────
    public void ReiniciarPartida()
    {
        StopAllCoroutines();
        CancelInvoke();
        IniciarPartida();
    }

    public void DefinirDificuldade(NivelDificuldade novaDificuldade)
    {
        dificuldade = novaDificuldade;
        tempoMaximo = dificuldade switch
        {
            NivelDificuldade.Medio   => tempoMedio,
            NivelDificuldade.Dificil => tempoDificil,
            _                        => tempoFacil
        };
        Debug.Log($"[GameManager] Dificuldade definida: {dificuldade} ({tempoMaximo}s)");
    }
}
