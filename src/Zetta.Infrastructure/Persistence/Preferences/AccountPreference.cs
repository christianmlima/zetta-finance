namespace Zetta.Infrastructure.Persistence.Preferences;

public sealed class AccountPreference
{
    private AccountPreference(Guid accountId, Guid userId, string color, string icon)
    {
        AccountId = accountId;
        UserId = userId;
        Color = color;
        Icon = icon;
    }

    private AccountPreference() { }

    public Guid AccountId { get; private set; }
    public Guid UserId { get; private set; }
    public string Color { get; private set; } = "#6366F1";
    public string Icon { get; private set; } = "wallet";

    public static AccountPreference CreateDefault(Guid accountId, Guid userId) =>
        new(accountId, userId, "#6366F1", "wallet");

    public void Update(string color, string icon)
    {
        Color = color;
        Icon = icon;
    }
}
