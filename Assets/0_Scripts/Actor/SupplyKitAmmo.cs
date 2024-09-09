namespace Actor
{
    public class SupplyKitAmmo : SupplyKit
    {
        protected override void OnInteractInternal(Character character)
        {
            character.GainAmmo();
        }
    }
}
