using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class relatorioFinal : MonoBehaviour
{
    public TextMeshProUGUI textoRelatorio;

    void Start()
    {
        string relatorio =
            $"Parabéns por completar o jogo!\n\n" +
            $"Pontuação final: {DadosFinais.pontuacaoTotal}\n" +
            $"Acertos na posição correta: {DadosFinais.acertosOrdem}\n" +
            $"Acertos fora da posição: {DadosFinais.acertosForaOrdem}\n" +
            $"Número de erros: {DadosFinais.qtdErros}\n" +
            $"Tempo que a lista ficou aberta: {DadosFinais.tempoListaAberta:F0}s\n" +
            $"Número de vezes que a lista foi aberta: {DadosFinais.vezesListaAberta}\n";

        textoRelatorio.text = relatorio;

    }
}

