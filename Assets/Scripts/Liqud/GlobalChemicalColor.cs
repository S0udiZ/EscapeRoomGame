using UnityEngine;
using System;
using UnityEditor;
using SaintsField;
using System.Collections.Generic;

namespace UnitySimpleLiquid
{

    [CreateAssetMenu(fileName = "GlobalChemicalColor", menuName = "Scriptable Objects/GlobalChemicalColor")]
    public class GlobalChemicalColor : ScriptableSingleton<GlobalChemicalColor>
    {
        [SerializeField]
        SaintsDictionary<string, Color> chemicalColors;

        public Dictionary<string, Color> ChemicalColors
        {
            get
            {
                return chemicalColors;
            }
        }

    }
}