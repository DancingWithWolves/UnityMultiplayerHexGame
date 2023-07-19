using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbobusChosenState : AbobusState
{
    public bool entered;
    public AbobusChosenState(GayManager gm, Abobus abobus_) : base(gm, abobus_) {}
    override public void Enter()
    {
        Debug.Log("Chosen state enter");
        gay_manager.ClearAllHighlightedCells();

        if (abobus.state == abobus.disabled_state) {
            return;
        }
        abobus.state = abobus.chosen_state;
        
        if (!entered) {
            entered = true;
            abobus.transform.position += new Vector3(0, 10, 0);
        }

        if ( (!abobus.movement_state.entered) && (!abobus.skill_performing_state.entered) ) {
            Debug.Log("Highlighting possible movement cells");
            List<HexCoordinates> movement_coords_list = abobus.GetPossibleMovementTurns();
            foreach (HexCoordinates hex_coordinates in movement_coords_list) {
                HexCell hex_cell = gay_manager.hex_grid.GetCellByHexCoordinates(hex_coordinates);
                hex_cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_green);
            }
        }
        
        if (abobus.movement_state.entered) {
            // �筮 ��室���, ⥯��� ���� �������� �� ᪨�, ���� ���
            Debug.Log("Checking if the turn is over");
            if (abobus.GetPossibleSkillTriggerTurns().Count == 0) {
                // ���������� �� ᪨�, 室 �����祭
                abobus.disabled_state.Enter();
                gay_manager.SwitchTurn();
                return;
            } 
        }
        // ���ᢥ⪠ 室�� ��� ������ ᪨��
        if (!abobus.skill_performing_state.entered) {
            Debug.Log("Highlighting skill trigger cells");
            List<HexCoordinates> skill_trigger_coords_list = abobus.GetPossibleSkillTriggerTurns();
            foreach (HexCoordinates hex_coords in skill_trigger_coords_list) {
                HexCell hex_cell = gay_manager.hex_grid.GetCellByHexCoordinates(hex_coords);
                hex_cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_yellow);
            }
        } else {
            Debug.Log("Highlighting skill performing cells");
            if (abobus.skill_performing_state.applied_to == null) {
                // �뤥����� ᯠ��, ������ �����稫 �ᯮ�짮���� ᪨�
            } else {
                // �뤥����� ��⨢��, ���� ��ନ஢��� �������� ���⪨ �ਬ������ ᪨��
                gay_manager.ClearAllHighlightedCells();
                List<HexCoordinates> skill_coords_list = abobus.GetPossibleSkillTurns(abobus.skill_performing_state.applied_to);
                foreach (HexCoordinates hex_coords in skill_coords_list) {
                    HexCell hex_cell = gay_manager.hex_grid.GetCellByHexCoordinates(hex_coords);
                    hex_cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_yellow);
                }
                if (skill_coords_list.Count == 0) {
                    Debug.Log("No suitable cells to perform skill");
                }
            }
        }
    }
    
    override public void HandleInput(HexCell hex_cell = null)
    {   
        if (abobus.state == abobus.disabled_state) {
            return;
        }
        Debug.Log("Chosen state handles input");
        // gay_manager.ClearAllHighlightedCells();
        if (hex_cell != null) {
            if (hex_cell.GetComponent<HighlightableCell>().GetState() == HighlightableCell.State.highlighted_green) {
                abobus.movement_state.Enter();
                abobus.movement_state.HandleInput(hex_cell);
                Enter();
            }
            if (hex_cell.GetComponent<HighlightableCell>().GetState() == HighlightableCell.State.highlighted_yellow) {
                if (abobus.skill_performing_state.applied_to == null) {
                    abobus.skill_performing_state.applied_to = hex_cell;
                    abobus.skill_performing_state.Enter();
                    Enter();
                }
            }
        }
        // abobus.state = abobus.idle_state;
        gay_manager.ClearAllHighlightedCells();
        // abobus.skill_performing_state.entered = false;
        if (entered) {
            Debug.Log("Unselecting abobus");
            entered = false;
            abobus.transform.position += new Vector3(0, -10, 0);
        }
        
        
    }

    override public void Refresh()
    {
        if (entered) {
            Debug.Log("Unselecting abobus 2");
            abobus.transform.position += new Vector3(0, -10, 0);
        }
        entered = false;
    }

}