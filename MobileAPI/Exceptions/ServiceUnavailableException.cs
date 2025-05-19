namespace MobileAPI.Exceptions;

public class ServiceUnavailableException(string service) : Exception($"Service '{service}' is unavailable");
