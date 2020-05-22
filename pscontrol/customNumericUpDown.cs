#region License
/* Copyright(c) 2020 Gerald Elzing (gerald.elzing+pscontrol@gmail.com)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
**/
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pscontrol
{
	public partial class CustomNumericUpDown : NumericUpDown
	{
		private CustomNumericUpDown overflow = null;
		private bool suppressOnValueChanged = false;

		/// <summary>
		/// This sets the value of the control without triggering the OnValueChanged
		/// </summary>
		public decimal ValueNoOnValueChanged
		{
			get
			{
				return Value;
			}
			set
			{
				if (Value != value) suppressOnValueChanged = true;
				Value = value;
			}
		}

		public CustomNumericUpDown()
		{
			InitializeComponent();
		}

		public void SetOverflow(CustomNumericUpDown overflow)
		{
			this.overflow = overflow;
		}

		//returns true if value can be incremented
		public bool AddOne()
		{
			if (Value + 1 <= Maximum)
			{
				Value++;
				return true;
			}
			else
			{
				//value must wrap if we can carry to overflow
				if ((overflow != null) && overflow.AddOne())
				{
					Value = Minimum;
					return true;
				}
				return false;
			}
		}

		//returns true if value can be decremented
		public bool SubOne()
		{
			if (Value - 1 >= Minimum)
			{
				Value--;
				return true;
			}
			else
			{
				//value must wrap if we can borrow from overflow
				if ((overflow != null) && overflow.SubOne())
				{
					Value = Maximum;
					return true;
				}
				return false;
			}
		}

		//override increment (button/key)
		public override void UpButton()
		{
			AddOne();
		}

		//override decrement (button/key)
		public override void DownButton()
		{
			SubOne();
		}

		/* adding leading zero's:
		protected override void ValidateEditText()
		{
			if (base.UserEdit)
			{
				base.ValidateEditText();
			}
		}

		protected override void UpdateEditText()
		{
			Text = Convert.ToInt32(base.Value).ToString("00");
		}*/

		//override windows scroll n lines setting (with scroll 1 'line')
		protected override void OnMouseWheel(MouseEventArgs e_)
		{
			if (e_.Delta > 0) AddOne();
			else SubOne();
			//((HandledMouseEventArgs)e_).Handled = true;//not needed here (because of the override)
		}

		protected override void OnValueChanged(EventArgs e_)
		{
			if (!suppressOnValueChanged)
			{
				base.OnValueChanged(e_);
			}
			suppressOnValueChanged = false;
		}
	}
}
