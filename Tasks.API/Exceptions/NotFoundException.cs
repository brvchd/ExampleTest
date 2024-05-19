namespace Tasks.API.Exceptions;

public abstract class NotFoundException(string message) : Exception(message);

public class ProjectNotFound(int id) : NotFoundException($"Project with id {id} not found");

public class TeamMemberNotFound(int id) : NotFoundException($"Project with id {id} not found");

public class TaskTypeNotFound(int id) : NotFoundException($"Project with id {id} not found");
