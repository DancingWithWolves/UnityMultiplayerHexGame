#define SHOW_GRID_COORDS
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;
public class HexGrid : MonoBehaviour {

	Canvas grid_canvas;

	public Material common_cell_material;
		
	public Material center_cell_material;
	public Material out_of_bounds_cell_material;
	public int width;
	public int height;
    public int cells_array_size;

	public HexCell cell_prefab;
    public TextMeshProUGUI cell_label_prefab;
	
	public Dictionary<HexCoordinates, int> hex_coords_to_index;
    private HexCell[] cells;
    public System.ArraySegment<HexCell> GetAllCells () 
    {
        return new System.ArraySegment<HexCell>(cells, 0, cells_array_size);
    }
	public void CreateGrid (int real_x, int real_y) 
	{
		hex_coords_to_index = new Dictionary<HexCoordinates, int>();
        grid_canvas = GetComponentInChildren<Canvas>();
		cells = new HexCell[height * width];
		int i = 0;

		int offset_horizontal = (width - real_x) / 2;
		int offset_vertical = (height - real_y) / 2;
		for (int z = 0; z < height; z++) {
			if (z % 2 == 0) {
				for (int x = 0; x < width; x++) {
					if ((x < offset_horizontal + 1) 
					 || (x > width - 1 - offset_horizontal)
					 || (z > height - offset_vertical - 1)
					 || (z < offset_vertical)) {
						CreateCell(x, z, i++, out_of_bounds_cell_material);
					} else {
						CreateCell(x, z, i++, common_cell_material);
					}
				}
			} else {
				for (int x = 0; x < width; x++) {
					if ((x < offset_horizontal) 
					 || (x > width - 1 - offset_horizontal)
					 || (z > height - offset_vertical - 1)
					 || (z < offset_vertical)) {
						CreateCell(x, z, i++, out_of_bounds_cell_material);
					} else {
						CreateCell(x, z, i++, common_cell_material);
					}
				}
			}
		}
        cells_array_size = i;
	}
	void CreateCell (int x, int z, int i, Material material = null)
	{
		Vector3 position = HexCoordinates.FromHexCoordinates(HexCoordinates.FromXZ(x, z));
		
		HexCell cell = cells[i] = Instantiate<HexCell>(cell_prefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.hex_coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.state = HexCell.State.empty;
		cell.GetComponent<Renderer>().material = material;
		
		hex_coords_to_index.Add(cell.hex_coordinates, i);
		#if SHOW_GRID_COORDS
		TextMeshProUGUI label = Instantiate<TextMeshProUGUI>(cell_label_prefab);
		label.rectTransform.SetParent(grid_canvas.transform, false);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		label.text = cell.hex_coordinates.ToStringOnSeparateLines();
		#endif
	}

	public HexCell GetCellByHexCoordinates(HexCoordinates hc) 
	{
		return cells[hex_coords_to_index[hc]];
	}

    public void ClearOutOfBoundsCells(List<HexCoordinates> list)
    {
        HexCoordinates[] arr = list.ToArray();
        // Debug.Log(hex_coords_to_index);
        foreach (HexCoordinates hex_coords in arr) {
            if (!hex_coords_to_index.ContainsKey(hex_coords)) {
                list.Remove(hex_coords);
            }
        }
    }

    public bool CheckHexCoordsOutOfBounds(HexCoordinates hex_coords)
    {
        return !hex_coords_to_index.ContainsKey(hex_coords);
    }


	
}