using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RangeOneComponent
{
    private static Vector3[] turns = 
           new Vector3[] {new Vector3(-1,  0,  1)
                        , new Vector3( 0, -1,  1)
                        , new Vector3( 1, -1,  0)
                        , new Vector3( 1,  0, -1)
                        , new Vector3( 0,  1, -1)
                        , new Vector3(-1,  1,  0)};

    public static Vector3[] GetBasisTurns() { 
        return turns; 
    }
}
