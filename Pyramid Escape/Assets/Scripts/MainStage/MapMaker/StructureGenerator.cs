using System;
using UnityEngine;

namespace MainStage.MapMaker
{
    public class StructureGenerator : MapModifier
    {
        private Action nextLogic;

        protected void StartStructureGenerate(Action nextAction)
        {
            nextLogic = nextAction;
            StartMapGenerate(StructureGenerate);
        }

        private void StructureGenerate()
        {
            // Todo: Generate Map Structures
            
            
            
            // End of Logic
            nextLogic();
        }
    }
}
