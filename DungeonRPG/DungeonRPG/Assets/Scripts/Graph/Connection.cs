
public class Connection
{
    Node next;
    bool directed;

    public Connection(Node next, bool directed)
    {
        this.next = next;
        this.directed = directed;
    }

    public Node Next
    {
        get
        {
            return next;
        }
    }

    public bool Directed
    {
        get
        {
            return directed;
        }
    }
    
}
