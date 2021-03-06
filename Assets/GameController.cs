﻿using UnityEngine;
using tabuleiro;
using xadrez;
using UnityEngine.UI;
using System.Collections.Generic;

class GameController : MonoBehaviour
{

    public GameObject reiBranco = null;
    public GameObject reiPreto = null;
    public GameObject torreBranca = null;
    public GameObject torrePreta = null;
    public GameObject DamaBranca = null;
    public GameObject DamaPreta = null;
    public GameObject PeaoPreto = null;
    public GameObject PeaoBranco = null;
    public GameObject bispoBranco = null;
    public GameObject bispoPreto = null;
    public GameObject cavaloBranco = null;
    public GameObject cavaloPreto = null;




    public Text txtMsg = null;
    public Text txtXeque = null;

    public GameObject pecaEscolhida { get; private set; }

    public Estado estado { get; private set; }

    PartidaDeXadrez partida;
    PosicaoXadrez origem, destino;
    Color corOriginal;

    Vector3 posDescaerteBrancas, posDescartePretas;

    public GameObject particulas;

    List<GameObject> listaParticulas;

    void Start()
    {
        estado = Estado.AguardandoJogada;
        pecaEscolhida = null;
        corOriginal = txtMsg.color;
        listaParticulas = new List<GameObject>();

        posDescaerteBrancas = new Vector3(-8.5f, 0f, -9f);
        posDescartePretas = new Vector3(8.5f, 0f, 9f);

        partida = new PartidaDeXadrez();

        txtXeque.text = "";
        InformarAguardando();


        Util.instanciarCavalo('b', 1, Cor.Branca, partida, cavaloBranco);
        Util.instanciarCavalo('g', 1, Cor.Branca, partida, cavaloBranco);
        Util.instanciarBispo('f', 1, Cor.Branca, partida, bispoBranco);
        Util.instanciarBispo('c', 1, Cor.Branca, partida, bispoBranco);
        Util.instanciarDama('d', 1, Cor.Branca, partida, DamaBranca);
        Util.instanciarPeao('a', 2, Cor.Branca, partida, PeaoBranco);
        Util.instanciarPeao('b', 2, Cor.Branca, partida, PeaoBranco);
        Util.instanciarPeao('c', 2, Cor.Branca, partida, PeaoBranco);
        Util.instanciarPeao('d', 2, Cor.Branca, partida, PeaoBranco);
        Util.instanciarPeao('e', 2, Cor.Branca, partida, PeaoBranco);
        Util.instanciarPeao('f', 2, Cor.Branca, partida, PeaoBranco);
        Util.instanciarPeao('g', 2, Cor.Branca, partida, PeaoBranco);
        Util.instanciarPeao('h', 2, Cor.Branca, partida, PeaoBranco);
        Util.instanciarRei('e', 1, Cor.Branca, partida, reiBranco);
        Util.instanciarTorre('a', 1, Cor.Branca, partida, torreBranca);
        Util.instanciarTorre('h', 1, Cor.Branca, partida, torreBranca);

        Util.instanciarRei('e', 8, Cor.Preta, partida, reiPreto);
        Util.instanciarTorre('a', 8, Cor.Preta, partida, torrePreta);
        Util.instanciarTorre('h', 8, Cor.Preta, partida, torrePreta);
        Util.instanciarCavalo('b', 8, Cor.Preta, partida, cavaloPreto);
        Util.instanciarCavalo('g', 8, Cor.Preta, partida, cavaloPreto);
        Util.instanciarBispo('c', 8, Cor.Preta, partida, bispoPreto);
        Util.instanciarBispo('f', 8, Cor.Preta, partida, bispoPreto);
        Util.instanciarDama('d', 8, Cor.Preta, partida, DamaPreta);
        Util.instanciarPeao('a', 7, Cor.Preta, partida, PeaoPreto);
        Util.instanciarPeao('b', 7, Cor.Preta, partida, PeaoPreto);
        Util.instanciarPeao('c', 7, Cor.Preta, partida, PeaoPreto);
        Util.instanciarPeao('d', 7, Cor.Preta, partida, PeaoPreto);
        Util.instanciarPeao('e', 7, Cor.Preta, partida, PeaoPreto);
        Util.instanciarPeao('f', 7, Cor.Preta, partida, PeaoPreto);
        Util.instanciarPeao('g', 7, Cor.Preta, partida, PeaoPreto);
        Util.instanciarPeao('h', 7, Cor.Preta, partida, PeaoPreto);

    }
    public void processarMouseDown(GameObject obj, GameObject casa)
    {
        if (estado == Estado.AguardandoJogada)
        {
            if (casa != null)
            {
                try
                {
                    char coluna = casa.name[0];
                    int linha = casa.name[1] - '0';
                    origem = new PosicaoXadrez(coluna, linha);
                    partida.validarPosicaoDeOrigem(origem.toPosicao());
                    pecaEscolhida = obj;
                    estado = Estado.Arrastando;
                    txtMsg.text = "Selecione a casa de destino";
                    GameObject.Find("somClique").GetComponent<AudioSource>().Play();
                    instanciarParticulas();
                }
                catch (TabuleiroException e)
                {
                    InformarAviso(e.Message);
                    GameObject.Find("somErro").GetComponent<AudioSource>().Play();
                }
            }
        }
        else if (estado == Estado.Arrastando)
        {
            GameObject casaDestino = null;
            if (obj.layer == LayerMask.NameToLayer("Casas"))
            {
                casaDestino = obj;
            }
            else
            {
                casaDestino = casa;
            }
            if (casaDestino != null && pecaEscolhida != null)
            {
                try
                {

                    char coluna = casaDestino.name[0];
                    int linha = casaDestino.name[1] - '0';
                    destino = new PosicaoXadrez(coluna, linha);

                    partida.validarPosicaoDeDestino(origem.toPosicao(), destino.toPosicao());
                    Peca pecaCapturada = partida.realizaJogada(origem.toPosicao(), destino.toPosicao());

                    if (pecaCapturada != null)
                    {
                        removerObjetoCapturado(pecaCapturada);
                        GameObject.Find("somCaptura").GetComponent<AudioSource>().Play();
                    }
                    else
                    {
                        GameObject.Find("somClique").GetComponent<AudioSource>().Play();
                    }
                    pecaEscolhida.transform.position = Util.posicaoNaCena(coluna, linha);


                    tratarJogadasEspeciais();

                    pecaEscolhida = null;

                    if (partida.terminada)
                    {
                        estado = Estado.GameOver;
                        txtMsg.text = "Vencedor: " + partida.jogadorAtual;
                        txtXeque.text = "XEQUEMATE";
                    }
                    else
                    {
                        estado = Estado.AguardandoJogada;
                        InformarAguardando();
                        Invoke("girarCamera", 0.5f);
                        if (partida.xeque)
                        {

                            txtXeque.text = "XEQUE";
                        }
                        else
                        {
                            txtXeque.text = "";
                        }
                    }
                }
                catch (TabuleiroException e)
                {
                    pecaEscolhida.transform.position = Util.posicaoNaCena(origem.coluna, origem.linha);
                    estado = Estado.AguardandoJogada;
                    InformarAviso(e.Message);
                    GameObject.Find("somErro").GetComponent<AudioSource>().Play();



                }
                finally
                {
                    destruirParticulas();
                }
            }
        }
    }

