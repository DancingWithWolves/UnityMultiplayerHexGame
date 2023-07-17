using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbobusMovementState : AbobusState
{
    public AbobusMovementState(GayManager gm, Abobus abobus_) : base(gm, abobus_) {}
    
    override public void Enter()
    {
        Debug.Log("Movement state enter");
        abobus.moved_this_turn = true;
        abobus.state = abobus.movement_state;
        gay_manager.DisableAbobi(abobus);
    }
    
    override public void HandleInput(HexCell hex_cell)
    {
        Debug.Log("Movement state handles input");
        abobus.MoveToHexCoordinates(hex_cell.hex_coordinates);
        abobus.chosen_state.Enter();
    }

}