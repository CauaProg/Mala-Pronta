using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class VerificacaoLista : MonoBehaviour
{
    public Button camisaCor1;
    public Button tenis;
    public Button calca;
    public Button agasalho;
    public Button meias;
    public Button cuecas;
    public Button camisaCor2;
    public Button bermuda;
    public Button desodorante;
    public Button shampoo;
    public Button sabonete;
    public Button escovaDente;
    public Button fecharLista;
    public Button abrirLista;
    public Button botaoRelatorio;
    public TextMeshProUGUI listaItens;

    int[] gabarito;
    int[] gabaritoRng;
    int[] respostas;

    string[] lista = {
        "Camisa cor 1", "Tênis", "Calça", "Agasalho", "Meias", "Cuecas",
        "Camisa cor 2", "Bermuda", "Desodorante", "Shampoo", "Sabonete", "Escova De dente"
    };

    int faseAtual = 1;
    bool listaAberta = false;
    public float timeAbertura = 0f;
    public int contadorListaAberta = 0;

    HashSet<int> botoesDesativados = new HashSet<int>();
    string CameraAtiva = "Armario";

    void Start()
    {
        NovaFase();

        camisaCor1.onClick.AddListener(() => SelecionarOpcao(0, camisaCor1));
        tenis.onClick.AddListener(() => SelecionarOpcao(1, tenis));
        calca.onClick.AddListener(() => SelecionarOpcao(2, calca));
        agasalho.onClick.AddListener(() => SelecionarOpcao(3, agasalho));
        meias.onClick.AddListener(() => SelecionarOpcao(4, meias));
        cuecas.onClick.AddListener(() => SelecionarOpcao(5, cuecas));
        camisaCor2.onClick.AddListener(() => SelecionarOpcao(6, camisaCor2));
        bermuda.onClick.AddListener(() => SelecionarOpcao(7, bermuda));
        desodorante.onClick.AddListener(() => SelecionarOpcao(8, desodorante));
        shampoo.onClick.AddListener(() => SelecionarOpcao(9, shampoo));
        sabonete.onClick.AddListener(() => SelecionarOpcao(10, sabonete));
        escovaDente.onClick.AddListener(() => SelecionarOpcao(11, escovaDente));

        abrirLista.onClick.AddListener(() => mostrarLista());
        fecharLista.onClick.AddListener(() => esconderLista());
    }

    int[] Embaralhar(int[] array)
    {
        int[] copia = (int[])array.Clone();
        System.Random rng = new System.Random();
        int n = copia.Length;

        while (n > 1)
        {
            int k = rng.Next(n--);
            int temp = copia[n];
            copia[n] = copia[k];
            copia[k] = temp;
        }

        return copia;
    }

    void SelecionarOpcao(int valor, Button botao)
    {
        for (int i = 0; i < respostas.Length; i++)
        {
            if (respostas[i] == -1)
            {
                respostas[i] = valor;
                break;
            }
        }

        botao.gameObject.SetActive(false);
        botoesDesativados.Add(valor);

        var pontos = Verificar();

        if (System.Array.IndexOf(respostas, -1) == -1)
        {
            DadosFinais.pontuacaoTotal += pontos.pontos;
            DadosFinais.acertosOrdem += pontos.acertosExatos;
            DadosFinais.acertosForaOrdem += pontos.acertosFora;
            faseAtual++;
            NovaFase();
        }
    }

    (int acertosExatos, int acertosFora, int pontos) Verificar()
    {
        int acertosExatos = 0, acertosFora = 0, pontos = 0;

        List<bool> gabaritoMarcado = new List<bool>(new bool[gabaritoRng.Length]);
        List<bool> respostasMarcadas = new List<bool>(new bool[respostas.Length]);

        for (int i = 0; i < gabaritoRng.Length; i++)
        {
            if (respostas[i] != -1 && gabaritoRng[i] == respostas[i])
            {
                acertosExatos++;
                pontos += 3;
                gabaritoMarcado[i] = true;
                respostasMarcadas[i] = true;
            }
        }

        for (int i = 0; i < respostas.Length; i++)
        {
            if (respostasMarcadas[i] || respostas[i] == -1) continue;

            for (int j = 0; j < gabaritoRng.Length; j++)
            {
                if (!gabaritoMarcado[j] && respostas[i] == gabaritoRng[j])
                {
                    acertosFora++;
                    pontos += 1;
                    gabaritoMarcado[j] = true;
                    respostasMarcadas[i] = true;
                    break;
                }
            }
        }

        return (acertosExatos, acertosFora, pontos);
    }

    void NovaFase()
    {
        if (faseAtual <= 5)
        {
            int tamanho = 4 + (faseAtual - 1) * 2;

            gabarito = new int[tamanho];
            respostas = new int[tamanho];
            botoesDesativados.Clear();

            for (int i = 0; i < tamanho; i++)
            {
                gabarito[i] = i;
                respostas[i] = -1;
            }

            gabaritoRng = Embaralhar(gabarito);

            string listaConcatenada = "";
            for (int i = 0; i < tamanho; i++)
                listaConcatenada += $"{i + 1}. {lista[gabaritoRng[i]]}\n";

            listaItens.text = listaConcatenada;

            mostrarLista();
            StartCoroutine(esconderListaInicio());
        }
        else
        {
            if (listaAberta)
                esconderLista();

            DadosFinais.vezesListaAberta = contadorListaAberta - 1;
            DadosFinais.qtdErros = 40 - (DadosFinais.acertosForaOrdem + DadosFinais.acertosOrdem);

            SceneManager.LoadScene("relatorio");
        }

        AtualizarBotoes();
    }

    IEnumerator esconderListaInicio()
    {
        yield return new WaitForSeconds(10f);
        esconderLista();
    }

    void mostrarLista()
    {
        if (!listaAberta)
        {
            listaItens.gameObject.SetActive(true);
            abrirLista.gameObject.SetActive(false);
            fecharLista.gameObject.SetActive(true);

            DesativarTodosBotoes();

            listaAberta = true;
            timeAbertura = Time.time;
            contadorListaAberta++;
        }
    }

    void esconderLista()
    {
        if (listaAberta)
        {
            listaItens.gameObject.SetActive(false);
            fecharLista.gameObject.SetActive(false);
            abrirLista.gameObject.SetActive(true);

            AtualizarBotoes();

            listaAberta = false;
            DadosFinais.tempoListaAberta += Time.time - timeAbertura;
        }
    }

    void DesativarTodosBotoes()
    {
        for (int i = 0; i < lista.Length; i++)
        {
            Button b = ObterBotaoPorIndice(i);
            if (b != null)
                b.gameObject.SetActive(false);
        }
    }

    void AtualizarBotoes()
    {
        for (int i = 0; i < lista.Length; i++)
        {
            Button b = ObterBotaoPorIndice(i);
            if (b != null)
            {
                bool clicado = botoesDesativados.Contains(i);
                bool visivelNaCamera = IsBotaoVisivelPorCamera(i);
                b.gameObject.SetActive(!clicado && visivelNaCamera);
            }
        }
    }

    bool IsBotaoVisivelPorCamera(int index)
    {
        return CameraAtiva switch
        {
            "Armario" => index >= 0 && index <= 7,
            "Banheiro" => index >= 8 && index <= 11,
            "Mesa" => false,
            _ => false
        };
    }

    public void AtualizarVisibilidadePorCamera(string novaCamera)
    {
        CameraAtiva = novaCamera;
        AtualizarBotoes();
    }

    Button ObterBotaoPorIndice(int index)
    {
        return index switch
        {
            0 => camisaCor1,
            1 => tenis,
            2 => calca,
            3 => agasalho,
            4 => meias,
            5 => cuecas,
            6 => camisaCor2,
            7 => bermuda,
            8 => desodorante,
            9 => shampoo,
            10 => sabonete,
            11 => escovaDente,
            _ => null
        };
    }
}

public static class DadosFinais
{
    public static int pontuacaoTotal;
    public static int acertosOrdem;
    public static int acertosForaOrdem;
    public static int qtdErros;
    public static float tempoListaAberta;
    public static int vezesListaAberta;
    public static int teste;
}
