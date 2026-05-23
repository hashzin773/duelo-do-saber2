using UnityEngine;

/// <summary>
/// AudioManager — Centraliza toda a sonorização do Duelo do Saber.
/// Dois AudioSources: trilha em loop + efeitos pontuais (PlayOneShot).
///
/// Como usar no Unity:
/// 1. Crie um GameObject vazio chamado "Audio Manager".
/// 2. Anexe este script a ele.
/// 3. Conecte os AudioClips no Inspector (pasta Assets/Audio/).
/// 4. Os dois AudioSources são criados automaticamente via AddComponent.
/// </summary>
public class AudioManager : MonoBehaviour
{
    // ─────────────────────────────────────────
    // SINGLETON
    // ─────────────────────────────────────────
    public static AudioManager Instance { get; private set; }

    // ─────────────────────────────────────────
    // CLIPS DE ÁUDIO (conecte no Inspector)
    // ─────────────────────────────────────────
    [Header("Trilhas Sonoras (loop)")]
    public AudioClip trilhaBatalha;     // Assets/Audio/trilhaBatalha.mp3
    public AudioClip trilhaMenu;        // Assets/Audio/trilhaMenu.mp3

    [Header("Efeitos Sonoros")]
    public AudioClip somAcerto;         // Assets/Audio/acerto.wav
    public AudioClip somErro;           // Assets/Audio/erro.wav
    public AudioClip somVitoria;        // Assets/Audio/vitoria.wav
    public AudioClip somDerrota;        // Assets/Audio/derrota.wav
    public AudioClip somClique;         // Assets/Audio/clique.wav
    public AudioClip somRevelar;        // Assets/Audio/revelar.wav
    public AudioClip somEspecial;       // Assets/Audio/especial.wav

    [Header("Volumes Iniciais (0 a 1)")]
    [Range(0f, 1f)] public float volumeTrilha   = 0.5f;
    [Range(0f, 1f)] public float volumeEfeitos  = 1.0f;

    // ─────────────────────────────────────────
    // AUDIO SOURCES (criados automaticamente)
    // ─────────────────────────────────────────
    private AudioSource fonteTrilha;
    private AudioSource fonteEfeitos;

    // ─────────────────────────────────────────
    // AWAKE
    // ─────────────────────────────────────────
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CriarAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void CriarAudioSources()
    {
        // Trilha — loop contínuo
        fonteTrilha          = gameObject.AddComponent<AudioSource>();
        fonteTrilha.loop     = true;
        fonteTrilha.volume   = volumeTrilha;
        fonteTrilha.playOnAwake = false;

        // Efeitos — disparos pontuais
        fonteEfeitos         = gameObject.AddComponent<AudioSource>();
        fonteEfeitos.loop    = false;
        fonteEfeitos.volume  = volumeEfeitos;
        fonteEfeitos.playOnAwake = false;
    }

    // ─────────────────────────────────────────
    // ON ENABLE / DISABLE — ouve eventos do GameManager
    // ─────────────────────────────────────────
    void OnEnable()
    {
        GameManager.EventoFimDeJogo   += AoFimDeJogo;
        GameManager.EventoTurnoMudou  += AoMudarTurno;
    }

    void OnDisable()
    {
        GameManager.EventoFimDeJogo   -= AoFimDeJogo;
        GameManager.EventoTurnoMudou  -= AoMudarTurno;
    }

    // ─────────────────────────────────────────
    // TRILHA SONORA
    // ─────────────────────────────────────────
    public void TocarTrilha(AudioClip clip)
    {
        if (clip == null || fonteTrilha == null) return;
        if (fonteTrilha.clip == clip && fonteTrilha.isPlaying) return; // Evita reiniciar mesma trilha

        fonteTrilha.clip = clip;
        fonteTrilha.Play();
    }

    public void PararTrilha()
    {
        fonteTrilha?.Stop();
    }

    public void TocarTrilhaBatalha() => TocarTrilha(trilhaBatalha);
    public void TocarTrilhaMenu()    => TocarTrilha(trilhaMenu);

    // ─────────────────────────────────────────
    // EFEITOS SONOROS
    // ─────────────────────────────────────────
    void TocarEfeito(AudioClip clip)
    {
        if (clip == null || fonteEfeitos == null) return;
        fonteEfeitos.PlayOneShot(clip, volumeEfeitos);
    }

    public void TocarAcerto()   => TocarEfeito(somAcerto);
    public void TocarErro()     => TocarEfeito(somErro);
    public void TocarVitoria()  => TocarEfeito(somVitoria);
    public void TocarDerrota()  => TocarEfeito(somDerrota);
    public void TocarClique()   => TocarEfeito(somClique);
    public void TocarRevelar()  => TocarEfeito(somRevelar);
    public void TocarEspecial() => TocarEfeito(somEspecial);

    // ─────────────────────────────────────────
    // CONTROLE DE VOLUME
    // ─────────────────────────────────────────
    public void DefinirVolumeTrilha(float valor)
    {
        volumeTrilha = Mathf.Clamp01(valor);
        if (fonteTrilha != null) fonteTrilha.volume = volumeTrilha;
    }

    public void DefinirVolumeEfeitos(float valor)
    {
        volumeEfeitos = Mathf.Clamp01(valor);
        if (fonteEfeitos != null) fonteEfeitos.volume = volumeEfeitos;
    }

    // ─────────────────────────────────────────
    // REAÇÕES AUTOMÁTICAS A EVENTOS
    // ─────────────────────────────────────────
    void AoFimDeJogo(bool jogadorVenceu)
    {
        PararTrilha();
        if (jogadorVenceu) TocarVitoria();
        else               TocarDerrota();
    }

    void AoMudarTurno(GameManager.Turno turno)
    {
        // Toca clique quando muda para o turno do jogador
        if (turno == GameManager.Turno.Jogador)
            TocarClique();
    }
}
