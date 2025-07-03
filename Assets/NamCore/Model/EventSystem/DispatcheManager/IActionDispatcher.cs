using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NamCore
{
    public interface IActionDispatcher 
    {
        void RegisterEvents();
        void UnregisterEvent();
    }
}
