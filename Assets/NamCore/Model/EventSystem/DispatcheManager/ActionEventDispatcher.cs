
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

namespace NameCore
{
    public class ActionEventDispatcher : Singleton<ActionEventDispatcher>
    {
        private readonly Dictionary<string, Action<object>> m_poolEvents = new Dictionary<string, Action<object>>();
        private event Action<object> m_eventHolder;


        public ActionEventDispatcher RegiterEvent<T>(T eventID, Action<object> eventToAdd)
        {
            if (!typeof(T).IsEnum)
            {
                Debug.Log($"{eventID} mustbe Enum Type ?????");
                return null;
            }
            if (m_poolEvents.TryGetValue(eventID.ToString(), out m_eventHolder))
            {
                m_eventHolder += eventToAdd;
                m_poolEvents[eventID.ToString()] = m_eventHolder;
            }
            else
            {
                m_eventHolder += eventToAdd;
                m_poolEvents.Add(eventID.ToString(), eventToAdd);
            }
            Debug.Log($"[ActionEventDispatcher] Regiter new event with id : {eventID}");
            return this;
        }

        public ActionEventDispatcher RemoveEvent<T>(T eventID, Action<object> eventToRemove)
        {
            if (!typeof(T).IsEnum)
            {
                Debug.Log($"{eventID} mustbe Enum Type ?????");
                return null;
            }
            if (m_poolEvents.TryGetValue(eventID.ToString(), out m_eventHolder))
            {
                m_eventHolder -= eventToRemove;
                m_poolEvents[eventID.ToString()] = m_eventHolder;
                Debug.Log($"[ActionEventDispatcher] Regiter new event with id : {eventID}");
            }
            else
            {
                Debug.LogWarning($"ActionEventDistpatcher Not Found The Event With ID {eventID}");
            }
            return this;
        }


        public ActionEventDispatcher PostEvent<T>(T eventID, object param)
        {
            if (!typeof(T).IsEnum)
            {
                Debug.Log($"{eventID} mustbe Enum Type ?????");
                return null;
            }
            if (!m_poolEvents.TryGetValue(eventID.ToString(), out
                m_eventHolder)) return this;
            if (ReferenceEquals(m_eventHolder, null))
            {
                m_poolEvents.Remove(eventID.ToString());
                return this;

            }
            m_eventHolder(param);
            Debug.Log($"ActionEventDistpatcher post the event with ID {eventID}");
            return this;
        }
        public void Clear()
        {
            m_poolEvents.Clear();
        }
    }
}

