using System;
using System.Collections.Generic;

public class StateMachine
{
    private StateNode _current;
    private Dictionary<Type, StateNode> _nodes = new();
    private HashSet<Transition> _anyTransitions = new();

    public void Update()
    {
        var transition = GetTransition();
        if (transition != null)
        {
            ChangeState(transition.To);
        }
        
        _current.State?.Update();
    }

    public void FixedUpdate()
    {
        _current.State?.FixedUpdate();
    }

    public void SetState(IState state)
    {
        _current = _nodes[state.GetType()];
        _current.State?.OnEnter();
    }

    private void ChangeState(IState state)
    {
        if(state == _current.State) return;
        var previousState = _current.State;
        var next = _nodes[state.GetType()];
        
        previousState?.OnExit();
        next?.State?.OnEnter();
        _current = next;
    }

    private ITransition GetTransition()
    {
        foreach (var transition in _anyTransitions)
            if (transition.Condition.Evaluate())
                return transition;
        foreach (var transition in _current.Transitions)
            if (transition.Condition.Evaluate())
                return transition;
        return null;
    }

    public void AddAnyTransition(IState to, IPredicate condition)
    {
        _anyTransitions.Add(new(GetOrAddNode(to).State, condition));
    }
    
    public void AddTransition(IState from, IState to, IPredicate condition)
    {
        GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
    }

    private StateNode GetOrAddNode(IState state)
    {
        var node = _nodes.GetValueOrDefault(state.GetType());

        if (node == null)
        {
            node = new StateNode(state);
            _nodes.Add(state.GetType(), node);
        }

        return node;
    }
    
    private class StateNode
    {
        public IState State { get; }
        public HashSet<ITransition> Transitions { get; } = new();
        
        public StateNode(IState state) {
            State = state;
        }
        
        public void AddTransition(IState to, IPredicate condition)
        {
            Transitions.Add(new Transition(to, condition));
        }
    }

}

