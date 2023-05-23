using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AbilityComponent : MonoBehaviour
{
    //Set the abilities on the engine
    public AbilityDataHolder[] Abilities;
    public List<AbilityBase> AbilityBases;

    void Awake()
    {
        int i = 0;
        //foreach one of the abilities, add ability base (cooldown etc. logic happens on that component)
        //and set the ability data to that ability base
        foreach (AbilityDataHolder data in Abilities) {
            AbilityBase addedComponent = gameObject.AddComponent(typeof(AbilityBase)) as AbilityBase;
            addedComponent.AbilityData = data;
            addedComponent.AbilityData.OnStart(this.gameObject);
            AbilityBases.Add(addedComponent);
            i += 1;
        }
    }

    //if you need to change the ability, you can use this function
    public void ChangeAbilityGivenIndex(AbilityDataHolder data, int index) {
        if (index > AbilityBases.Count) return;

        AbilityBases[index].ResetInternalState();
        AbilityBases[index].ChangeAbilityData(data);
        AbilityBases[index].AbilityData.OnStart(this.gameObject);
    }

    void Update() {

    }

}
