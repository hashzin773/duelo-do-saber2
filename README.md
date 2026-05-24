# 🃏 Duelo do Saber

> Jogo educativo de cartas de perguntas e respostas desenvolvido em Unity 2D como projeto de reforço escolar para ONG.

---

## 📌 Sobre o Projeto

**Duelo do Saber** é um jogo no estilo duelo de cartas onde o jogador enfrenta um oponente controlado por IA respondendo perguntas de múltipla escolha sobre diversas matérias escolares. O projeto foi desenvolvido com foco em **engajamento educativo**, utilizando feedback visual e sonoro para reforçar o aprendizado.

| Campo | Informação |
|---|---|
| **Engine** | Unity 6 (6000.3.9f1) |
| **Linguagem** | C# |
| **Tipo** | Jogo 2D educativo |
| **Plataforma** | Windows |
| **Proposta** | Reforço escolar para ONG |

---

## 👥 Equipe

| Nome |
|---|
| Luiz Henrique |
| Cesar Augusto |
| Franklyn |
| Pedro Paulo |

---

## 🎮 Como Jogar

1. O **oponente** revela uma carta-pergunta
2. O **jogador** escolhe uma das 4 cartas de resposta
3. **Acerto** → oponente perde 1 vida
4. **Erro** → jogador perde 1 vida
5. O temporizador corre — responder rápido vale mais pontos
6. Quem zerar as vidas do adversário primeiro **vence**

### Cartas Especiais
| Carta | Efeito |
|---|---|
| Eliminar Alternativa | Remove uma alternativa errada |
| Pular Pergunta | Passa para próxima sem perder vida |
| Dano Duplo | Oponente perde 2 vidas ao acertar |

### Matérias
Matemática · Português · História · Geografia · Ciências

---

## 🏗️ Estrutura do Projeto

```
Assets/
├── Audio/          → Trilhas sonoras e efeitos sonoros
├── Images/         → Sprites das cartas e background
├── Prefabs/        → Prefab da carta de resposta
├── Scenes/         → Cenas do jogo
│   ├── MenuPrincipal.unity
│   ├── Batalha.unity
│   └── VitoriaDerrota.unity
└── Scripts/        → Todos os scripts C#
    ├── GameManager.cs
    ├── QuestionManager.cs
    ├── CardController.cs
    ├── UIManager.cs
    ├── AIOpponent.cs
    ├── AudioManager.cs
    ├── MenuController.cs
    ├── TransicaoDeCena.cs
    └── VitoriaDerrota.cs
```

---

## 📁 Scripts — Descrição

| Script | Responsabilidade |
|---|---|
| `GameManager.cs` | Controla vidas, turnos, pontuação, temporizador e dificuldade |
| `QuestionManager.cs` | Banco de 91 perguntas, sorteio e verificação de respostas |
| `CardController.cs` | Comportamento de cada carta de resposta (clique, feedback visual) |
| `UIManager.cs` | Atualização de toda a interface: HUD, cartas, temporizador, pontuação |
| `AIOpponent.cs` | Lógica e reações do oponente controlado por IA |
| `AudioManager.cs` | Trilhas sonoras e efeitos sonoros centralizados |
| `MenuController.cs` | Menu principal, pausa via ESC e navegação entre cenas |
| `TransicaoDeCena.cs` | Efeito de fade preto nas transições entre cenas |
| `VitoriaDerrota.cs` | Tela de resultado com pontuação final e opções de recomeço |

---

## ⚙️ Padrões de Arquitetura

- **Singleton com DontDestroyOnLoad** em todos os managers
- **Eventos C# (delegate + event)** para comunicação desacoplada entre scripts
- **Coroutines** para temporizador, delays dramáticos e efeito de fade
- **GridLayoutGroup / HorizontalLayoutGroup** para organização automática das cartas

---

## 🎵 Áudio

| Arquivo | Uso |
|---|---|
| `trilhaBatalha.wav` | Trilha de fundo durante a batalha |
| `trilhaMenu.wav` | Trilha de fundo no menu principal |
| `acerto.wav` | Som ao acertar uma resposta |
| `erro.wav` | Som ao errar uma resposta |
| `vitoria.wav` | Som de vitória ao fim da partida |
| `derrota.wav` | Som de derrota ao fim da partida |
| `clique.mp3` | Som de clique nos botões |

---

## 🚀 Como Abrir o Projeto

1. Instale o **Unity 6** (versão 6000.3.9f1 ou superior)
2. Clone ou baixe este repositório
3. Abra o Unity Hub → **Add project from disk**
4. Selecione a pasta raiz do projeto
5. Aguarde a importação dos assets
6. Abra a cena `Assets/Scenes/MenuPrincipal.unity`
7. Clique em **Play** ▶

> ⚠️ Certifique-se de que o pacote **TextMeshPro** está instalado (Window → Package Manager).

---

## 📊 Status do Desenvolvimento

| Etapa | Descrição | Status |
|---|---|---|
| 1 | Configuração do projeto Unity | ✅ Concluído |
| 2 | Scripts da lógica do jogo | ✅ Concluído |
| 3 | Montagem da cena Batalha | ✅ Concluído |
| 4 | Prefab da carta de resposta | ✅ Concluído |
| 5 | Conexão do UIManager no Inspector | ✅ Concluído |
| 6 | Arte e visual (sprites) | ⚠️ Pendente |
| 7 | Áudio (AudioManager) | ✅ Concluído |
| 8 | Menu Principal e Pausa | ✅ Concluído |
| 9 | Transições entre cenas | ✅ Concluído |
| 10 | Tela de Vitória/Derrota | ✅ Concluído |
| 11 | Testes e ajustes finais | 🔄 Em andamento |

---

## 📄 Licença

Projeto desenvolvido para fins educacionais. Todos os assets de áudio utilizados são de licença Creative Commons 0 (CC0) obtidos em freesound.org, mixkit.co e pixabay.com.
