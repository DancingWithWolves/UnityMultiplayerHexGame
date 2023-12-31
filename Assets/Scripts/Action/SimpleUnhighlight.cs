using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleUnhighlight : IAction
{
    private List<HexCell> to_unhighligt;
    public SimpleUnhighlight (HexCell applied_to, List<HexCell> to_unhighligt)
    {
        this.applied_to = applied_to;
        this.to_unhighligt = to_unhighligt;
    }
    private HexCell applied_to;
    private GameManager game_manager;
    
    public HexCell AppliedTo { 
        get => applied_to; 
        set {
            applied_to = value;
        } 
    }

    public string DebugMessage()
    {
        return "Simply unhighlight " + to_unhighligt;
    }

    public void Invoke()
    {
        Debug.Log("Simply unhighlight");
        foreach (HexCell cell in to_unhighligt) {
            cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.default_);
        }
    }
}
