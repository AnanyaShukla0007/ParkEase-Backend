namespace Auth.Application.DTOs;

public class ManagerApplicationRequest
{
    public string FullName { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string ProposedLotName { get; set; } = string.Empty;

    public string LotName { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string? Notes { get; set; }

    public string? AnythingElse { get; set; }
}
