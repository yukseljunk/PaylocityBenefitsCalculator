using ErrorOr;

namespace Api.ServiceErrors;

public static class BonusErrors
{
    public static Error InvalidWeek(int weekNo) => Error.Validation("Bonus.InvalidWeekNo", $"Week '{weekNo}' is not valid, should be between 1-26");

}
