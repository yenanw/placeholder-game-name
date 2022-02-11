using System;
using System.Collections.Generic;

// Credits to Jason Weimann: https://www.youtube.com/watch?v=V75hgcsCGOM
public class StateMachine
{
    private IState _currentState;

    private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>();
    private List<Transition> _currentTransitions = new List<Transition>();
    private List<Transition> _anyTransitions = new List<Transition>();

    private static readonly List<Transition> s_emptyTransitions = new List<Transition>(0);

    public void Tick()
    {
        var transition = GetTransition();
        if (transition != null)
            SetState(transition.To);

        _currentState.Tick();
    }

    public void SetState(IState state)
    {
        if (state == _currentState)
            return;

        _currentState?.OnExit();
        _currentState = state;

        _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
        if (_currentTransitions == null)
            _currentTransitions = s_emptyTransitions;

        _currentState.OnEnter();
    }

    public void AddTransition(IState from, IState to, Func<bool> pred)
    {
        if (!_transitions.TryGetValue(from.GetType(), out var transitions))
        {
            transitions = new List<Transition>();
            _transitions[from.GetType()] = transitions;
        }

        transitions.Add(new Transition(to, pred));
    }

    public void AddAnyTransition(IState state, Func<bool> pred)
    {
        _anyTransitions.Add(new Transition(state, pred));
    }

    private class Transition
    {
        public IState To { get; }
        public Func<bool> Condition { get; }

        public Transition(IState to, Func<bool> condition)
        {
            this.To = to;
            this.Condition = condition;
        }
    }

    // this method is called each tick, retrieving the first transition
    // that satisfies the condition/predicate, but the order is based
    // on the insertion so perhaps a priority system might be needed later.
    private Transition GetTransition()
    {
        foreach (var transition in _anyTransitions)
            if (transition.Condition())
                return transition;

        foreach (var transition in _currentTransitions)
            if (transition.Condition())
                return transition;

        return null;
    }
}
