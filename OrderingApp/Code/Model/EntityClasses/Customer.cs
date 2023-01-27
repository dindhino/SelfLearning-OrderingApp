﻿//------------------------------------------------------------------------------
// <auto-generated>This code was generated by LLBLGen Pro v5.9.</auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;

namespace OrderingApp.EntityClasses
{
	/// <summary>Class which represents the entity 'Customer'.</summary>
	public partial class Customer : CommonEntityBase
	{
		/// <summary>Method called from the constructor</summary>
		partial void OnCreated();
		private System.Int32 _id = default(System.Int32);

		/// <summary>Initializes a new instance of the <see cref="Customer"/> class.</summary>
		public Customer() : base()
		{
			this.Name = string.Empty;
			OnCreated();
		}

		/// <summary>Gets the Id field. </summary>
		public System.Int32 Id => _id;
		/// <summary>Gets or sets the Name field. </summary>
		public System.String Name { get; set; }
		/// <summary>Gets or sets the PhoneNo field. </summary>
		public System.String? PhoneNo { get; set; }
	}
}
