using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    // A sequence can contain one or more child node
    public List<Node> nodes = new();

    public Sequence(List<Node> nodes)
    {
        this.nodes = nodes;
    }

    public override NodeState Evaluate()
    {
        bool isAnyChildRunning = false;
        foreach (Node node in nodes)
        {
            switch (node.Evaluate())
            {
                //If a child node return a FAILURE, the whole sequence fails
                case NodeState.FAILURE:
                    nodeState = NodeState.FAILURE;
                    return nodeState;
                //If a child node returns SUCCESS, move on
                case NodeState.SUCCESS:
                    continue;
                //If a child is running, wait until it completes
                case NodeState.RUNNING:
                    isAnyChildRunning = true;
                    continue;
                default:
                    nodeState = NodeState.FAILURE;
                    return nodeState;
            }
        }
        // This part of the code will only run
        // When no child node fails
        nodeState = isAnyChildRunning ? NodeState.RUNNING : NodeState.SUCCESS;
        return nodeState;
    }
}
