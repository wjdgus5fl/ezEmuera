using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MinorShift.Emuera.GameView
{
	/// <summary>
	/// 描画の最小単位
	/// </summary>
	abstract class AConsoleDisplayPart
	{
		public bool Error { get; protected set; }

	    private string _originalStr, _translatedStr;

	    public string Str
	    {
	        get => GlobalStatic.EzTransState ? _translatedStr : _originalStr;
	        protected set
	        {
	            _originalStr = value;
	            _translatedStr = Riey.EzTranslate.Translate(_originalStr);
	        }
	    }

		public string AltText { get; protected set; }
		public int PointX { get; set; }
		public float XsubPixel { get; set; }
		public float WidthF { get; set; }
		public int Width { get; set; }
		public virtual int Top { get { return 0; } }
		public virtual int Bottom { get { return Config.FontSize; } }
		public abstract bool CanDivide { get; }
		
		public abstract void DrawTo(Graphics graph, int pointY, bool isSelecting, bool isBackLog, TextDrawingMode mode);
		public abstract void GDIDrawTo(int pointY, bool isSelecting, bool isBackLog);

		public abstract void SetWidth(StringMeasure sm, float subPixel);
		public override string ToString()
		{
			if (Str == null)
				return "";
			return Str;
		}
	}

	/// <summary>
	/// 色つき
	/// </summary>
	abstract class AConsoleColoredPart : AConsoleDisplayPart
	{
		protected Color Color { get; set; }
		protected Color ButtonColor { get; set; }
		protected bool colorChanged;
	}
}
