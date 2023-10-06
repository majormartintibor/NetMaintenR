namespace NetJobR.NetworkInspectionAssignment;

public abstract record NetworkInspectionAssingmentCommand
{
    public record AssignWorker(Guid NetworkInspectionId, Guid WorkerId) : NetworkInspectionAssingmentCommand;

    //These could be used in the future to make it more realistic
    //public record UnassignWorker(Guid NetworkInspectionId) : NetworkInspectionAssingmentCommand;
    //public record SubstituteWorker(Guid NetworkInspectionId, Guid SubstituteWorkerId) : NetworkInspectionAssingmentCommand;
}