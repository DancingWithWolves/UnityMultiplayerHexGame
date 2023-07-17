using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GayManager : MonoBehaviour
{
    public GameObject hex_grid_go;
    public HexGrid hex_grid;
    private PlayerInput playerInput;
    private TouchControls touchControls;

    public enum Teams {
        blue, 
        yellow
    };

    private Dictionary<Teams, List<GameObject>> abobi;

    public Abobus chosen_abobus;

  
    GameObject SpawnAbobus<T>(Object original, Vector2 hex_coords_vec, Teams team)
    where T:UnityEngine.Component
    {
        if (!typeof(T).IsSubclassOf(typeof(Abobus))) {
            Debug.Log("Trying to spawn abobus from non-abobus-derived component");
            return null;
        }
        var hc = HexCoordinates.FromXZ((int)hex_coords_vec.x, (int)hex_coords_vec.y);
        GameObject abobus_go = Instantiate(original) as GameObject;
        
        abobus_go.AddComponent<T>();
        abobus_go.GetComponent<Abobus>().Init(this, team, hc);

        return abobus_go;
    }
    // Start is called before the first frame update
    void Start()
    {
        chosen_abobus = null;
        abobi = new Dictionary<Teams, List<GameObject>>();
        foreach (Teams team in System.Enum.GetValues(typeof(Teams))) {
            abobi.Add(team, new List<GameObject>());
        }

        hex_grid = hex_grid_go.GetComponent<HexGrid>();
        hex_grid.CreateGrid();
        
        GameObject abobus_go = SpawnAbobus<Slong>(Resources.Load("Abobi/KnightPrefab"), new Vector2(0, 0), Teams.blue);
        abobi[Teams.blue].Add(abobus_go);
        
        abobus_go = SpawnAbobus<Slong>(Resources.Load("Abobi/PawnPrefab"), new Vector2(3, 0), Teams.yellow);
        abobi[Teams.yellow].Add(abobus_go);  

        playerInput = GetComponent<PlayerInput>();

        //touchControls.Player.Click.started += ctx => OnMouseClick(ctx);
        touchControls.Player.Click.performed += ctx => OnMouseClick(ctx);
        touchControls.Player.RightClick.performed += ctx => OnRightMouseClick(ctx);
    }


    void Awake() {
        touchControls = new TouchControls();
    }
    void OnEnable() {
        touchControls.Enable();
    }
    void OnDisable() {
        touchControls.Disable();
    }

    void Update() {
        // HexCoordinates hc = HexCoordinates.ToOffsetCoordinates(x,z);
        // abobus_go.transform.position = HexCoordinates.FromHexCoordinates(hc);  
    }

    public List<Abobus> GetAllAbobi()
    {
        List<Abobus> ans = new List<Abobus>();
        foreach(List<GameObject> abobi_in_team in abobi.Values) {
            foreach(GameObject abobus_go in abobi_in_team) {
                ans.Add(abobus_go.GetComponent<Abobus>());
            }
        }
        return ans;
    }
    
    public void ClearAllHighlightedCells()
    {
        foreach (HexCell hex_cell in hex_grid.GetAllCells()) {
            hex_cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.default_);
        }
    }

    public void OnMouseClick(InputAction.CallbackContext value) {
        Ray inputRay = Camera.main.ScreenPointToRay(touchControls.Player.ClickPosition.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            GameObject hit_go = hit.collider.transform.gameObject;

            Abobus abobus = hit_go.GetComponent<Abobus>();
            if (abobus) {
                if (ReferenceEquals(abobus, chosen_abobus)) {
                    Debug.Log("Meow");
                    chosen_abobus.idle_state.Enter();
                    chosen_abobus = null;
                } else if (chosen_abobus) {
                    chosen_abobus.idle_state.Enter();
                    chosen_abobus = abobus;
                    abobus.chosen_state.Enter();
                } else {
                    chosen_abobus = abobus;
                    abobus.chosen_state.Enter();
                }
                
            }
            
        }
        
        
    }

    public void OnRightMouseClick(InputAction.CallbackContext value) {
        Ray inputRay = Camera.main.ScreenPointToRay(touchControls.Player.ClickPosition.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            GameObject hit_go = hit.collider.transform.gameObject;

            HexCell hex_cell = hit_go.GetComponent<HexCell>();
            if (hex_cell) {
                if (hex_cell.GetComponent<HighlightableCell>().is_highlighted) {
                    chosen_abobus.state.HandleInput(hex_cell);
                    if (chosen_abobus.state == chosen_abobus.idle_state) {
                        chosen_abobus = null;
                    }
                }
            }
        }
    }

}
