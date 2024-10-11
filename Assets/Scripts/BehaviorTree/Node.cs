using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A node can return either of the following
public enum NodeState
{
    SUCCESS,
    FAILURE,
    RUNNING
}

// Base class for all nodes
public abstract class Node
{
    protected NodeState nodeState;
    // Getter for accessibility
    public NodeState NodeState => nodeState;

    // Define a delegate with return type of Nodestate and NO paramets
    // Function that will handler returning of the node's currentState
    public delegate NodeState NodeReturnDelegate();

    //Constructor
    public Node() { }

    // Function that derived classes should implement
    public abstract NodeState Evaluate();
}
