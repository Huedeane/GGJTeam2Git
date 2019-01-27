using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_ObjectType { NPC, Item , Other}

public interface IComponentObject{

    E_ObjectType ObjectType { get; set; }
    NPC NPCObject { get; set; }
    Item ItemObject { get; set; }
    

}
