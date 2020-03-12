using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicTacToe : MonoBehaviour
{
    public int player;
    public bool won;

    public List<Case> cases;
    public Text crossScoreTxt;
    int crossScore;
    public Text circleScoreTxt;
    int circleScore;
    public Text winnerTxt;
    public Button replayButton;

    // Start is called before the first frame update
    void Start()
    {
        reset();
    }

    public void reset()
    {
        player = 0;
        won = false;
        winnerTxt.gameObject.SetActive(false);
        replayButton.gameObject.SetActive(false);
        foreach (Case c in cases)
        {
            c.assignment = Case.player.Unset;
            c.setImg();
        }
    }

    public void checkWin(int[] position)
    {
        if (cases[0].assignment == cases[1].assignment && cases[1].assignment == cases[2].assignment && cases[0].assignment != Case.player.Unset)
        {
            setWinner();
        }
        if (cases[3].assignment == cases[4].assignment && cases[4].assignment == cases[5].assignment && cases[3].assignment != Case.player.Unset)
        {
            setWinner();
        }
        if (cases[6].assignment == cases[7].assignment && cases[7].assignment == cases[8].assignment && cases[6].assignment != Case.player.Unset)
        {
            setWinner();
        }
        if (cases[0].assignment == cases[3].assignment && cases[3].assignment == cases[6].assignment && cases[0].assignment != Case.player.Unset)
        {
            setWinner();
        }
        if (cases[1].assignment == cases[4].assignment && cases[4].assignment == cases[7].assignment && cases[1].assignment != Case.player.Unset)
        {
            setWinner();
        }
        if (cases[2].assignment == cases[5].assignment && cases[5].assignment == cases[8].assignment && cases[2].assignment != Case.player.Unset)
        {
            setWinner();
        }
        if (cases[0].assignment == cases[4].assignment && cases[4].assignment == cases[8].assignment && cases[0].assignment != Case.player.Unset)
        {
            setWinner();
        }
        if (cases[2].assignment == cases[4].assignment && cases[4].assignment == cases[6].assignment && cases[2].assignment != Case.player.Unset)
        {
            setWinner();
        }

        foreach (Case c in cases)
        {
            if (c.assignment == Case.player.Unset)
                return;
        }
        SetDraw();
    }

    public void setWinner()
    {
        won = true;
        if (player == 0)
        {
            crossScore++;
            winnerTxt.text = "Cross Win !";
        }
        else
        {
            circleScore++;
            winnerTxt.text = "Circle Win !";
        }
        crossScoreTxt.text = "Cross : " + crossScore.ToString();
        circleScoreTxt.text = "Circle : " + circleScore.ToString();

        winnerTxt.gameObject.SetActive(true);
        replayButton.gameObject.SetActive(true);
    }

    public void SetDraw()
    {
        winnerTxt.gameObject.SetActive(true);
        winnerTxt.text = "Draw !";
        replayButton.gameObject.SetActive(true);
    }

    public void changePlayer()
    {
        player++;
        player = player % 2;
    }
}


