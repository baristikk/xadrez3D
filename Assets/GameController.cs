using UnityEngine;
using tabuleiro;
using xadrez;

class GameController : MonoBehaviour {

    public GameObject reiBranco = null;
    public GameObject reiPreto = null;

    PartidaDeXadrez partida;

	void Start () {
        partida = new PartidaDeXadrez();
        util.instanciarRei('e', 1, Cor.Branca, partida, reiBranco);
        util.instanciarRei('e', 8, Cor.Preta, partida, reiPreto);
    }
}
