using System;
using System.Collections.Generic;

namespace ConsoleAppCSharp
{
    enum Signal
    {
        Signal0,
        Signal1,
        Signal2,
        Signal3
    }

    class EventTarget
    {
        public delegate void TDelegate();

        private List<TDelegate> _delegates = new List<TDelegate>();

        public EventTarget()
        {
        }
        public void RegisterEvent(TDelegate delegt)
        {
            _delegates.Add(delegt);
        }
        public void TriggerEvent()
        {
            foreach (TDelegate delegt in _delegates)
            {
                delegt();
            }
        }
    }

    class SignalManager : Singleton<SignalManager>
    {
        private Dictionary<Signal, EventTarget> _eventTable = new Dictionary<Signal, EventTarget>();
        public void Register(Signal signal, EventTarget.TDelegate deleg)
        {
            EventTarget triggers = null;
            if (!_eventTable.ContainsKey(signal))
            {
                triggers = new EventTarget();
                _eventTable.Add(signal, triggers);
            }
            else
            {
                triggers = _eventTable[signal];
            }

            triggers.RegisterEvent(deleg);
        }
        public void Notify(Signal signal)
        {
            EventTarget triggers = _eventTable[signal];
            triggers.TriggerEvent();
        }
    }

    class SignalManagerTest
    {
        public void OnEvent1()
        {
            Console.WriteLine("OnEvent1");
        }
        public void OnEvent2()
        {
            Console.WriteLine("OnEvent2");
        }
        public void OnEvent3()
        {
            Console.WriteLine("OnEvent3");
        }
        public void OnEvent4()
        {
            Console.WriteLine("OnEvent4");
        }

        public void Main()
        {
            SignalManager.CreateInstance();
            SignalManager.Instance.Register(Signal.Signal0, OnEvent1);
            SignalManager.Instance.Register(Signal.Signal0, OnEvent2);
            SignalManager.Instance.Register(Signal.Signal2, OnEvent3);
            SignalManager.Instance.Notify(Signal.Signal0);
            SignalManager.DestroyInstance();
        }
    }
}
