﻿//------------------------------------------------------------------------------
// <auto-generated>This code was generated by LLBLGen Pro v5.9.</auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;

namespace OrderingApp.EntityClasses
{
	/// <summary>Class which represents the entity 'Order'.</summary>
	public partial class Order : CommonEntityBase
	{
		/// <summary>Method called from the constructor</summary>
		partial void OnCreated();
		private System.Int32 _id = default(System.Int32);

		/// <summary>Initializes a new instance of the <see cref="Order"/> class.</summary>
		public Order() : base()
		{
			this.OrderItems = new List<OrderItem>();
			OnCreated();
		}

		/// <summary>Gets or sets the CustomerId field. </summary>
		public System.Int32 CustomerId { get; set; }
		/// <summary>Gets the Id field. </summary>
		public System.Int32 Id => _id;
		/// <summary>Represents the navigator which is mapped onto the association 'Order.Customer - Customer.Orders (m:1)'</summary>
		public virtual Customer Customer { get; set; } = null!;
		/// <summary>Represents the navigator which is mapped onto the association 'OrderItem.Order - Order.OrderItems (m:1)'</summary>
		public virtual List<OrderItem> OrderItems { get; set; }
	}
}