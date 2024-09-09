using UnityEngine;

namespace Actor
{
    public partial class Character
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var supply = collision.collider.GetComponent<SupplyKit>();
            if (supply != null)
            {
                supply.Gain(this);
            }
        }
    }
}
