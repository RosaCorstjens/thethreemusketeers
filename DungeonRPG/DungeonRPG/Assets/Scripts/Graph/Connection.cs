
public class Connection
{
    Node next;
    bool isLiniar;

    public Connection(Node next, bool isLiniar)
    {
        this.next = next;
        this.isLiniar = isLiniar;
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

    public bool IsLiniar
    {
        get
        {
            return isLiniar;
        }
    }
    
}
