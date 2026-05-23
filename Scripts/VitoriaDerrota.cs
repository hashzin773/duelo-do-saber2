using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// VitoriaDerrota — Controla a tela de resultado final.
///
/// Como usar no Unity:
/// 1. Crie um GameObject vazio chamado "Vitoria Derrota Manager"
/// 2. Anexe este script
/// 3. Conecte os campos no Inspector
/// </summary>
public class VitoriaDerrota : MonoBehaviour
{
    [Header("Textos")]
    public TextMeshProUGUI textoResultado;
    public TextMeshProUGUI textoPontuacao;

    [Header("Botões")]
    public Button botaoRecomecar;
    public Button botaoMenuPrincipal;

    [Header("Mensagens")]
    public string mensagemVitoria = "VOCÊ VENCEU! 🏆";
    public string mensagemDerrota = "VOCÊ PERDEU! 💀";

    void Start()
    {
        // Conecta os botões
        botaoRecomecar?.onClick.AddListener(Recomecar);
        botaoMenuPrincipal?.onClick.AddListener(IrParaMenu);

        // Exibe resultado e pontuação
        MostrarResultado();

        // Toca som de vitória ou derrota
        if (GameManager.Instance != null)
        {
            // O AudioManager já tocou o som via evento, mas tocamos a trilha do menu
            AudioManager.Instance?.TocarTrilhaMenu();
        }
    }

    void MostrarResultado()
    {
        // Verifica se o jogador venceu pela pontuação ou pelo GameManager
        bool venceu = PlayerPrefs.GetInt("Venceu", 0) == 1;
        int pontos  = PlayerPrefs.GetInt("Pontuacao", 0);

        if (textoResultado != null)
            textoResultado.text = venceu ? mensagemVitoria : mensagemDerrota;

        if (textoPontuacao != null)
            textoPontuacao.text = $"Pontuação: {pontos}";
    }

    void Recomecar()
    {
        AudioManager.Instance?.TocarClique();

        if (TransicaoDeCena.Instance != null)
            TransicaoDeCena.Instance.CarregarCena("Batalha");
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene("Batalha");
    }

    void IrParaMenu()
    {
        AudioManager.Instance?.TocarClique();

        if (TransicaoDeCena.Instance != null)
            TransicaoDeCena.Instance.CarregarCena("MenuPrincipal");
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuPrincipal");
    }
}
