using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Case : MonoBehaviour
{
    public int[] position;
    public enum player { Unset, Circle, Cross };
    public player assignment;
    public TicTacToe ttt;
    public Sprite cross, circle;

    public void SetPlayer()
    {
        if (assignment != player.Unset || ttt.won)
            return;
        if (ttt.player == 0)
            assignment = player.Cross;
        else if (ttt.player == 1)
            assignment = player.Circle;
        setImg();
        ttt.checkWin();
        ttt.changePlayer();
    }

    public void setImg()
    {
        if (assignment == player.Cross)
            transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = cross;
        if (assignment == player.Circle)
            transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = circle;
        if (assignment == player.Unset)
            transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = null;
    }
}
