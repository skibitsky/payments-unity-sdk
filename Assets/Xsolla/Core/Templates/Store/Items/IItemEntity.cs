using System.Collections.Generic;

public interface IItemEntity
{
    string GetSku();
    string GetName();
    string GetDescription();
    string GetImageUrl();

    bool IsVirtualCurrency();
    bool IsConsumable();
    
    KeyValuePair<string, uint>? GetVirtualPrice();
    KeyValuePair<string, float>? GetRealPrice();
}