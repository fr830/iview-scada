using System;
using System.ComponentModel; 

namespace Aimirim.iView
{
	// This is a special type converter which will be associated with the Employee class.
	// It converts an Employee object to string representation for use in a property grid.
    internal class iPenConverter : ExpandableObjectConverter
	{
		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType )
		{
			if( destType == typeof(string) && value is iPen )
			{
				// Cast the value to an Employee type
                iPen emp = (iPen)value;

				// Return department and department role separated by comma.
				return emp.ToString();
			}
			return base.ConvertTo(context,culture,value,destType);
		}
	}
}
