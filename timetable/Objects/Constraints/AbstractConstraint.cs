﻿using System;
using System.Xml.Linq;
using Timetabling.DB;
using System.Linq;

namespace Timetabling.Objects
{
	public abstract class AbstractConstraint
	{
		public int weight { get; set; }
		public XElement constraint { get; set; }

		/// <summary>
		/// Sets the weight.
		/// </summary>
		/// <param name="w">The width.</param>
		public void SetWeight(int w)
		{
			weight = w;
			constraint.Add(new XElement("Weight_Percentage", weight));

		}
		/// <summary>
		/// Sets the element.
		/// </summary>
		/// <param name="s">S.</param>
		public void SetElement(string s)
		{
			constraint = new XElement(s);
		}
		/// <summary>
		/// Returns the XElement
		/// </summary>
		/// <returns>The xelement.</returns>
		public abstract XElement ToXelement();
		public abstract XElement[] Create(DataModel dB);
	}
}