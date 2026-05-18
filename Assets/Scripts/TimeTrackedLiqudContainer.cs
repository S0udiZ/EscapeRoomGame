using UnityEngine;
using System.Collections.Generic;
using UnitySimpleLiquid;
using UnityEngine.Rendering;
using System.Linq;

public class TimeTrackedLiqudContainer : TimeTrakedObject
{

    //yes lists in lists cry about it
    List<List<LiquidContainer.Chemical>> trackedChems;
    List<float> trackedFillProcent;


    //Special Behavoiur

    LiquidContainer liquidContainer;


    public override void Init()
    {
        liquidContainer = GetComponent<LiquidContainer>();
        if (!liquidContainer)
        {
            Debug.LogError("Missing LiquidContainer for TimeTrackedLiqudContainer");
        }

        trackedChems = new();
        trackedFillProcent = new();
    }

    public override bool IsSameAsLastFrame(int index)
    {
        if (trackedFillProcent[index] != liquidContainer.FillAmountPercent)
        {
            return false;
        }

        //I can already fell my computer screaming
        if (trackedChems[index].Count != liquidContainer.ChemProcents.Count)
        {
            return false;
        }


        //Damn optimised i hope. btw if this causes bugs who cares

        if (trackedChems[index].Count > 0 && liquidContainer.ChemProcents.Count > 0)
        {
            if (trackedChems[index][0].Amount != liquidContainer.ChemProcents[0].Amount)
            {
                return false;
            }
        }

        //for (int i = 0; i < trackedChems[index].Count; i++)
        //{
        //    if (trackedChems[index][i].Amount != liquidContainer.ChemProcents[i].Amount)
        //    {
        //        print($"Failed 2 | {Time.time}");
        //        return false;
        //    }
        //    if (trackedChems[index][i].Type != liquidContainer.ChemProcents[i].Type)
        //    {
        //        print($"Failed 3 | {Time.time}");
        //        return false;
        //    }
        //}



        return true;
    }

    public override void UpdateGameObject(int index)
    {
        print($"Updated | {Time.time}");
        liquidContainer.ChemProcents = trackedChems[index];
        liquidContainer.FillAmountPercent = trackedFillProcent[index];
        liquidContainer.UpdateColor();

    }
    public override void AddToLists()
    {
        print($"Added | {Time.time}");
        trackedChems.Add(liquidContainer.ChemProcents.ToList());
        trackedFillProcent.Add(liquidContainer.FillAmountPercent);
    }
    public override void RemoveOnList(int index)
    {
        trackedChems.RemoveAt(index);
        trackedFillProcent.RemoveAt(index);
    }

    public override void StartRewind()
    {
        base.StartRewind();
        liquidContainer.TimeFrozen = true;

    }
    public override void StopRewind()
    {
        base.StopRewind();
        liquidContainer.TimeFrozen = false;
        liquidContainer.staticBlend = false;
    }

    //Extra Behavoiour
    public override void ExtraBehaivourFixedUpdate()
    {

    }

    public override void ExtraBehaivourTimeRewind()
    {

    }
}
