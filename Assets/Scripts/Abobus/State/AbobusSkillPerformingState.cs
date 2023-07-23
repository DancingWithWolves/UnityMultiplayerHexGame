using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbobusSkillPerformingState : AbobusState
{
    public AbobusSkillPerformingState(GayManager gm, Abobus abobus_) : base(gm, abobus_) {
        Refresh();
    }
    
    public HexCell applied_to;

    override public void Enter()
    {
        if (applied_to == null) {
            Debug.Log($"<color=red>ERROR</color> Cannot perform skill with applied_to = null!");
            return;
        }
        entered = true;
        gay_manager.DisableAbobi(abobus.team, abobus);

        List<HexCoordinates> skill_performing_coords_list = abobus.GetPossibleSkillTriggerTurns();
        foreach (HexCoordinates hex_coords in skill_performing_coords_list) {
            HexCell hex_cell = gay_manager.hex_grid.GetCellByHexCoordinates(hex_coords);
            hex_cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_yellow);
        }
    }
    
    override public void HandleInput(HexCell hex_cell = null)
    {
        if (hex_cell != null) {
            if (hex_cell.GetComponent<HighlightableCell>().GetState() == HighlightableCell.State.highlighted_green) {
                Debug.Log("Skill performing state handles input", abobus);
                abobus.PerformSkill(applied_to, hex_cell);
                applied_to = null;
            }
        }
        
    }

    override public void Refresh()
    {
        applied_to = null;
        entered = false;
    }
    override public void Exit()
    {

    }
}