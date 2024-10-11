using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : Node
{
    private NodeReturnDelegate _nodeFunction;

    // ActionNode needs a function with return type NodeState 
    public ActionNode(NodeReturnDelegate nodeFunction)
    {
        _nodeFunction = nodeFunction;
    }

    public override NodeState Evaluate()
    {
        // Since node function returns a NodeState
        switch (_nodeFunction())
        {
            case NodeState.FAILURE:
                nodeState = NodeState.FAILURE;
                return nodeState;
            case NodeState.SUCCESS:
                nodeState = NodeState.SUCCESS;
                return nodeState;
            case NodeState.RUNNING:
                nodeState = NodeState.RUNNING;
                return nodeState;
        }
        nodeState = NodeState.RUNNING;
        return nodeState;
    }
}
