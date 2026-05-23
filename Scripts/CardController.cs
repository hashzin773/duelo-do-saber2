using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// CardController — Controla o comportamento de cada carta na tela.
/// Cada carta de resposta terá este script anexado.
///
/// Como usar no Unity:
/// 1. Crie um Prefab de carta (ex: um Panel com Button e TextMeshPro).
/// 2. Anexe este script ao Prefab.
/// 3. Conecte os campos "textoCarta" e "botaoCarta" no Inspector.
/// 4. O UIManager vai instanciar os prefabs e chamar Configurar() em cada um.
/// </summary>
public class CardController : MonoBehaviour
{
    // ─────────────────────────────────────────
    // REFERÊNCIAS (conecte no Inspector do Unity)
    // ─────────────────────────────────────────
    [Header("Componentes da Carta")]
    public TextMeshProUGUI textoCarta;   // Texto que aparece na carta
    public Button botaoCarta;            // Botão clicável da carta
    public Image imagemCarta;            // Imagem/fundo da carta

    [Header("Cores de Feedback")]
    public Color corNormal = Color.white;
    public Color corCorreta = new Color(0.2f, 0.8f, 0.2f);   // Verde
    public Color corErrada  = new Color(0.8f, 0.2f, 0.2f);   // Vermelho
    public Color corDesabilitada = new Color(0.5f, 0.5f, 0.5f); // Cinza

    // ─────────────────────────────────────────
    // DADOS INTERNOS DA CARTA
    // ─────────────────────────────────────────
    private int indiceResposta;          // Qual alternativa esta carta representa (0, 1, 2 ou 3)
    private bool foiEliminada = false;   // Carta especial "eliminar alternativa"
    private bool jogoEstaAtivo = true;

    // ─────────────────────────────────────────
    // AWAKE / START
    // ─────────────────────────────────────────
    void Awake()
    {
        // Garante que o botão chama o método correto ao ser clicado
        if (botaoCarta != null)
            botaoCarta.onClick.AddListener(AoClicar);
    }

    void OnEnable()
    {
        // Ouve o evento de mudança de turno para habilitar/desabilitar cartas
        GameManager.EventoTurnoMudou += AoMudarTurno;
        GameManager.EventoFimDeJogo  += AoFimDeJogo;
    }

    void OnDisable()
    {
        GameManager.EventoTurnoMudou -= AoMudarTurno;
        GameManager.EventoFimDeJogo  -= AoFimDeJogo;
    }

    // ─────────────────────────────────────────
    // CONFIGURAR CARTA
    // Chamado pelo UIManager ao montar as cartas na tela
    // ─────────────────────────────────────────
    public void Configurar(string textoResposta, int indice)
    {
        indiceResposta = indice;
        foiEliminada = false;
        jogoEstaAtivo = true;

        if (textoCarta != null)
            textoCarta.text = textoResposta;

        if (imagemCarta != null)
            imagemCarta.color = corNormal;

        // Carta começa desabilitada — só habilita quando for turno do jogador
        DefinirInteracao(false);
    }

    // ─────────────────────────────────────────
    // AO CLICAR NA CARTA
    // ─────────────────────────────────────────
    void AoClicar()
    {
        if (!jogoEstaAtivo || foiEliminada) return;
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.turnoAtual != GameManager.Turno.Jogador) return;

        // Desabilita todas as cartas imediatamente (evita duplo clique)
        DefinirInteracao(false);

        // Verifica se a resposta está correta
        bool correta = QuestionManager.Instance.VerificarResposta(indiceResposta);

        // Mostra feedback visual
        MostrarFeedback(correta);

        // Informa o GameManager
        GameManager.Instance.ProcessarResposta(correta);
    }

    // ─────────────────────────────────────────
    // FEEDBACK VISUAL
    // ─────────────────────────────────────────
    void MostrarFeedback(bool correta)
    {
        if (imagemCarta == null) return;
        imagemCarta.color = correta ? corCorreta : corErrada;

        // Volta à cor normal após 1.2 segundos
        Invoke(nameof(ResetarCor), 1.2f);
    }

    void ResetarCor()
    {
        if (imagemCarta != null && !foiEliminada)
            imagemCarta.color = corNormal;
    }

    // ─────────────────────────────────────────
    // CARTA ESPECIAL: ELIMINAR ALTERNATIVA
    // Chamado pelo UIManager quando o jogador usa a carta especial
    // ─────────────────────────────────────────
    public void Eliminar()
    {
        foiEliminada = true;
        DefinirInteracao(false);

        if (imagemCarta != null)
            imagemCarta.color = corDesabilitada;

        if (textoCarta != null)
            textoCarta.text = "✗"; // Indica visualmente que foi eliminada
    }

    // ─────────────────────────────────────────
    // CONTROLE DE TURNO
    // ─────────────────────────────────────────
    void AoMudarTurno(GameManager.Turno turno)
    {
        // Habilita a carta apenas quando for turno do jogador e ela não foi eliminada
        bool jogadorPodeJogar = turno == GameManager.Turno.Jogador && !foiEliminada;
        DefinirInteracao(jogadorPodeJogar);
    }

    void AoFimDeJogo(bool jogadorVenceu)
    {
        jogoEstaAtivo = false;
        DefinirInteracao(false);
    }

    void DefinirInteracao(bool ativa)
    {
        if (botaoCarta != null)
            botaoCarta.interactable = ativa;
    }
}
