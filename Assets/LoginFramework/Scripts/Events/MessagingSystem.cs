/***************************** Module Header *****************************\
Module Name:  MessagingSystem.cs
Project:      Vironit Unity3D Login Framework
Copyright (c) VironIT, http://https://vironit.com

Messaging System to generate and handle events

The MIT License (MIT)

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VitonIT.LoginFramework
{
    public delegate void MessageHandlerDelegate(BaseEvent message);

    public class MessagingSystem : SingletonAsComponent<MessagingSystem>
    {

        public static MessagingSystem Instance
        {
            get { return ((MessagingSystem)_Instance); }
            set { _Instance = value; }
        }

        private Dictionary<string, List<MessageHandlerDelegate>> _listenerDict = new Dictionary<string, List<MessageHandlerDelegate>>();
        private Queue<BaseEvent> _messageQueue = new Queue<BaseEvent>();
        private float maxQueueProcessingTime = 0.16667f;

        public bool AddEventListener(System.Type type, MessageHandlerDelegate handler)
        {
            if (type == null)
            {
                Debug.Log("MessagingSystem: AttachListener failed due to no message type specified");
                return false;
            }

            string msgName = type.Name;

            if (!_listenerDict.ContainsKey(msgName))
            {
                _listenerDict.Add(msgName, new List<MessageHandlerDelegate>());
            }

            List<MessageHandlerDelegate> listenerList = _listenerDict[msgName];
            if (listenerList.Contains(handler))
            {
                return false; // listener already in list
            }

            listenerList.Add(handler);
            return true;
        }

        public bool DispatchEvent(BaseEvent msg)
        {
            if (!_listenerDict.ContainsKey(msg.name))
            {
                return false;
            }
            _messageQueue.Enqueue(msg);
            return true;
        }

        void Update()
        {
            float timer = 0.0f;
            while (_messageQueue.Count > 0)
            {
                if (maxQueueProcessingTime > 0.0f)
                {
                    if (timer > maxQueueProcessingTime)
                        return;
                }

                BaseEvent msg = _messageQueue.Dequeue();
                if (!TriggerMessage(msg))
                    Debug.Log("Error when processing message: " + msg.name);

                if (maxQueueProcessingTime > 0.0f)
                    timer += Time.deltaTime;
            }
        }

        private bool TriggerMessage(BaseEvent msg)
        {
            string msgName = msg.name;
            if (!_listenerDict.ContainsKey(msgName))
            {
                Debug.Log("MessagingSystem: Message \"" + msgName + "\" has no listeners!");
                return false; // no listeners for messae so ignore it
            }

            List<MessageHandlerDelegate> listenerList = _listenerDict[msgName];

            for (int i = 0; i < listenerList.Count; ++i)
            {
                listenerList[i](msg);

                //return true; // message consumed.
            }

            return true;
        }

        public bool RemoveEventListener(System.Type type, MessageHandlerDelegate handler)
        {
            if (type == null)
            {
                Debug.Log("MessagingSystem: DetachListener failed due to no message type specified");
                return false;
            }

            string msgName = type.Name;

            if (!_listenerDict.ContainsKey(type.Name))
            {
                return false;
            }

            List<MessageHandlerDelegate> listenerList = _listenerDict[msgName];
            if (!listenerList.Contains(handler))
            {
                return false;
            }

            listenerList.Remove(handler);
            return true;
        }

    }
}