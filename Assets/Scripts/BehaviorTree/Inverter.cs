using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : Node
{
    private Node node;

    public Inverter(Node node)
    {
        this.node = node;
    }

    public override NodeState Evaluate()
    {
        // Inverter returns the opposite
        switch (node.Evaluate())
        {
            case NodeState.FAILURE:
                nodeState = NodeState.SUCCESS;
                return nodeState;
            case NodeState.SUCCESS:
                nodeState = NodeState.FAILURE;
                return nodeState;
            case NodeState.RUNNING:
                nodeState = NodeState.RUNNING;
                return nodeState;
        }
        nodeState = NodeState.FAILURE;
        return nodeState;
    }
}
