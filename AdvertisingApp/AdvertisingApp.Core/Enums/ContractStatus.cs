namespace AdvertisingApp.Core.Enums;

public enum ContractStatus
{
    Created = 0, // Создано
    Active = 1, // Активно
    Cancelled = 2 // Отменено (только если еще не началось)
}