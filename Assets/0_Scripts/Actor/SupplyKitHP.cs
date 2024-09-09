namespace Actor
{
    public class SupplyKitHP : SupplyKit
    {
        protected override void OnInteractInternal(Character character)
        {
            character.HP += 1;
            character.UpdateHP();
        }
    }
}
