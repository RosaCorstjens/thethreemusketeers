
public class Connection
{
    Node next;
    bool isDirected;

    public Connection(Node next, bool isDirected)
    {
        this.next = next;
        this.isDirected = isDirected;
    }

    public Connection()
    {

    }

    public Node Next
    {
        get
        {
            return next;
        }
    }

    public bool IsDirected
    {
        get
        {
            return isDirected;
        }
    }
    
}
