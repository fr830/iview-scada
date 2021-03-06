﻿
namespace Aimirim.iView
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using System.Xml;
	using System.Collections.Generic;
	using System.Drawing.Imaging;
	using System.Drawing.Design;

	[ToolboxItem(false)]
	[ToolboxBitmapAttribute(typeof(iPanel))]
	public class iPanel : Panel, ISupportInitialize
	{
		#region FIELDS
		private iPanelAnimotion _animotion;

		private event EventHandler click;
		private event EventHandler doubleclick;
		#endregion

		#region PROPERTIES
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;		// Get the default values from the base class
				int style = cp.Style;					// Store the Style and ExStyle values
				int exStyle = cp.ExStyle;
				cp.Style = style;
				cp.ExStyle = exStyle;					// Store the results
				
				if (BackColor == Color.Transparent)
				{
					//cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT
					cp.ExStyle = cp.ExStyle | 32;

					SetStyle(ControlStyles.Opaque, true);
					SetStyle(ControlStyles.OptimizedDoubleBuffer , false);
					//SetStyle(ControlStyles.UserPaint, false);
					//SetStyle(ControlStyles.AllPaintingInWmPaint, false);
					return cp;
				}
				else
				{
					SetStyle(ControlStyles.Opaque, false);
					SetStyle(ControlStyles.OptimizedDoubleBuffer , true);
					//SetStyle(ControlStyles.UserPaint, true);
					//SetStyle(ControlStyles.AllPaintingInWmPaint, true);
					return base.CreateParams;
				}
			}
		}
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
				RecreateHandle();
			}
		}
		
		[CategoryAttribute("Layout")]
		[DescriptionAttribute("Specifies whether a control will automatically size itself to fit its contents.")]
		[Browsable(true)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
				Refresh();
			}
		}
		
		[CategoryAttribute("Behavior")]
		[DescriptionAttribute("iView SCADA properties")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public iPanelAnimotion Animotion
		{
			get
			{
				if (_animotion == null)
				{
					_animotion = new iPanelAnimotion();
				}
				return _animotion;
			}
			set
			{
				_animotion = value;
			}
		}
		#endregion

		#region CONSTRUCTORS
		public iPanel()
		{
		}
		#endregion

		#region METHODS
		protected override void OnClick(EventArgs e)
		{
			if (this.click != null)
			{
				this.click(this, e);
			}
		}
		public new event EventHandler Click
		{
			add
			{
				click += value;
			}
			remove
			{
				click -= value;
			}
		}
		protected override void OnDoubleClick(EventArgs e)
		{
			if (this.doubleclick != null)
			{
				this.doubleclick(this,e);
			}
		}
		public new event EventHandler DoubleClick
		{
			add
			{
				doubleclick += value;
			}
			remove
			{
				doubleclick -= value;
			}
		}
		protected override void OnMouseEnter(EventArgs e)
		{
			if ((click != null && click.GetInvocationList().Length > 0) || (doubleclick != null && doubleclick.GetInvocationList().Length > 0) )
			{
				this.Cursor = Cursors.Hand;
			}
			base.OnMouseEnter(e);
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			if (this.Cursor == Cursors.Hand)
			{
				this.Cursor = Cursors.Default;
			}
			base.OnMouseLeave(e);
		}
		
		protected override void OnPaint(PaintEventArgs e)
		{
			if (BackgroundImage != null && AutoSize)
			{
				Height = Height < BackgroundImage.Height ? BackgroundImage.Height : Height;
				Width = Width < BackgroundImage.Width ? BackgroundImage.Width : Width;
			}

			if (DesignMode && BackColor == Color.Transparent)
			{
				if (BackgroundImage != null)
				{
					Bitmap bmp = new Bitmap(BackgroundImage);
					// Make the bottom left pixel the transparent color
					if (Animotion.Image.Enable)
					{
						bmp.MakeTransparent(Animotion.Image.TransparentColor);
					}
					e.Graphics.DrawImage(bmp, e.ClipRectangle);
				}
			}
			else
			{
				base.OnPaint(e);
			}
		}
		private Rectangle xy_projection(Rectangle rect, int heightB, int widthB)
		{
			int x = 0;
			int y = 0;
			int height = 0;
			int width = 0;

			int heightP = this.Height;
			int widthP = this.Width;
			
			double xRatio = (double)widthB / (double)widthP;
			double yRatio = (double)heightB / (double)heightP;
			int[] xy = new int[2];


			if (BackgroundImageLayout == ImageLayout.Stretch)
			{
				x = rect.X;
				y = rect.Y;
				height = rect.Height;
				width = rect.Width;
			}
			else if (BackgroundImageLayout == ImageLayout.Center)
			{
				int borderHeight = (heightP - heightB) / 2;
				int borderWidth = (widthP - widthB) / 2;
				x = borderWidth;
				y = borderHeight;
				height = heightB;
				width = widthB;
			}
			else if (BackgroundImageLayout == ImageLayout.Zoom)
			{
			}
			else
			{
				x = rect.X;
				y = rect.Y;
				height = heightB;
				width = widthB;
			}

			return new Rectangle(x, y, width, height);
		}

		public void BeginInit()
		{
		}
		public void EndInit()
		{
			if (!DesignMode)
			{
				Animotion.Attach(this);
			}
		}
		#endregion
		
		#region EVENTS
		#endregion
	}
}
