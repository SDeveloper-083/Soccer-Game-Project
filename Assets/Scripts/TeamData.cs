using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "00000000", menuName = "Soccer/Team")]
public class TeamData : ScriptableObject
{
    public string ID;
    public string teamName;
    public string shortName;

    public PlayerData[] lineUp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
