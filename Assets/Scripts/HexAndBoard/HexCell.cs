using UnityEngine;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Unity.VisualScripting;
using System;

public class HexCell : MonoBehaviour {
	public HexCoordinates hex_coordinates;
    public GameManager game_manager;
    public Abobus abobus;
    public State state;
    public enum State {
        out_of_bounds, abobus, empty
    };

    public List<Action> actions;
    public String debug_str;

    public override string ToString () {
        String ans = "";
        foreach (Action action in actions ) {
            ans += action.ToString() + '\n';
        }
		return ans;
	}

    void Awake ()
    {
        game_manager = GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<GameManager>();
        actions = new List<Action>();
    }

    void Update () {
        debug_str = ToString();
    }
    public void Refresh ()
    {
        // skills.Clear();
        abobus = game_manager.GetAbobusByHexCoordinates(hex_coordinates);
        if (abobus) {
            state = State.abobus;
        } else if (game_manager.hex_grid.CheckHexCoordsOutOfBounds(hex_coordinates)) {
            state = State.out_of_bounds;
        } else {
            state = State.empty;
        }
        
    }

    public void React()
    {
        Debug.Log("Cell <color=yellow>" + hex_coordinates.ToString() + "</color> reacts:\n" + ToString());

        for (int i = 0; i < actions.Count; ++i) {
            actions[i].Invoke();
        }              
        
    }

}