    void InformarAviso(string msg)
    {
        txtMsg.color = Color.red;
        txtMsg.text = msg;
        Invoke("informarAguardando", 1f);
    }

    void InformarAguardando()
    {
        txtMsg.color = corOriginal;
        txtMsg.text = "Aguardando jogada: " + partida.jogadorAtual;
    }

    void removerObjetoCapturado(Peca peca)
    {
        GameObject obj = peca.obj;
        if (peca.cor == Cor.Branca)
        {
            obj.transform.position = posDescaerteBrancas;
            posDescaerteBrancas.z = posDescaerteBrancas.z + 1.5f;
        }
        else
        {
            obj.transform.position = posDescartePretas;
            posDescartePretas.z = posDescartePretas.z - 1.5f;
        }
    }

    void tratarJogadasEspeciais()
    {
        Posicao pos = destino.toPosicao();
        Peca pecaMovida = partida.tab.peca(pos);

        // #jogadaespecial roquepequeno
        if (pecaMovida is Rei && destino.coluna == origem.coluna + 2)
        {
            GameObject torre = partida.tab.peca(pos.linha, pos.coluna - 1).obj;
            torre.transform.position = Util.posicaoNaCena('f', origem.linha);
        }

        // #jogadaespecial roquegrande
        if (pecaMovida is Rei && destino.coluna == origem.coluna - 2)
        {
            GameObject torre = partida.tab.peca(pos.linha, pos.coluna + 1).obj;
            torre.transform.position = Util.posicaoNaCena('d', origem.linha);
        }

        // #jogada promocao 
        if (partida.promovida != null)
        {
            removerObjetoCapturado(partida.promovida);
            Vector3 posPromovida = Util.posicaoNaCena(destino.coluna, destino.linha);
            GameObject prefab = (pecaMovida.cor == Cor.Branca) ? DamaBranca : DamaPreta;
            GameObject dama = Instantiate(prefab, posPromovida, Quaternion.identity) as GameObject;
            pecaMovida.obj = dama;

        }
    }
    void girarCamera()
    {
        if(partida.jogadorAtual == Cor.Branca)
        {
            Camera.main.GetComponent<CameraRotacao>().irParaBranca();
        }
        else
        {
            Camera.main.GetComponent<CameraRotacao>().irParaPreta ();
        }
    }
    void instanciarParticulas()
    {
        listaParticulas.Clear();
        float y = GameObject.Find("PlanoDasPecas").transform.position.y;

        bool[ , ] mat =  partida.tab.peca(origem.toPosicao()).movimentosPossiveis();
        for (int i = 0; i<partida.tab.linhas; i++)
        {
            for (int j = 0; j <partida.tab.colunas; j++)
            {
                if (mat[i,j])
                {
                    char coluna = (char) ('a' + j);
                    int linha = 8 - i;
                    Vector3 posCasa = GameObject.Find("" + coluna + linha).transform.position;
                    Vector3 pos = new Vector3(posCasa.x, y, posCasa.z);
                    GameObject obj = Instantiate(particulas, pos, Quaternion.identity) as GameObject;
                    listaParticulas.Add(obj);
                }
            }
        }
    }
    void destruirParticulas()
    {
        foreach (GameObject obj in listaParticulas)
        {
            Destroy(obj);
        }
        listaParticulas.Clear();
    }
}