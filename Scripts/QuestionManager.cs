using UnityEngine;
using System.Collections.Generic;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance { get; private set; }

    [System.Serializable]
    public class Pergunta
    {
        public string enunciado;
        public string[] alternativas;
        public int indiceRespostaCorreta;
        public string materia;
        public int nivelMinimo;
    }

    private List<Pergunta> bancoDePerguntass = new List<Pergunta>();
    private List<Pergunta> perguntasUsadas   = new List<Pergunta>();
    public Pergunta perguntaAtual { get; private set; }

    public delegate void OnNovaPergunta(Pergunta pergunta);
    public static event OnNovaPergunta EventoNovaPergunta;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
        CarregarPerguntas();
    }

    void CarregarPerguntas()
    {
        // ─────────────────────────────────────────
        // MATEMÁTICA — Nível 1
        // ─────────────────────────────────────────
        AdicionarPergunta("Quanto é 8 x 7?",
            new string[] { "54", "56", "64", "48" }, 1, "Matematica", 1);
        AdicionarPergunta("Qual é o resultado de 45 ÷ 9?",
            new string[] { "4", "6", "5", "7" }, 2, "Matematica", 1);
        AdicionarPergunta("Quanto é 123 + 456?",
            new string[] { "579", "569", "589", "599" }, 0, "Matematica", 1);
        AdicionarPergunta("Qual é o dobro de 37?",
            new string[] { "64", "74", "84", "54" }, 1, "Matematica", 1);
        AdicionarPergunta("Quanto é 200 - 85?",
            new string[] { "105", "125", "115", "135" }, 2, "Matematica", 1);
        AdicionarPergunta("Qual é a metade de 96?",
            new string[] { "46", "52", "48", "44" }, 2, "Matematica", 1);
        AdicionarPergunta("Quanto é 9 x 9?",
            new string[] { "72", "81", "90", "63" }, 1, "Matematica", 1);
        AdicionarPergunta("Qual número é primo?",
            new string[] { "9", "15", "21", "13" }, 3, "Matematica", 1);

        // MATEMÁTICA — Nível 2
        AdicionarPergunta("Qual é a raiz quadrada de 81?",
            new string[] { "7", "8", "9", "10" }, 2, "Matematica", 2);
        AdicionarPergunta("Quanto é 15% de 200?",
            new string[] { "20", "25", "30", "35" }, 2, "Matematica", 2);
        AdicionarPergunta("Qual é o valor de 2³?",
            new string[] { "6", "8", "9", "4" }, 1, "Matematica", 2);
        AdicionarPergunta("Quanto é 3/4 de 120?",
            new string[] { "80", "90", "100", "60" }, 1, "Matematica", 2);
        AdicionarPergunta("Qual é o MMC de 4 e 6?",
            new string[] { "8", "10", "12", "24" }, 2, "Matematica", 2);
        AdicionarPergunta("Um triângulo tem ângulos de 60 e 80 graus. Qual é o terceiro?",
            new string[] { "30", "40", "50", "60" }, 1, "Matematica", 2);
        AdicionarPergunta("Quanto é 25% de 80?",
            new string[] { "15", "20", "25", "30" }, 1, "Matematica", 2);

        // MATEMÁTICA — Nível 3
        AdicionarPergunta("Qual é a formula da area do circulo?",
            new string[] { "2xPIxR", "PIxR²", "PIxD", "2xPIxR²" }, 1, "Matematica", 3);
        AdicionarPergunta("Se x + 5 = 12, quanto vale x?",
            new string[] { "5", "6", "7", "8" }, 2, "Matematica", 3);
        AdicionarPergunta("Qual é o resultado de raiz de 144?",
            new string[] { "11", "12", "13", "14" }, 1, "Matematica", 3);
        AdicionarPergunta("Quanto é 2/3 + 1/6?",
            new string[] { "3/9", "5/6", "1/2", "3/6" }, 1, "Matematica", 3);

        // ─────────────────────────────────────────
        // PORTUGUÊS — Nível 1
        // ─────────────────────────────────────────
        AdicionarPergunta("Qual é o plural de 'cidadão'?",
            new string[] { "cidadãos", "cidadões", "cidadãoes", "cidadons" }, 0, "Portugues", 1);
        AdicionarPergunta("O que é um substantivo?",
            new string[] { "Palavra que indica ação", "Palavra que nomeia seres", "Palavra que modifica o verbo", "Palavra que liga orações" }, 1, "Portugues", 1);
        AdicionarPergunta("Qual palavra está escrita corretamente?",
            new string[] { "excessão", "exceção", "exeção", "execção" }, 1, "Portugues", 1);
        AdicionarPergunta("O que é um adjetivo?",
            new string[] { "Nomeia um ser", "Indica uma ação", "Caracteriza o substantivo", "Liga duas orações" }, 2, "Portugues", 1);
        AdicionarPergunta("Qual é o antônimo de 'alegre'?",
            new string[] { "feliz", "animado", "triste", "contente" }, 2, "Portugues", 1);
        AdicionarPergunta("Quantas sílabas tem a palavra 'borboleta'?",
            new string[] { "2", "3", "4", "5" }, 2, "Portugues", 1);
        AdicionarPergunta("Qual é o sinônimo de 'belo'?",
            new string[] { "feio", "bonito", "grande", "rápido" }, 1, "Portugues", 1);
        AdicionarPergunta("Qual é o feminino de 'ator'?",
            new string[] { "atora", "atriz", "atrisa", "atresa" }, 1, "Portugues", 1);

        // PORTUGUÊS — Nível 2
        AdicionarPergunta("O que é um verbo?",
            new string[] { "Nome de um lugar", "Palavra que indica ação ou estado", "Palavra que qualifica", "Palavra que quantifica" }, 1, "Portugues", 2);
        AdicionarPergunta("Qual frase está na voz passiva?",
            new string[] { "O gato comeu o peixe", "O peixe foi comido pelo gato", "O gato estava com fome", "O peixe fugiu do gato" }, 1, "Portugues", 2);
        AdicionarPergunta("Qual é o grau superlativo absoluto de 'bom'?",
            new string[] { "boníssimo", "bonístico", "melhoríssimo", "ótimo" }, 3, "Portugues", 2);
        AdicionarPergunta("O que é uma metáfora?",
            new string[] { "Comparação com como", "Comparação sem como", "Exagero proposital", "Repetição de sons" }, 1, "Portugues", 2);
        AdicionarPergunta("Qual é o plural de 'pão'?",
            new string[] { "pãos", "pães", "pãoes", "pãons" }, 1, "Portugues", 2);

        // PORTUGUÊS — Nível 3
        AdicionarPergunta("O que é uma conjunção?",
            new string[] { "Liga substantivos", "Liga orações ou termos", "Modifica o verbo", "Substitui o substantivo" }, 1, "Portugues", 3);
        AdicionarPergunta("Qual figura de linguagem é 'a vida é uma viagem'?",
            new string[] { "Comparação", "Metáfora", "Hipérbole", "Personificação" }, 1, "Portugues", 3);
        AdicionarPergunta("Qual é o sujeito em 'Os alunos estudaram muito'?",
            new string[] { "estudaram", "muito", "Os alunos", "Os alunos estudaram" }, 2, "Portugues", 3);

        // ─────────────────────────────────────────
        // HISTÓRIA — Nível 1
        // ─────────────────────────────────────────
        AdicionarPergunta("Em que ano o Brasil foi descoberto?",
            new string[] { "1400", "1492", "1500", "1522" }, 2, "Historia", 1);
        AdicionarPergunta("Quem proclamou a Independência do Brasil?",
            new string[] { "Dom João VI", "Dom Pedro I", "Tiradentes", "Deodoro da Fonseca" }, 1, "Historia", 1);
        AdicionarPergunta("Quem foi o primeiro presidente do Brasil?",
            new string[] { "Dom Pedro II", "Getúlio Vargas", "Deodoro da Fonseca", "Juscelino Kubitschek" }, 2, "Historia", 1);
        AdicionarPergunta("Em que ano foi abolida a escravidão no Brasil?",
            new string[] { "1880", "1885", "1888", "1890" }, 2, "Historia", 1);
        AdicionarPergunta("Qual lei aboliu a escravidão no Brasil?",
            new string[] { "Lei do Ventre Livre", "Lei Áurea", "Lei dos Sexagenários", "Lei da Terra" }, 1, "Historia", 1);
        AdicionarPergunta("Em que continente surgiu a civilização egípcia?",
            new string[] { "Ásia", "Europa", "África", "América" }, 2, "Historia", 1);
        AdicionarPergunta("Quem foi Pedro Álvares Cabral?",
            new string[] { "Rei de Portugal", "Navegador que chegou ao Brasil", "Explorador espanhol", "Primeiro governador do Brasil" }, 1, "Historia", 1);

        // HISTÓRIA — Nível 2
        AdicionarPergunta("Qual foi a principal causa da Primeira Guerra Mundial?",
            new string[] { "Crise econômica mundial", "Assassinato do arquiduque Francisco Fernando", "Invasão da Polônia", "Revolução Russa" }, 1, "Historia", 2);
        AdicionarPergunta("Em que ano terminou a Segunda Guerra Mundial?",
            new string[] { "1943", "1944", "1945", "1946" }, 2, "Historia", 2);
        AdicionarPergunta("O que foi a Revolução Industrial?",
            new string[] { "Revolução política na França", "Transformação do trabalho artesanal em industrial", "Guerra entre nações industriais", "Movimento operário no Brasil" }, 1, "Historia", 2);
        AdicionarPergunta("Onde ocorreu a Revolução Francesa?",
            new string[] { "Inglaterra", "Alemanha", "França", "Itália" }, 2, "Historia", 2);
        AdicionarPergunta("Quem foi Tiradentes?",
            new string[] { "Poeta do século XVIII", "Líder da Inconfidência Mineira", "Primeiro imperador do Brasil", "General da Guerra do Paraguai" }, 1, "Historia", 2);

        // HISTÓRIA — Nível 3
        AdicionarPergunta("O que foi a Guerra Fria?",
            new string[] { "Guerra entre Brasil e Argentina", "Tensão entre EUA e URSS após a 2ª Guerra", "Conflito no Polo Norte", "Guerra na Península Coreana" }, 1, "Historia", 3);
        AdicionarPergunta("Em que ano ocorreu a Revolução Russa?",
            new string[] { "1905", "1914", "1917", "1921" }, 2, "Historia", 3);
        AdicionarPergunta("Qual foi o regime político de Hitler na Alemanha?",
            new string[] { "Comunismo", "Fascismo", "Nazismo", "Socialismo" }, 2, "Historia", 3);

        // ─────────────────────────────────────────
        // GEOGRAFIA — Nível 1
        // ─────────────────────────────────────────
        AdicionarPergunta("Qual é a capital do Brasil?",
            new string[] { "São Paulo", "Rio de Janeiro", "Salvador", "Brasília" }, 3, "Geografia", 1);
        AdicionarPergunta("Qual é o maior rio do mundo em volume de água?",
            new string[] { "Rio Nilo", "Rio Amazonas", "Rio Mississippi", "Rio Yangtzé" }, 1, "Geografia", 1);
        AdicionarPergunta("Quantos continentes existem no mundo?",
            new string[] { "5", "6", "7", "8" }, 2, "Geografia", 1);
        AdicionarPergunta("Qual é o maior oceano do mundo?",
            new string[] { "Atlântico", "Índico", "Ártico", "Pacífico" }, 3, "Geografia", 1);
        AdicionarPergunta("Qual é a capital da Argentina?",
            new string[] { "Santiago", "Lima", "Buenos Aires", "Montevidéu" }, 2, "Geografia", 1);
        AdicionarPergunta("Em qual continente fica o Brasil?",
            new string[] { "América do Norte", "América Central", "América do Sul", "América Latina" }, 2, "Geografia", 1);
        AdicionarPergunta("Qual é o maior país do mundo em território?",
            new string[] { "China", "EUA", "Brasil", "Rússia" }, 3, "Geografia", 1);
        AdicionarPergunta("Qual é a capital da França?",
            new string[] { "Londres", "Berlim", "Paris", "Roma" }, 2, "Geografia", 1);

        // GEOGRAFIA — Nível 2
        AdicionarPergunta("Qual é o deserto mais quente do mundo?",
            new string[] { "Gobi", "Atacama", "Saara", "Arábia" }, 2, "Geografia", 2);
        AdicionarPergunta("Qual é a montanha mais alta do mundo?",
            new string[] { "Monte Rosa", "K2", "Monte Everest", "Aconcágua" }, 2, "Geografia", 2);
        AdicionarPergunta("Qual é o menor país do mundo?",
            new string[] { "Mônaco", "San Marino", "Vaticano", "Liechtenstein" }, 2, "Geografia", 2);
        AdicionarPergunta("Qual é o rio mais longo do mundo?",
            new string[] { "Amazonas", "Nilo", "Yangtzé", "Mississippi" }, 1, "Geografia", 2);
        AdicionarPergunta("Qual é a capital do Japão?",
            new string[] { "Xangai", "Pequim", "Seul", "Tóquio" }, 3, "Geografia", 2);

        // GEOGRAFIA — Nível 3
        AdicionarPergunta("Qual é o país mais populoso do mundo?",
            new string[] { "Índia", "China", "EUA", "Indonésia" }, 0, "Geografia", 3);
        AdicionarPergunta("Qual linha imaginária divide o planeta em Norte e Sul?",
            new string[] { "Trópico de Câncer", "Meridiano de Greenwich", "Equador", "Trópico de Capricórnio" }, 2, "Geografia", 3);
        AdicionarPergunta("Qual é o maior bioma do Brasil?",
            new string[] { "Cerrado", "Mata Atlântica", "Amazônia", "Caatinga" }, 2, "Geografia", 3);

        // ─────────────────────────────────────────
        // CIÊNCIAS — Nível 1
        // ─────────────────────────────────────────
        AdicionarPergunta("Qual gás é essencial para a respiração humana?",
            new string[] { "Dióxido de carbono", "Nitrogênio", "Oxigênio", "Hidrogênio" }, 2, "Ciencias", 1);
        AdicionarPergunta("Quantos ossos tem o corpo humano adulto?",
            new string[] { "186", "196", "206", "216" }, 2, "Ciencias", 1);
        AdicionarPergunta("Qual é o maior planeta do sistema solar?",
            new string[] { "Saturno", "Urano", "Netuno", "Júpiter" }, 3, "Ciencias", 1);
        AdicionarPergunta("O que fazem as plantas na fotossíntese?",
            new string[] { "Absorvem oxigênio", "Produzem alimento usando luz solar", "Liberam gás carbônico", "Consomem água do solo" }, 1, "Ciencias", 1);
        AdicionarPergunta("Qual é o planeta mais próximo do Sol?",
            new string[] { "Vênus", "Terra", "Marte", "Mercúrio" }, 3, "Ciencias", 1);
        AdicionarPergunta("De que é feita a água?",
            new string[] { "Hidrogênio e nitrogênio", "Oxigênio e carbono", "Hidrogênio e oxigênio", "Carbono e hidrogênio" }, 2, "Ciencias", 1);
        AdicionarPergunta("Qual órgão bombeia o sangue no corpo humano?",
            new string[] { "Pulmão", "Fígado", "Coração", "Rim" }, 2, "Ciencias", 1);
        AdicionarPergunta("Qual é o nome da força que atrai os objetos para o chão?",
            new string[] { "Magnetismo", "Gravidade", "Atrito", "Inércia" }, 1, "Ciencias", 1);

        // CIÊNCIAS — Nível 2
        AdicionarPergunta("O que é um átomo?",
            new string[] { "Molécula de água", "Menor partícula de um elemento químico", "Tipo de célula", "Partícula de luz" }, 1, "Ciencias", 2);
        AdicionarPergunta("Qual é a fórmula química da água?",
            new string[] { "CO2", "NaCl", "H2O", "O2" }, 2, "Ciencias", 2);
        AdicionarPergunta("O que é a cadeia alimentar?",
            new string[] { "Sequência de elementos químicos", "Relação de alimentação entre seres vivos", "Ciclo da água na natureza", "Processo de digestão" }, 1, "Ciencias", 2);
        AdicionarPergunta("Qual é a unidade básica dos seres vivos?",
            new string[] { "Átomo", "Molécula", "Célula", "Tecido" }, 2, "Ciencias", 2);
        AdicionarPergunta("Qual gás as plantas liberam durante a fotossíntese?",
            new string[] { "Gás carbônico", "Nitrogênio", "Oxigênio", "Hidrogênio" }, 2, "Ciencias", 2);
        AdicionarPergunta("O que estuda a Zoologia?",
            new string[] { "As plantas", "Os animais", "Os fungos", "Os minerais" }, 1, "Ciencias", 2);

        // CIÊNCIAS — Nível 3
        AdicionarPergunta("O que é a teoria da evolução de Darwin?",
            new string[] { "Seres vivos foram criados de uma vez", "Espécies evoluem por seleção natural", "Animais não mudam ao longo do tempo", "Evolução ocorre por vontade própria" }, 1, "Ciencias", 3);
        AdicionarPergunta("Qual é a velocidade da luz no vácuo aproximadamente?",
            new string[] { "150.000 km/s", "200.000 km/s", "300.000 km/s", "400.000 km/s" }, 2, "Ciencias", 3);
        AdicionarPergunta("O que é o DNA?",
            new string[] { "Proteína celular", "Molécula que carrega informação genética", "Tipo de vírus", "Enzima digestiva" }, 1, "Ciencias", 3);

        // ─────────────────────────────────────────
        // ARTES — Nível 1
        // ─────────────────────────────────────────
        AdicionarPergunta("Quais são as cores primárias?",
            new string[] { "Verde, laranja e roxo", "Azul, vermelho e amarelo", "Branco, preto e cinza", "Rosa, azul e verde" }, 1, "Artes", 1);
        AdicionarPergunta("Quem pintou a Mona Lisa?",
            new string[] { "Michelangelo", "Rafael", "Leonardo da Vinci", "Picasso" }, 2, "Artes", 1);
        AdicionarPergunta("Qual é o instrumento de cordas mais famoso?",
            new string[] { "Flauta", "Tambor", "Violino", "Trompete" }, 2, "Artes", 1);
        AdicionarPergunta("O que é escultura?",
            new string[] { "Arte de pintar em tela", "Arte de criar formas em três dimensões", "Arte de fotografar", "Arte de escrever poemas" }, 1, "Artes", 1);

        // ─────────────────────────────────────────
        // EDUCAÇÃO FÍSICA — Nível 1
        // ─────────────────────────────────────────
        AdicionarPergunta("Quantos jogadores tem um time de futebol em campo?",
            new string[] { "9", "10", "11", "12" }, 2, "Ed. Fisica", 1);
        AdicionarPergunta("Qual é o esporte com mais praticantes no Brasil?",
            new string[] { "Vôlei", "Futebol", "Basquete", "Natação" }, 1, "Ed. Fisica", 1);
        AdicionarPergunta("Quantos sets são necessários para vencer no vôlei?",
            new string[] { "2", "3", "4", "5" }, 1, "Ed. Fisica", 1);
        AdicionarPergunta("Em que país se originou o judô?",
            new string[] { "China", "Coreia", "Japão", "Brasil" }, 2, "Ed. Fisica", 1);

        Debug.Log($"[QuestionManager] {bancoDePerguntass.Count} perguntas carregadas.");
    }

    void AdicionarPergunta(string enunciado, string[] alternativas, int respostaCorreta, string materia, int nivel)
    {
        bancoDePerguntass.Add(new Pergunta
        {
            enunciado             = enunciado,
            alternativas          = alternativas,
            indiceRespostaCorreta = respostaCorreta,
            materia               = materia,
            nivelMinimo           = nivel
        });
    }

    public void SortearPergunta()
    {
        int nivelAtual = GameManager.Instance != null ? GameManager.Instance.nivelAtual : 1;

        List<Pergunta> disponiveis = bancoDePerguntass.FindAll(p =>
            p.nivelMinimo <= nivelAtual && !perguntasUsadas.Contains(p)
        );

        if (disponiveis.Count == 0)
        {
            Debug.Log("[QuestionManager] Todas as perguntas usadas! Resetando histórico.");
            perguntasUsadas.Clear();
            disponiveis = bancoDePerguntass.FindAll(p => p.nivelMinimo <= nivelAtual);
        }

        int indice    = Random.Range(0, disponiveis.Count);
        perguntaAtual = disponiveis[indice];
        perguntasUsadas.Add(perguntaAtual);

        Debug.Log($"[QuestionManager] Pergunta sorteada: {perguntaAtual.enunciado}");
        EventoNovaPergunta?.Invoke(perguntaAtual);
    }

    public bool VerificarResposta(int indiceEscolhido)
    {
        if (perguntaAtual == null) return false;
        bool correto = indiceEscolhido == perguntaAtual.indiceRespostaCorreta;
        Debug.Log($"[QuestionManager] Escolhido: {indiceEscolhido} | Correto: {perguntaAtual.indiceRespostaCorreta} | {(correto ? "CERTO" : "ERRADO")}");
        return correto;
    }

    public int EliminarAlternativaErrada()
    {
        if (perguntaAtual == null) return -1;

        List<int> erradas = new List<int>();
        for (int i = 0; i < perguntaAtual.alternativas.Length; i++)
            if (i != perguntaAtual.indiceRespostaCorreta)
                erradas.Add(i);

        int indiceEliminado = erradas[Random.Range(0, erradas.Count)];
        Debug.Log($"[QuestionManager] Alternativa eliminada: índice {indiceEliminado}");
        return indiceEliminado;
    }
}
