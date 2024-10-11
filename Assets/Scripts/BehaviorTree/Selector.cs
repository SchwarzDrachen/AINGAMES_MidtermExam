using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    // A selector can contain one or more child node
    public List<Node> nodes = new();

    public Selector(List<Node> nodes)
    {
        this.nodes = nodes;
    }

    public override NodeState Evaluate()
    {
        foreach (Node node in nodes)
        {
            switch (node.Evaluate())
            {
                //If a child node return a FAILURE, move on
                case NodeState.FAILURE:
                    continue;
                //If a child node returns SUCCESS, selector evaluates as success and ends
                case NodeState.SUCCESS:
                    nodeState = NodeState.SUCCESS;
                    return nodeState;
                //If a child is running, it just runs
                case NodeState.RUNNING:
                    nodeState = NodeState.RUNNING;
                    return nodeState;
                default:
                    continue;
            }
        }
        // This part of the code will only run
        // if all child evaluates as FAILURE
        nodeState = NodeState.FAILURE;
        return nodeState;
    }
}
