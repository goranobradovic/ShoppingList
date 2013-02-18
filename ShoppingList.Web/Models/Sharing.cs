namespace ShoppingList.Web.Models
{
    public class Sharing
    {
        public long Id { get; set; }

        public virtual ShoppingList ShoppingList { get; set; }

        public string ExternalUserId { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }
}