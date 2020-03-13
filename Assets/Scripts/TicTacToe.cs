using System;
using System.Linq;
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

    public void checkWin()
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

        if (player == 1)
        {

        }
    }

    private int PlayerToInt(Case.player p)
    {
        switch (p)
        {
            case Case.player.Unset:
                return 0;
            case Case.player.Circle:
                return 1;
            case Case.player.Cross:
                return 2;
            default:
                return 0;
        }
    }

    private Case.player IntToPlayer(int p)
    {
        switch (p)
        {
            case 0:
                return Case.player.Unset;
            case 1:
                return Case.player.Circle;
            case 2:
                return Case.player.Cross;
            default:
                return Case.player.Unset;
        }
    }

    private int IntPow(int x, int pow)
    {
        int ret = 1;
        while ( pow != 0 )
        {
            if ( (pow & 1) == 1 )
                ret *= x;
            x *= x;
            pow >>= 1;
        }
        return ret;
    }

    private int BuildStateInt()
    {
        int res = 0;

        for (int i = 0; i < 9; ++i)
        {
            int val = PlayerToInt(cases[i].assignment) * IntPow(10, i);
            res += val;
        }

        return res;
    }

    public void SetStateFromInt(int s)
    {
        for (int i = 0; i < 9; ++i)
        {
            int val = s / IntPow(10, i) % 10;
            cases[i].assignment = IntToPlayer(val);
        }
    }

    public (float, int) Step(int s, int a, int callingPlayer)
    {
        SetStateFromInt(s);
        cases[a].assignment = IntToPlayer(2 - player);

        checkWin();
        changePlayer();

        for (int i = 0; i < 9; ++i)
        {
            if (cases[i].assignment == Case.player.Unset)
            {
                cases[i].assignment = IntToPlayer(2 - (1 - player));
                break;
            }
        }

        int s_p = BuildStateInt();

        if (won == false) {
            return (0f, s_p);
        }

        if (player != callingPlayer) // Player was already changed, thus !=
        {
            return (1f, s_p);
        }

        return (-1f, s_p);
    }

    public (int[], int[], float[], int[]) StepUntilEnd(int s, int callingPlayer, float[,] Pi)
    {
        List<int> s_list = new List<int>();
        List<int> a_list = new List<int>();
        List<float> r_list = new List<float>();
        List<int> s_p_list = new List<int>();

        while (!won)
        {
            int a = RandomActionWeighted(Pi, s);
            (float r, int s_p) = Step(s, a, callingPlayer);
            s_list.Add(s);
            a_list.Add(a);
            r_list.Add(r);
            s_p_list.Add(s_p);
            s = s_p;
        }

        return (s_list.ToArray(), a_list.ToArray(), r_list.ToArray(), s_p_list.ToArray());
    }

    private int RandomActionWeighted(float[,] Pi, int s)
    {
        var sortedActions = Enumerable.Range(0, 9).Select(x => (Pi[s, x], x)).OrderBy(x => x.Item1).ToArray();

        System.Random rand = new System.Random();
        double val = rand.NextDouble();
        double cum = 0f;

        for (int i = 0; i < 9; ++i)
        {
            cum += sortedActions[0].Item1;
            if (cum >= val)
            {
                return i;
            }
        }

        return -1;
    }
}


