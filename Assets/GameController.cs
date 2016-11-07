using UnityEngine;
using tabuleiro;
using xadrez;

class GameController : MonoBehaviour {

    public GameObject reiBranco = null;

    PartidaDeXadrez partida;

	void Start () {
        partida = new PartidaDeXadrez();
        util.instanciarRei('e', 1, Cor.Branca, partida, reiBranco);
	}
}
