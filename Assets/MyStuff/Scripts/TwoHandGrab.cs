using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
//[CanSelectMultiple(false)]
public class TwoHandGrab : XRBaseInteractable
{
    
    private void Start()
    {
        XRBaseInteractor interacrot = selectingInteractor;
        IXRSelectInteractor newInteractor = firstInteractorSelecting;
        List<IXRSelectInteractor> moreInteractors = interactorsSelecting;
    }
}
