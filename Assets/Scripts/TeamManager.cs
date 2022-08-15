using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public bool isHome;
    public static TeamState teamState;
    public TeamData team;
    public FormationType formation;
    public PlayerManager[] playerList;
    public PlayerManager[] playerSort;
    public Vector3[] position;
    private void Awake() {
        position = new Vector3[11];
        playerList = GetComponentsInChildren<PlayerManager>();
        playerSort = GetComponentsInChildren<PlayerManager>();
        if(isHome)
        {
            teamState = TeamState.AttackKickOff;
        }
        else
        {
            teamState = TeamState.DeffendKickOff;
        }
        
        switch(formation)
        {
            default:
            case FormationType.Form442:
            {
                break;
            }
            case FormationType.Form433:
            {
                if(teamState == TeamState.AttackKickOff)
                    Form433KickOffAtk();
                else if(teamState == TeamState.DeffendKickOff)
                    Form433KickOffDef();
                break;
            }
        }
        for (int i = 0; i < playerList.Length; i++)
        {
            playerList[i].transform.position = position[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        SelectionSort(playerSort);
        for (int i = 0; i < playerList.Length; i++)
        {
            playerList[i].playerData = team.lineUp[i];
        }
    }
    void SelectionSort(PlayerManager[] players)
    {
        int min;
        PlayerManager temp;

        for (int i = 0; i < players.Length; i++)
        {
            min = i;
            for (int j = 1 + i; j < players.Length; j++)
            {
                if(players[j].DistanceToBall < players[min].DistanceToBall)
                {
                    min = j;
                }
            }
            if(min != i)
            {
                temp = players[i];
                players[i] = players[min];
                players[min] = temp;
            }
        }
    }
    public enum FormationType
    {
       Form442,
        Form433,
        Form4231,

    };
    static int size = 11;
    public void Form433KickOffAtk()
    {
        position[0] = new Vector3(-57, 0, 0);
        position[1] = new Vector3(-40, 0, -14);
        position[2] = new Vector3(-40, 0, 14);
        position[3] = new Vector3(-33, 0, -30);
        position[4] = new Vector3(-33, 0, 30);
        position[5] = new Vector3(-15, 0, -24);
        position[6] = new Vector3(-17, 0, 0);
        position[7] = new Vector3(-15, 0, 24);
        position[8] = new Vector3(-2, 0, -28);
        position[9] = new Vector3(-2, 0, 28);
        position[10] = new Vector3(0, 0, 0.5f);
    }
    public void Form433KickOffDef()
    {
        position[0] = new Vector3(-57, 0, 0);
        position[1] = new Vector3(-40, 0, -14);
        position[2] = new Vector3(-40, 0, 14);
        position[3] = new Vector3(-33, 0, -30);
        position[4] = new Vector3(-33, 0, 30);
        position[5] = new Vector3(-15, 0, -24);
        position[6] = new Vector3(-17, 0, 0);
        position[7] = new Vector3(-15, 0, 24);
        position[8] = new Vector3(-2, 0, -28);
        position[9] = new Vector3(-2, 0, 28);
        position[10] = new Vector3(-1, 0, 11);
    }

}
