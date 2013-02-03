using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingList.Web.Models
{

    public class ShoppingList
    {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public string Owner
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the secret url.
        /// </summary>
        /// <value>The secret url.</value>
        public string SecretUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public virtual ICollection<Item> Items
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the bool.
        /// </summary>
        /// <value>The bool.</value>
        public bool Active
        {
            get;
            set;
        }
    }

    /// <summary>
    /// item
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the parent list.
        /// </summary>
        /// <value>The parent list.</value>
        public virtual ShoppingList ShoppingList
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the parent list id.
        /// </summary>
        /// <value>
        /// The parent list id.
        /// </value>
        public int ShoppingList_Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public decimal Amount
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the is bought.
        /// </summary>
        /// <value>The is bought.</value>
        public bool Bought
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        /// <value>The unit.</value>
        public string Unit
        {
            get;
            set;
        }
    }
}