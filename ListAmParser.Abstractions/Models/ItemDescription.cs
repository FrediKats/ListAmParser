namespace ListAmParser.Abstractions.Models;

public record ItemDescription(
    int ItemId,
    string AuthorName,
    DateOnly LastUpdateTime,
    string Title,
    string Address,
    int RoomCount,
    bool? HasDishWasher,
    ItemPrice? Price,
    string UserProfileLink);