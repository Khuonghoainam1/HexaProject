
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace NameCore
{
    public static class ActionEventDispatcherExtension
    {
        public static ActionEventDispatcher RegiterEvent<T>(this MonoBehaviour mono, T eventID, Action<object> eventToAdd)
        {
            if (!typeof(T).IsEnum)
            {
                Debug.Log($"{eventID} mustbe Enum Type ?????");
                return null;
            }
            if (ReferenceEquals(ActionEventDispatcher.Ins, null))
            {
                Debug.LogError($"ActionEventDispatcherExtension Action Event DisPatcher Module is Null???????");
                return null;
            }

            return ActionEventDispatcher.Ins.RegiterEvent(eventID, eventToAdd);
        }

        public static ActionEventDispatcher RemoveEvent<T>(this MonoBehaviour mono, T eventID, Action<object> eventToRemove)
        {
            if (!typeof(T).IsEnum)
            {
                Debug.Log($"{eventID} mustbe Enum Type ?????");
                return null;
            }
            if (ReferenceEquals(ActionEventDispatcher.Ins, null))
            {
                Debug.LogError($"ActionEventDispatcherExtension Action Event DisPatcher Module is Null???????");
                return null;
            }

            return ActionEventDispatcher.Ins.RemoveEvent(eventID, eventToRemove);
        }

        public static ActionEventDispatcher PostEvent<T>(this MonoBehaviour mono, T eventID, object param = null)
        {
            if (!typeof(T).IsEnum)
            {
                Debug.Log($"{eventID} mustbe Enum Type ?????");
                return null;
            }
            return ActionEventDispatcher.Ins.PostEvent(eventID, param);
        }

        public static void ActionEventDispatcherClear(this MonoBehaviour mono)
        {
            ActionEventDispatcher.Ins.Clear();
        }
    }
}

