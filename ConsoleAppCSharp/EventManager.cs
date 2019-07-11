using System;
using System.Collections.Generic;

namespace ConsoleAppCSharp
{
    class Event1 { }
    class Event2 { }

    interface IEventTrigger
    {
        void TriggerEvent(object evnt);
    }

    class EventTrigger<T> : IEventTrigger
    {
        public delegate void TDelegate(T evnt);

        private List<TDelegate> _delegates = new List<TDelegate>();

        public EventTrigger()
        {
        }
        public void RegisterEvent(TDelegate delegt)
        {
            _delegates.Add(delegt);
        }
        public void TriggerEvent(object evnt)
        {
            DoTriggerEvent((T)evnt);
        }

        private void DoTriggerEvent(T evnt)
        {
            foreach (TDelegate delegt in _delegates)
            {
                delegt(evnt);
            }
        }
    }

    class EventManager : Singleton<EventManager>
    {
        private Dictionary<string, IEventTrigger> _eventTable = new Dictionary<string, IEventTrigger>();
        public void Register<T>(EventTrigger<T>.TDelegate delegt) where T : class, new()
        {
            string key = typeof(T).Name;

            EventTrigger<T> triggers = null;
            if (!_eventTable.ContainsKey(key))
            {
                triggers = new EventTrigger<T>();
                _eventTable.Add(key, triggers);
            }
            else
            {
                triggers = (EventTrigger<T>)_eventTable[key];
            }

            triggers.RegisterEvent(delegt);
        }
        public void Notify<T>(T evnt) where T : class
        {
            IEventTrigger triggers = _eventTable[typeof(T).Name];
            triggers.TriggerEvent(evnt);
        }
    }

    class EventManagerTest
    {
        public void OnEvent1(Event1 ev)
        {
            Console.WriteLine("OnEvent");
        }
        public void OnEvent2(Event1 ev)
        {
            Console.WriteLine("OnEvent");
        }
        public void OnEvent3(Event2 ev)
        {
            Console.WriteLine("OnEvent");
        }
        public void OnEvent4(Event2 ev)
        {
            Console.WriteLine("OnEvent");
        }

        public void Main()
        {
            EventManager.CreateInstance();
            EventManager.Instance.Register<Event1>(OnEvent1);
            EventManager.Instance.Register<Event1>(OnEvent2);
            EventManager.Instance.Register<Event2>(OnEvent3);
            EventManager.Instance.Register<Event2>(OnEvent4);
            EventManager.Instance.Notify(new Event1());
            EventManager.DestroyInstance();
        }
    }
}
