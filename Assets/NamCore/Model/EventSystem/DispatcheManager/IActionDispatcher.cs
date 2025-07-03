using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NameCore
{
    public interface IActionDispatcher 
    {
        void RegisterEvents();
        void UnregisterEvent();
    }
}
