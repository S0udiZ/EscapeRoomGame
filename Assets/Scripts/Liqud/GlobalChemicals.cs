using System;
using UnityEditor;
using UnityEngine;
namespace UnitySimpleLiquid
{
    [CreateAssetMenu(fileName = "GlobalChemicals", menuName = "Scriptable Objects/GlobalChemicals")]
    public class GlobalChemicalReactions : ScriptableObject
    {

        [Serializable]
        public struct ChemcialReaction
        {
            [SerializeField]
            public string ChemicalA;
            [SerializeField]
            public string ChemicalB;
        }
        [Serializable]
        public struct ReactionSchema
        {
            [SerializeField]
            public ChemcialReaction Reaction;
            [SerializeField]
            public ChemcialReaction Result;

            [SerializeField]
            [Tooltip("Reaction rate in [L/s]")]
            public float ReactionRate;
        }

        [SerializeField]
        public ReactionSchema[] reactionSchema;

    }
}