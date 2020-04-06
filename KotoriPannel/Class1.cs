#region NameSpaces

using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

#endregion

/*
/// <summary>
/// DarkUI Theme
/// Author : THE LORD
/// Release Date : Saturday, August 12, 2017
/// Update : Saturday, August 14, 2017
/// HF Account : https://hackforums.net/member.php?action=profile&uid=3304362
/// PM Me for any bug.
/// </summary>
*/

#region Helper

public sealed class HelperMethods
{

    #region MouseStates

    /// <summary>
    /// The helper enumerator to get mouse states.
    /// </summary>
    public enum MouseMode : byte
    {
        Normal,
        Hovered,
        Pushed,
        Disabled
    }

    #endregion

    #region Draw Methods

    /// <summary>
    /// The Method to draw the image from encoded base64 string.
    /// </summary>
    /// <param name="G">The Graphics to draw the image.</param>
    /// <param name="Base64Image">The Encoded base64 image.</param>
    /// <param name="Rect">The Rectangle area for the image.</param>
    public void DrawImageFromBase64(Graphics G, string Base64Image, Rectangle Rect)
    {
        Image IM = null;
        using (System.IO.MemoryStream ms = new System.IO.MemoryStream(Convert.FromBase64String(Base64Image)))
        {
            IM = Image.FromStream(ms);
            ms.Close();
        }
        G.DrawImage(IM, Rect);
    }

    /// <summary>
    /// The Method to fill rounded rectangle.
    /// </summary>
    /// <param name="G">The Graphics to draw the image.</param>
    /// <param name="C">The Color to the rectangle area.</param>
    /// <param name="Rect">The Rectangle area to be filled.</param>
    /// <param name="Curve">The Rounding border radius.</param>
    /// <param name="TopLeft">Wether the top left of rectangle be round or not.</param>
    /// <param name="TopRight">Wether the top right of rectangle be round or not.</param>
    /// <param name="BottomLeft">Wether the bottom left of rectangle be round or not.</param>
    /// <param name="BottomRight">Wether the bottom right of rectangle be round or not.</param>
    public void FillRoundedPath(Graphics G, Color C, Rectangle Rect, int Curve, bool TopLeft = true,
        bool TopRight = true, bool BottomLeft = true, bool BottomRight = true)
    {
        G.FillPath(new SolidBrush(C), RoundRec(Rect, Curve, TopLeft, TopRight, BottomLeft, BottomRight));
    }

    /// <summary>
    /// The Method to fill the rounded rectangle.
    /// </summary>
    /// <param name="G">The Graphics to fill the rectangle.</param>
    /// <param name="B">The brush to the rectangle area.</param>
    /// <param name="Rect">The Rectangle area to be filled.</param>
    /// <param name="Curve">The Rounding border radius.</param>
    /// <param name="TopLeft">Wether the top left of rectangle be round or not.</param>
    /// <param name="TopRight">Wether the top right of rectangle be round or not.</param>
    /// <param name="BottomLeft">Wether the bottom left of rectangle be round or not.</param>
    /// <param name="BottomRight">Wether the bottom right of rectangle be round or not.</param>
    public void FillRoundedPath(Graphics G, Brush B, Rectangle Rect, int Curve, bool TopLeft = true,
        bool TopRight = true, bool BottomLeft = true, bool BottomRight = true)
    {
        G.FillPath(B, RoundRec(Rect, Curve, TopLeft, TopRight, BottomLeft, BottomRight));
    }

    /// <summary>
    /// The Method to fill the rectangle the base color and surrounding with another color(Rectangle with shadow).
    /// </summary>
    /// <param name="G">The Graphics to fill the rectangle.</param>
    /// <param name="CenterColor">The Center color of the rectangle area.</param>
    /// <param name="SurroundColor">The Inner Surround color of the rectangle area.</param>
    /// <param name="P">The Point of the surrounding color.</param>
    /// <param name="Rect">The Rectangle area to be filled.</param>
    /// <param name="Curve">The Rounding border radius.</param>
    /// <param name="TopLeft">Wether the top left of rectangle be round or not.</param>
    /// <param name="TopRight">Wether the top right of rectangle be round or not.</param>
    /// <param name="BottomLeft">Wether the bottom left of rectangle be round or not.</param>
    /// <param name="BottomRight">Wether the bottom right of rectangle be round or not.</param>
    public void FillWithInnerRectangle(Graphics G, Color CenterColor, Color SurroundColor, Point P, Rectangle Rect,
        int Curve, bool TopLeft = true, bool TopRight = true, bool BottomLeft = true, bool BottomRight = true)
    {
        using (
            PathGradientBrush PGB =
                new PathGradientBrush(RoundRec(Rect, Curve, TopLeft, TopRight, BottomLeft, BottomRight)))
        {

            PGB.CenterColor = CenterColor;
            PGB.SurroundColors = new Color[] { SurroundColor };
            PGB.FocusScales = P;
            GraphicsPath GP = new GraphicsPath { FillMode = FillMode.Winding };
            GP.AddRectangle(Rect);
            G.FillPath(PGB, GP);
            GP.Dispose();
        }
    }

    /// <summary>
    /// The Method to fill the circle the base color and surrounding with another color(Rectangle with shadow).
    /// </summary>
    /// <param name="G">The Graphics to fill the circle.</param>
    /// <param name="CenterColor">The Center color of the rectangle area.</param>
    /// <param name="SurroundColor">The Inner Surround color of the rectangle area.</param>
    /// <param name="P">The Point of the surrounding color.</param>
    /// <param name="Rect">The circle area to be filled.</param>
    public void FillWithInnerEllipse(Graphics G, Color CenterColor, Color SurroundColor, Point P, Rectangle Rect)
    {
        GraphicsPath GP = new GraphicsPath { FillMode = FillMode.Winding };
        GP.AddEllipse(Rect);
        using (PathGradientBrush PGB = new PathGradientBrush(GP))
        {
            PGB.CenterColor = CenterColor;
            PGB.SurroundColors = new Color[] { SurroundColor };
            PGB.FocusScales = P;
            G.FillPath(PGB, GP);
            GP.Dispose();
        }
    }

    /// <summary>
    /// The Method to fill the rounded rectangle the base color and surrounding with another color(Rectangle with shadow).
    /// </summary>
    /// <param name="G">The Graphics to fill rounded the rectangle.</param>
    /// <param name="CenterColor">The Center color of the rectangle area.</param>
    /// <param name="SurroundColor">The Inner Surround color of the rectangle area.</param>
    /// <param name="P">The Point of the surrounding color.</param>
    /// <param name="Rect">The Rectangle area to be filled.</param>
    /// <param name="Curve">The Rounding border radius.</param>
    /// <param name="TopLeft">Wether the top left of rectangle be round or not.</param>
    /// <param name="TopRight">Wether the top right of rectangle be round or not.</param>
    /// <param name="BottomLeft">Wether the bottom left of rectangle be round or not.</param>
    /// <param name="BottomRight">Wether the bottom right of rectangle be round or not.</param>
    public void FillWithInnerRoundedPath(Graphics G, Color CenterColor, Color SurroundColor, Point P, Rectangle Rect,
        int Curve, bool TopLeft = true, bool TopRight = true, bool BottomLeft = true, bool BottomRight = true)
    {
        using (
            PathGradientBrush PGB =
                new PathGradientBrush(RoundRec(Rect, Curve, TopLeft, TopRight, BottomLeft, BottomRight)))
        {
            PGB.CenterColor = CenterColor;
            PGB.SurroundColors = new Color[] { SurroundColor };
            PGB.FocusScales = P;
            G.FillPath(PGB, RoundRec(Rect, Curve, TopLeft, TopRight, BottomLeft, BottomRight));
        }
    }

    /// <summary>
    /// The Method to draw the rounded rectangle area.
    /// </summary>
    /// <param name="G">The Graphics to draw rounded the rectangle.</param>
    /// <param name="C">Border Color</param>
    /// <param name="Size">Border thickness</param>
    /// <param name="Rect">The Rectangle area to be drawn.</param>
    /// <param name="Curve">The Rounding border radius.</param>
    /// <param name="TopLeft">Wether the top left of rectangle be round or not.</param>
    /// <param name="TopRight">Wether the top right of rectangle be round or not.</param>
    /// <param name="BottomLeft">Wether the bottom left of rectangle be round or not.</param>
    /// <param name="BottomRight">Wether the bottom right of rectangle be round or not.</param>
    public void DrawRoundedPath(Graphics G, Color C, float Size, Rectangle Rect, int Curve, bool TopLeft = true,
        bool TopRight = true, bool BottomLeft = true, bool BottomRight = true)
    {
        G.DrawPath(new Pen(C, Size), RoundRec(Rect, Curve, TopLeft, TopRight, BottomLeft, BottomRight));
    }

    /// <summary>
    /// The method to draw the triangle.
    /// </summary>
    /// <param name="G">The Graphics to draw triangle.</param>
    /// <param name="C">The Triangle Color.</param>
    /// <param name="Size">The Triangle thickness</param>
    /// <param name="P1">Point 1</param>
    /// <param name="P2">Point 2</param>
    /// <param name="P3">Point 3</param>
    /// <param name="P4">Point 4</param>
    /// <param name="P5">Point 5</param>
    /// <param name="P6">Point 6</param>
    public void DrawTriangle(Graphics G, Color C, int Size, Point P1_0, Point P1_1, Point P2_0, Point P2_1, Point P3_0,
        Point P3_1)
    {
        G.DrawLine(new Pen(C, Size), P1_0, P1_1);
        G.DrawLine(new Pen(C, Size), P2_0, P2_1);
        G.DrawLine(new Pen(C, Size), P3_0, P3_1);
    }

    /// <summary>
    /// The Method to fill the rectangle with border.
    /// </summary>
    /// <param name="G">The Graphics to fill the the rectangle.</param>
    /// <param name="Rect">The Rectangle to fill.</param>
    /// <param name="RectColor">The Rectangle color.</param>
    /// <param name="StrokeColor">The Stroke(Border) color.</param>
    /// <param name="StrokeSize">The Stroke thickness.</param>
    public void FillStrokedRectangle(Graphics G, Rectangle Rect, Color RectColor, Color Stroke, int StrokeSize = 1)
    {
        using (SolidBrush B = new SolidBrush(RectColor))
        using (Pen S = new Pen(Stroke, StrokeSize))
        {
            G.FillRectangle(B, Rect);
            G.DrawRectangle(S, Rect);
        }

    }

    /// <summary>
    /// The Method to fill rounded rectangle with border.
    /// </summary>
    /// <param name="G">The Graphics to fill rounded the rectangle.</param>
    /// <param name="Rect">The Rectangle to fill.</param>
    /// <param name="RectColor">The Rectangle color.</param>
    /// <param name="StrokeColor">The Stroke(Border) color.</param>
    /// <param name="StrokeSize">The Stroke thickness.</param>
    /// <param name="Curve">The Rounding border radius.</param>
    /// <param name="TopLeft">Wether the top left of rectangle be round or not.</param>
    /// <param name="TopRight">Wether the top right of rectangle be round or not.</param>
    /// <param name="BottomLeft">Wether the bottom left of rectangle be round or not.</param>
    /// <param name="BottomRight">Wether the bottom right of rectangle be round or not.</param>
    public void FillRoundedStrokedRectangle(Graphics G, Rectangle Rect, Color RectColor, Color Stroke,
        int StrokeSize = 1, int curve = 1, bool TopLeft = true, bool TopRight = true, bool BottomLeft = true,
        bool BottomRight = true)
    {
        using (SolidBrush B = new SolidBrush(RectColor))
        {
            using (Pen S = new Pen(Stroke, StrokeSize))
            {
                FillRoundedPath(G, B, Rect, curve, TopLeft, TopRight, BottomLeft, BottomRight);
                DrawRoundedPath(G, Stroke, StrokeSize, Rect, curve, TopLeft, TopRight, BottomLeft, BottomRight);
            }
        }
    }

    /// <summary>
    /// The Method to draw the image with custom color.
    /// </summary>
    /// <param name="G"> The Graphic to draw the image.</param>
    /// <param name="R"> The Rectangle area of image.</param>
    /// <param name="_Image"> The image that the custom color applies on it.</param>
    /// <param name="C">The Color that be applied to the image.</param>
    /// <remarks></remarks>
    public void DrawImageWithColor(Graphics G, Rectangle R, Image _Image, Color C)
    {
        float[][] ptsArray = new float[][]
        {
            new float[] {Convert.ToSingle(C.R / 255.0), 0f, 0f, 0f, 0f},
            new float[] {0f, Convert.ToSingle(C.G / 255.0), 0f, 0f, 0f},
            new float[] {0f, 0f, Convert.ToSingle(C.B / 255.0), 0f, 0f},
            new float[] {0f, 0f, 0f, Convert.ToSingle(C.A / 255.0), 0f},
            new float[]
            {
                Convert.ToSingle( C.R/255.0),
                Convert.ToSingle( C.G/255.0),
                Convert.ToSingle( C.B/255.0), 0f,
                Convert.ToSingle( C.A/255.0)
            }
        };
        ImageAttributes imgAttribs = new ImageAttributes();
        imgAttribs.SetColorMatrix(new ColorMatrix(ptsArray), ColorMatrixFlag.Default, ColorAdjustType.Default);
        G.DrawImage(_Image, R, 0, 0, _Image.Width, _Image.Height, GraphicsUnit.Pixel, imgAttribs);
    }


    /// <summary>
    /// The Method to draw the image with custom color.
    /// </summary>
    /// <param name="G"> The Graphic to draw the image.</param>
    /// <param name="R"> The Rectangle area of image.</param>
    /// <param name="_Image"> The Encoded base64 image that the custom color applies on it.</param>
    /// <param name="C">The Color that be applied to the image.</param>
    /// <remarks></remarks>
    public void DrawImageWithColor(Graphics G, Rectangle R, string _Image, Color C)
    {
        Image IM = ImageFromBase64(_Image);
        float[][] ptsArray = new float[][]
        {
            new float[] {Convert.ToSingle(C.R / 255.0), 0f, 0f, 0f, 0f},
            new float[] {0f, Convert.ToSingle(C.G / 255.0), 0f, 0f, 0f},
            new float[] {0f, 0f, Convert.ToSingle(C.B / 255.0), 0f, 0f},
            new float[] {0f, 0f, 0f, Convert.ToSingle(C.A / 255.0), 0f},
            new float[]
            {
                Convert.ToSingle( C.R/255.0),
                Convert.ToSingle( C.G/255.0),
                Convert.ToSingle( C.B/255.0), 0f,
                Convert.ToSingle( C.A/255.0)
            }
        };
        ImageAttributes imgAttribs = new ImageAttributes();
        imgAttribs.SetColorMatrix(new ColorMatrix(ptsArray), ColorMatrixFlag.Default, ColorAdjustType.Default);
        G.DrawImage(IM, R, 0, 0, IM.Width, IM.Height, GraphicsUnit.Pixel, imgAttribs);
    }

    #endregion

    #region Shapes

    /// <summary>
    /// The Triangle that joins 3 points to the triangle shape.
    /// </summary>
    /// <param name="P1">Point 1.</param>
    /// <param name="P2">Point 2.</param>
    /// <param name="P3">Point 3.</param>
    /// <returns>The Trangle shape based on given points.</returns>
    public Point[] Triangle(Point P1, Point P2, Point P3)
    {
        return new Point[] {
            P1,
            P2,
            P3
        };
    }

    #endregion

    #region Brushes

    /// <summary>
    /// The Brush with two colors one center another surounding the center based on the given rectangle area. 
    /// </summary>
    /// <param name="CenterColor">The Center color of the rectangle.</param>
    /// <param name="SurroundColor">The Surrounding color of the rectangle.</param>
    /// <param name="P">The Point of surrounding.</param>
    /// <param name="Rect">The Rectangle of the brush.</param>
    /// <returns>The Brush with two colors one center another surounding the center.</returns>
    public static PathGradientBrush GlowBrush(Color CenterColor, Color SurroundColor, Point P, Rectangle Rect)
    {
        GraphicsPath GP = new GraphicsPath { FillMode = FillMode.Winding };
        GP.AddRectangle(Rect);
        return new PathGradientBrush(GP)
        {
            CenterColor = CenterColor,
            SurroundColors = new Color[] { SurroundColor },
            FocusScales = P
        };
    }

    /// <summary>
    /// The Brush from RGBA color.
    /// </summary>
    /// <param name="R">Red.</param>
    /// <param name="G">Green.</param>
    /// <param name="B">Blue.</param>
    /// <param name="A">Alpha.</param>
    /// <returns>The Brush from given RGBA color.</returns>
    public SolidBrush SolidBrushRGBColor(int R, int G, int B, int A = 0)
    {
        return new SolidBrush(Color.FromArgb(A, R, G, B));
    }

    /// <summary>
    /// The Brush from HEX color.
    /// </summary>
    /// <param name="C_WithoutHash">HEX Color without hash.</param>
    /// <returns>The Brush from given HEX color.</returns>
    public SolidBrush SolidBrushHTMlColor(string C_WithoutHash)
    {
        return new SolidBrush(GetHTMLColor(C_WithoutHash));
    }

    #endregion

    #region Pens

    /// <summary>
    /// The Pen from RGBA color.
    /// </summary>
    /// <param name="R">Red.</param>
    /// <param name="G">Green.</param>
    /// <param name="B">Blue.</param>
    /// <param name="A">Alpha.</param>
    /// <returns>The Pen from given RGBA color.</returns>
    public Pen PenRGBColor(int R, int G, int B, int A, float Size)
    {
        return new Pen(Color.FromArgb(A, R, G, B), Size);
    }

    /// <summary>
    /// The Pen from HEX color.
    /// </summary>
    /// <param name="C_WithoutHash">HEX Color without hash.</param>
    /// <param name="Size">The Size of the pen.</param>
    /// <returns></returns>
    public Pen PenHTMlColor(string C_WithoutHash, float Size = 1)
    {
        return new Pen(GetHTMLColor(C_WithoutHash), Size);
    }

    #endregion

    #region Colors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="C_WithoutHash"></param>
    /// <returns></returns>
    public Color GetHTMLColor(string C_WithoutHash)
    {
        return ColorTranslator.FromHtml("#" + C_WithoutHash);
    }

    /// <summary>
    /// The Color from HEX by alpha property.
    /// </summary>
    /// <param name="alpha">Alpha.</param>
    /// <param name="C_WithoutHash">HEX Color without hash.</param>
    /// <returns>The Color from HEX with given ammount of transparency</returns>
    public Color GetAlphaHTMLColor(int alpha, string C_WithoutHash)
    {
        return Color.FromArgb(alpha, ColorTranslator.FromHtml("#" + C_WithoutHash));
    }

    #endregion

    #region Methods

    /// <summary>
    /// The String format to provide the alignment.
    /// </summary>
    /// <param name="Horizontal">Horizontal alignment.</param>
    /// <param name="Vertical">Horizontal alignment. alignment.</param>
    /// <returns>The String format.</returns>
    public StringFormat SetPosition(StringAlignment Horizontal = StringAlignment.Center, StringAlignment Vertical = StringAlignment.Center)
    {
        return new StringFormat
        {
            Alignment = Horizontal,
            LineAlignment = Vertical
        };
    }

    /// <summary>
    /// The Matrix array of single from color.
    /// </summary>
    /// <param name="C">The Color.</param>
    /// <returns>The Matrix array of single from the given color</returns>
    public float[][] ColorToMatrix(Color C)
    {
        return new float[][] {
            new float[] {
                Convert.ToSingle(C.R / 255),
                0,
                0,
                0,
                0
            },
            new float[] {
                0,
                Convert.ToSingle(C.G / 255),
                0,
                0,
                0
            },
            new float[] {
                0,
                0,
                Convert.ToSingle(C.B / 255),
                0,
                0
            },
            new float[] {
                0,
                0,
                0,
                Convert.ToSingle(C.A / 255),
                0
            },
            new float[] {
                Convert.ToSingle(C.R / 255),
                Convert.ToSingle(C.G / 255),
                Convert.ToSingle(C.B / 255),
                0f,
                Convert.ToSingle(C.A / 255)
            }
        };
    }

    /// <summary>
    /// The Image from encoded base64 image.
    /// </summary>
    /// <param name="Base64Image">The Encoded base64 image</param>
    /// <returns>The Image from encoded base64.</returns>
    public Image ImageFromBase64(string Base64Image)
    {
        using (System.IO.MemoryStream ms = new System.IO.MemoryStream(Convert.FromBase64String(Base64Image)))
        {
            return Image.FromStream(ms);
        }
    }


    #endregion

    #region Round Border

    /// <summary>
    /// Credits : AeonHack
    /// </summary>

    public GraphicsPath RoundRec(Rectangle r, int Curve, bool TopLeft = true, bool TopRight = true,
      bool BottomLeft = true, bool BottomRight = true)
    {
        GraphicsPath CreateRoundPath = new GraphicsPath(FillMode.Winding);
        if (TopLeft)
        {
            CreateRoundPath.AddArc(r.X, r.Y, Curve, Curve, 180f, 90f);
        }
        else
        {
            CreateRoundPath.AddLine(r.X, r.Y, r.X, r.Y);
        }
        if (TopRight)
        {
            CreateRoundPath.AddArc(r.Right - Curve, r.Y, Curve, Curve, 270f, 90f);
        }
        else
        {
            CreateRoundPath.AddLine(r.Right - r.Width, r.Y, r.Width, r.Y);
        }
        if (BottomRight)
        {
            CreateRoundPath.AddArc(r.Right - Curve, r.Bottom - Curve, Curve, Curve, 0f, 90f);
        }
        else
        {
            CreateRoundPath.AddLine(r.Right, r.Bottom, r.Right, r.Bottom);

        }
        if (BottomLeft)
        {
            CreateRoundPath.AddArc(r.X, r.Bottom - Curve, Curve, Curve, 90f, 90f);
        }
        else
        {
            CreateRoundPath.AddLine(r.X, r.Bottom, r.X, r.Bottom);
        }
        CreateRoundPath.CloseFigure();
        return CreateRoundPath;
    }

    #endregion

}

#endregion

#region  Form

public class DarkUIForm : ContainerControl
{

    #region Declarations

    private static readonly HelperMethods H = new HelperMethods();
    private bool Movable = false;
    private Point MousePoint = new Point(-1, -1);
    private int MoveHeight = 40;

    #endregion

    #region Constructors

    public DarkUIForm()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
        UpdateStyles();
        DoubleBuffered = true;
        Font = new Font("Helvetica Neue", 9);
    }

    #endregion

    #region Enumerators

    public enum TitlePostion
    {
        Left,
        Center,
        Right
    }

    #endregion

    #region Draw Control

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics G = e.Graphics;
        G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        G.InterpolationMode = InterpolationMode.HighQualityBicubic;
        G.CompositingQuality = CompositingQuality.HighQuality;
        Rectangle r = new Rectangle(0, 0, Width - 1, Height - 1);
        Rectangle rH = new Rectangle(0, 0, Width - 1, 40);
        using (SolidBrush B = new SolidBrush(Color.FromArgb(39, 39, 39)))
        using (LinearGradientBrush BH = new LinearGradientBrush(rH, Color.FromArgb(64, 64, 64), Color.FromArgb(48, 48, 48), 90))
        using (Pen p = new Pen(Color.FromArgb(22, 22, 22)))
        using (Pen p2 = new Pen(Color.FromArgb(15, 255, 255, 255)))
        {
            G.FillRectangle(B, r);
            G.FillRectangle(BH, rH);
            G.DrawLine(p, 0, 40, Width - 2, 40);
            G.DrawLine(p2, 0, 41, Width - 2, 41);
            G.DrawRectangle(p, r);
        }

        if (FindForm().ShowIcon)
        {
            if (FindForm().Icon != null)
            {
                G.DrawIcon(FindForm().Icon, new Rectangle(5, 10, 20, 20));
            }
        }

        G.DrawString(Text, Font, Brushes.White, rH, H.SetPosition());
    }


    #endregion

    #region Events

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Button == System.Windows.Forms.MouseButtons.Left & new Rectangle(0, 0, Width, MoveHeight).Contains(e.Location))
        {
            Movable = true;
            MousePoint = e.Location;
        }
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        Movable = false;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        int x = MousePosition.X;
        int y = MousePosition.Y;
        int x1 = MousePoint.X;
        int y1 = MousePoint.Y;

        if (Movable)
            Parent.Location = new Point(x - x1, y - y1);
        Focus();
    }

    protected override void OnCreateControl()
    {
        base.OnCreateControl();
        ParentForm.FormBorderStyle = FormBorderStyle.None;
        ParentForm.Dock = DockStyle.None;
        Dock = DockStyle.Fill;
        Invalidate();
    }


    #endregion

}

#endregion

#region  CheckBox 

[DefaultEvent("CheckedChanged"), DefaultProperty("Checked")]
public class DarkUICheckBox : Control
{

    #region  Variables 

    private bool _Checked;
    private static readonly HelperMethods H = new HelperMethods();

    #endregion

    #region  Properties 

    [Category("Custom Properties"), Description("Gets or sets a value indicating whether the control is checked.")]
    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            CheckedChanged?.Invoke(this);
            Invalidate();
        }
    }

    #endregion

    #region  Constructors 

    public DarkUICheckBox()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
        DoubleBuffered = true;
        Cursor = Cursors.Hand;
        BackColor = Color.Transparent;
        ForeColor = Color.FromArgb(209, 209, 209);
        Font = new Font("Helvetica Neue", 9);
        UpdateStyles();
    }

    #endregion

    #region  Draw Control 

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics G = e.Graphics;
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        Rectangle rect = new Rectangle(1, 0, 18, 17);
        try
        {
            using (LinearGradientBrush BackBrush = new LinearGradientBrush(rect, Color.FromArgb(48, 48, 48), Color.FromArgb(64, 64, 64), 270))
            using (Pen P = new Pen(Color.FromArgb(22, 22, 22)))
            using (Pen CheckMarkPen = new Pen(Color.FromArgb(189, 189, 189), 2))
            using (Pen P2 = new Pen(Color.FromArgb(30, Color.White)))
            using (SolidBrush TB = new SolidBrush(Color.FromArgb(209, 209, 209)))
            {
                H.FillRoundedPath(G, BackBrush, rect, 2);
                H.DrawRoundedPath(G, P.Color, 1, new Rectangle(0, 0, 18, 17), 2);
                G.DrawLine(P2, 3, 1, 15, 1);
                G.DrawString(Text, Font, TB, new Rectangle(19, 3, Width, Height - 4), H.SetPosition(StringAlignment.Near));
                if (Checked)
                    G.DrawLines(CheckMarkPen, new Point[]
                {
                    new Point(4, 8),
                    new Point(7, 11),
                    new Point(14, 4)
                });
            }
        }

        catch
        {

        }


    }

    #endregion

    #region  Events 

    public event CheckedChangedEventHandler CheckedChanged;
    public delegate void CheckedChangedEventHandler(object sender);

    protected override void OnClick(EventArgs e)
    {
        _Checked = !Checked;
        CheckedChanged?.Invoke(this);
        base.OnClick(e);
        Invalidate();
    }

    protected override void OnTextChanged(System.EventArgs e)
    {
        Invalidate();
        base.OnTextChanged(e);
    }

    protected override void OnResize(System.EventArgs e)
    {
        base.OnResize(e);
        Height = 18;
        Invalidate();
    }

    #endregion

}

#endregion

#region  RadioButton 

[DefaultEvent("CheckedChanged"), DefaultProperty("Checked")]
public class DarkUIRadioButton : Control
{

    #region  Variables 

    private bool _Checked;
    protected int _Group = 1;
    private static readonly HelperMethods H = new HelperMethods();

    #endregion

    #region  Properties 

    [Category("Custom Properties"), Description("Gets or sets a value indicating whether the control is checked.")]
    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            CheckedChanged?.Invoke(this);
            Invalidate();
        }
    }

    [Category("Custom Properties")]
    public int Group
    {
        get { return _Group; }
        set
        {
            _Group = value;
            Invalidate();
        }
    }

    #endregion

    #region  Constructors 

    public DarkUIRadioButton()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
        DoubleBuffered = true;
        UpdateStyles();
        Cursor = Cursors.Hand;
        BackColor = Color.Transparent;
        ForeColor = Color.FromArgb(209, 209, 209);
        Font = new Font("Helvetica Neue", 9);
    }

    #endregion

    #region  Draw Control 

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics G = e.Graphics;
        Rectangle R = new Rectangle(0, 0, 18, 18);
        G.SmoothingMode = SmoothingMode.HighQuality;
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        using (LinearGradientBrush BackBrush = new LinearGradientBrush(R, Color.FromArgb(48, 48, 48), Color.FromArgb(64, 64, 64), 270))
        using (SolidBrush CheckMarkBrush = new SolidBrush(Color.FromArgb(189, 189, 189)))
        using (Pen Border = new Pen(Color.FromArgb(22, 22, 22)))
        using (SolidBrush TB = new SolidBrush(ForeColor))
        using (Pen P2 = new Pen(Color.FromArgb(20, Color.White)))
        {
            G.FillEllipse(BackBrush, R);
            G.DrawEllipse(P2, new Rectangle(1, 1, 16, 16));
            G.DrawEllipse(Border, R);
            G.DrawString(Text, Font, TB, new Rectangle(21, 1, Width, Height - 2), H.SetPosition(StringAlignment.Near));
            if (Checked)
                G.FillEllipse(CheckMarkBrush, new Rectangle(5, 5, 8, 8));
        }



    }

    #endregion

    #region  Events 

    public event CheckedChangedEventHandler CheckedChanged;
    public delegate void CheckedChangedEventHandler(object sender);

    private void UpdateState()
    {
        if (!IsHandleCreated || !Checked)
            return;
        foreach (Control C in Parent.Controls)
        {
            if (!object.ReferenceEquals(C, this) && C is DarkUIRadioButton && ((DarkUIRadioButton)C).Group == _Group)
            {
                ((DarkUIRadioButton)C).Checked = false;
            }
        }
    }

    protected override void OnClick(EventArgs e)
    {
        _Checked = !Checked;
        UpdateState();
        base.OnClick(e);
        Invalidate();
    }

    protected override void OnCreateControl()
    {
        UpdateState();
        base.OnCreateControl();
    }

    protected override void OnTextChanged(System.EventArgs e)
    {
        Invalidate();
        base.OnTextChanged(e);
    }

    protected override void OnResize(System.EventArgs e)
    {
        base.OnResize(e);
        Height = 21;
        Invalidate();
    }

    #endregion

}

#endregion

#region  ComboBox 

[DefaultEvent("SelectedIndexChanged")]
public class DarkUIComboBox : ComboBox
{

    #region  Declarations 

    private static readonly HelperMethods H = new HelperMethods();
    private int _StartIndex = 0;
    public new event SelectedIndexChangedEventHandler SelectedIndexChanged;
    public delegate void SelectedIndexChangedEventHandler(object sender);

    #endregion

    #region  Constructors 

    public DarkUIComboBox()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
        BackColor = Color.Transparent;
        Font = new Font("Helvetica Neue", 10);
        DrawMode = DrawMode.OwnerDrawFixed;
        DoubleBuffered = true;
        StartIndex = 0;
        DropDownStyle = ComboBoxStyle.DropDownList;
        UpdateStyles();
    }

    #endregion

    #region  Properties 

    [Category("Custom Properties"), Description("Gets or sets the index specifying the currently selected item.")]
    private int StartIndex
    {
        get { return _StartIndex; }
        set
        {
            _StartIndex = value;
            try
            {
                base.SelectedIndex = value;
                SelectedIndexChanged?.Invoke(this);
            }
            catch
            {
            }
            Invalidate();
        }
    }

    #endregion

    #region  Draw Control 

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        Graphics G = e.Graphics;
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        try
        {
            using (LinearGradientBrush BG = new LinearGradientBrush(e.Graphics.ClipBounds, Color.FromArgb(48, 48, 48), Color.FromArgb(64, 64, 64), 270))
            using (SolidBrush TC = new SolidBrush((e.State & DrawItemState.Selected) != 0 ? Color.White : Color.FromArgb(189, 189, 189)))
            using (Font F = new Font(Font.Name, Font.Size - 2))
            {
                G.FillRectangle(BG, e.Bounds);
                G.DrawString(GetItemText(Items[e.Index]), F, TC, e.Bounds, H.SetPosition(StringAlignment.Near));
            }
        }
        catch
        {
        }

    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics G = e.Graphics;

        G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        Rectangle Rect = new Rectangle(0, 0, Width - 1, Height - 1);

        using (Pen P2 = new Pen(Color.FromArgb(20, Color.White)))
        using (SolidBrush B = new SolidBrush(Color.FromArgb(31, 31, 31)))
        using (LinearGradientBrush B2 = new LinearGradientBrush(new Rectangle(Width - 29, 0, 30, Height - 1), Color.FromArgb(48, 48, 48), Color.FromArgb(64, 64, 64), 270))
        {
            H.FillRoundedPath(G, B, Rect, 1);
            H.FillRoundedPath(G, B2, new Rectangle(Width - 29, 0, 30, Height - 1), 1, false, true, false);
            G.DrawLine(P2, Width - 28, 1, Width - 28 + Width, 1);
            H.DrawRoundedPath(G, Color.FromArgb(22, 22, 22), 1, Rect, 2);
        }

        G.SmoothingMode = SmoothingMode.AntiAlias;

        H.DrawTriangle(G, Color.FromArgb(192, 192, 192), 2,
            new Point(Width - 20, 10), new Point(Width - 16, 14),
            new Point(Width - 16, 14), new Point(Width - 12, 10),
            new Point(Width - 16, 15), new Point(Width - 16, 14));
        G.SmoothingMode = SmoothingMode.None;
        using (SolidBrush TC = new SolidBrush(Color.FromArgb(168, 168, 168)))
        using (Font F = new Font(Font.Name, Font.Size - 2))
        {
            G.DrawString(Text, F, TC, new Rectangle(7, 1, Width - 1, Height - 1), H.SetPosition(StringAlignment.Near));
        }
    }

    #endregion

}

#endregion

#region TextBox

[DefaultEvent("TextChanged")]
public class DarkUITextBox : Control
{

    #region Declarations

    private TextBox _T = new TextBox();
    private TextBox T
    {
        get { return _T; }
        set
        {
            if (_T != null)
            {
                _T.TextChanged -= T_TextChanged;
                _T.KeyDown -= T_KeyDown;
            }
            _T = value;
            if (_T != null)
            {
                _T.TextChanged += T_TextChanged;
                _T.KeyDown += T_KeyDown;
            }
        }
    }
    private static readonly HelperMethods H = new HelperMethods();
    private HorizontalAlignment _TextAlign = HorizontalAlignment.Left;
    private int _MaxLength = 32767;
    private bool _ReadOnly = false;
    private bool _UseSystemPasswordChar = false;
    private string _WatermarkText = string.Empty;
    private Image _Image;
    private AutoCompleteSource _AutoCompleteSource = AutoCompleteSource.None;
    private AutoCompleteMode _AutoCompleteMode = AutoCompleteMode.None;
    private AutoCompleteStringCollection _AutoCompleteCustomSource;
    private bool _Multiline = false;
    private string[] _Lines = null;

    #endregion

    #region Native Methods

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, string lParam);

    #endregion

    #region Properties

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public BorderStyle BorderStyle
    {
        get
        {
            return BorderStyle.None;
        }
    }

    [Category("Custom Properties"), Description("Gets or sets how text is aligned in TextBox control.")]
    public HorizontalAlignment TextAlign
    {
        get { return _TextAlign; }
        set
        {
            _TextAlign = value;
            if (T != null)
            {
                T.TextAlign = value;
            }
            Invalidate();
        }
    }

    [Category("Custom Properties"), Description("Gets or sets how text is aligned in TextBox control.")]
    public int MaxLength
    {
        get { return _MaxLength; }
        set
        {
            _MaxLength = value;
            if (T != null)
            {
                T.MaxLength = value;
            }
            Invalidate();
        }
    }

    [Browsable(false), ReadOnly(true)]
    public override Color BackColor
    {
        get { return Color.Transparent; }
    }

    [Category("Custom Properties"), Description("Gets or sets a value indicating whether text in the text box is read-only.")]
    public bool ReadOnly
    {
        get { return _ReadOnly; }
        set
        {
            _ReadOnly = value;
            if (T != null)
            {
                T.ReadOnly = value;
            }
        }
    }

    [Category("Custom Properties"), Description("Gets or sets a value indicating whether the text in  TextBox control should appear as the default password character.")]
    public bool UseSystemPasswordChar
    {
        get { return _UseSystemPasswordChar; }
        set
        {
            _UseSystemPasswordChar = value;
            if (T != null)
            {
                T.UseSystemPasswordChar = value;
            }
        }
    }

    [Category("Custom Properties"), Description("Gets or sets a value indicating whether this is a multiline System.Windows.Forms.TextBox control.")]
    public bool Multiline
    {
        get { return _Multiline; }
        set
        {
            _Multiline = value;
            if (T == null)
                return;
            T.Multiline = value;
            if (value)
            {
                T.Height = Height - 10;
            }
            else
            {
                Height = T.Height + 10;
            }
        }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new Image BackgroundImage
    {
        get { return null; }
    }

    [Category("Custom Properties"), Description("Gets or sets the current text in  TextBox.")]
    public override string Text
    {
        get { return base.Text; }
        set
        {
            base.Text = value;
            if (T != null)
            {
                T.Text = value;
            }
        }
    }

    [Category("Custom Properties"), Description("Gets or sets the text in the System.Windows.Forms.TextBox while being empty.")]
    public string WatermarkText
    {
        get { return _WatermarkText; }
        set
        {
            _WatermarkText = value;
            SendMessage(T.Handle, 5377, 0, value);
            Invalidate();
        }
    }

    [Category("Custom Properties"), Description("Gets or sets the image of the control.")]
    public Image Image
    {
        get { return _Image; }
        set
        {
            _Image = value;
            Invalidate();
        }
    }

    [Category("Custom Properties"), Description("Gets or sets a value specifying the source of complete strings used for automatic completion.")]
    public AutoCompleteSource AutoCompleteSource
    {
        get { return _AutoCompleteSource; }
        set
        {
            _AutoCompleteSource = value;
            if (T != null)
            {
                T.AutoCompleteSource = value;
            }
            Invalidate();
        }
    }

    [Category("Custom Properties"), Description("Gets or sets a value specifying the source of complete strings used for automatic completion.")]
    public AutoCompleteStringCollection AutoCompleteCustomSource
    {
        get { return _AutoCompleteCustomSource; }
        set
        {
            _AutoCompleteCustomSource = value;
            if (T != null)
            {
                T.AutoCompleteCustomSource = value;
            }
            Invalidate();
        }
    }

    [Category("Custom Properties"), Description("Gets or sets an option that controls how automatic completion works for the TextBox.")]
    public AutoCompleteMode AutoCompleteMode
    {
        get { return _AutoCompleteMode; }
        set
        {
            _AutoCompleteMode = value;
            if (T != null)
            {
                T.AutoCompleteMode = value;
            }
            Invalidate();
        }
    }

    [Category("Custom Properties"), Description("Gets or sets the font of the text displayed by the control.")]
    public new Font Font
    {
        get { return base.Font; }
        set
        {
            base.Font = value;
            if (T == null)
                return;
            T.Font = value;
            T.Location = new Point(5, 5);
            T.Width = Width - 8;
            if (!Multiline)
                Height = T.Height + 11;
        }
    }

    [Category("Custom Properties"), Description("Gets or sets the lines of text in the control.")]
    public string[] Lines
    {
        get { return _Lines; }
        set
        {
            _Lines = value;
            if (T == null)
                return;
            T.Lines = value;
            Invalidate();
        }
    }

    [Category("Custom Properties"), Description("Gets or sets the ContextMenuStrip associated with this control.")]
    public override ContextMenuStrip ContextMenuStrip
    {
        get { return base.ContextMenuStrip; }
        set
        {
            base.ContextMenuStrip = value;
            if (T == null)
                return;
            T.ContextMenuStrip = value;
            Invalidate();
        }
    }

    #endregion

    #region Constructors

    public DarkUITextBox()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
        DoubleBuffered = true;
        UpdateStyles();
        ForeColor = Color.FromArgb(189, 189, 189);
        Font = new Font("Helvetica World", 10);
        T.Multiline = false;
        T.Cursor = Cursors.IBeam;
        T.BackColor = Color.FromArgb(31, 31, 31);
        T.ForeColor = ForeColor;
        T.BorderStyle = BorderStyle.None;
        T.Location = new Point(7, 4);
        T.Font = Font;
        T.UseSystemPasswordChar = UseSystemPasswordChar;
        Size = new Size(135, 30);
        if (Multiline)
        {
            T.Height = Height - 11;
        }
        else
        {
            Height = T.Height + 11;
        }
    }

    #endregion

    #region Events

    public new event TextChangedEventHandler TextChanged;
    public delegate void TextChangedEventHandler(object sender);

    protected override void OnCreateControl()
    {
        base.OnCreateControl();
        if (!Controls.Contains(T))
            Controls.Add(T);
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        T.Size = new Size(Width - 10, Height - 10);
    }

    #region TextBox MouseEvents

    private void T_TextChanged(object sender, EventArgs e)
    {
        Text = T.Text;
        TextChanged?.Invoke(this);
        Invalidate();
    }

    private void T_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.A)
            e.SuppressKeyPress = true;
        if (e.Control && e.KeyCode == Keys.C)
        {
            T.Copy();
            e.SuppressKeyPress = true;
        }
        Invalidate();
    }

    #endregion

    #endregion

    #region Draw Control

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics G = e.Graphics;
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        Rectangle Rect = new Rectangle(0, 0, Width - 1, Height - 1);

        using (SolidBrush B = new SolidBrush(Color.FromArgb(31, 31, 31)))
        {
            H.FillRoundedPath(G, B, Rect, 2);
            H.DrawRoundedPath(G, Color.FromArgb(22, 22, 22), 1, Rect, 2);
        }

        if (Image != null)
        {
            T.Location = new Point(31, 4);
            T.Width = Width - 60;
            G.InterpolationMode = InterpolationMode.HighQualityBicubic;
            G.DrawImage(Image, new Rectangle(8, 6, 16, 16));
        }
        else
        {
            T.Location = new Point(7, 4);
            T.Width = Width - 10;

        }



    }

    #endregion

}

#endregion

#region  Button 

public class DarkUIButton : Control
{

    #region  Declarations 

    private static readonly HelperMethods H = new HelperMethods();
    private HelperMethods.MouseMode State;
    private int _RoundRadius = 0;

    #endregion

    #region  Constructors 

    public DarkUIButton()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
        DoubleBuffered = true;
        BackColor = Color.Transparent;
        Font = new Font("Helvetica Neue", 10);
        UpdateStyles();
    }

    #endregion

    #region  Properties 

    [Category("Custom Properties"), Description("Gets or sets a value indicating whether the control can Rounded in corners.")]
    public int RoundRadius
    {
        get { return _RoundRadius; }
        set
        {
            _RoundRadius = value;
            Invalidate();
        }
    }

    #endregion

    #region  Draw Control 

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics G = e.Graphics;
        GraphicsPath GP = new GraphicsPath();
        GraphicsPath GP2 = new GraphicsPath();
        Rectangle Rect = new Rectangle(0, 0, Width - 1, Height - 2);
        Rectangle Rect2 = new Rectangle(0, 1, Width - 1, Height - 2);
        if (RoundRadius > 0)
        {
            G.SmoothingMode = SmoothingMode.AntiAlias;
            GP = H.RoundRec(Rect, RoundRadius);
            GP2 = H.RoundRec(Rect2, RoundRadius);
        }
        else
        {
            GP.AddRectangle(Rect);
            GP2.AddRectangle(Rect2);
        }

        GP.CloseFigure();

        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        switch (State)
        {

            case HelperMethods.MouseMode.Normal:
                using (LinearGradientBrush BG = new LinearGradientBrush(Rect, Color.FromArgb(48, 48, 48), Color.FromArgb(64, 64, 64), 270))
                using (Pen P = new Pen(Color.FromArgb(22, 22, 22)))
                using (Pen P2 = new Pen(Color.FromArgb(30, Color.White)))
                using (SolidBrush TB = new SolidBrush(Color.FromArgb(212, 212, 212)))
                {
                    G.FillPath(BG, GP);
                    G.DrawPath(P2, GP2);
                    G.DrawPath(P, GP);

                    G.DrawString(Text, Font, TB, new Rectangle(0, 0, Width, Height), H.SetPosition());
                }

                break;
            case HelperMethods.MouseMode.Hovered:

                Cursor = Cursors.Hand;
                using (LinearGradientBrush BG = new LinearGradientBrush(Rect, Color.FromArgb(29, 29, 29), Color.FromArgb(41, 41, 41), 270))
                using (Pen P = new Pen(Color.FromArgb(22, 22, 22)))
                using (Pen P2 = new Pen(Color.FromArgb(30, Color.White)))
                using (SolidBrush TB = new SolidBrush(Color.FromArgb(212, 212, 212)))
                {
                    G.FillPath(BG, GP);
                    G.DrawPath(P2, GP2);
                    G.DrawPath(P, GP);
                    G.DrawString(Text, Font, TB, new Rectangle(0, 0, Width, Height), H.SetPosition());
                }

                break;
            case HelperMethods.MouseMode.Pushed:

                using (LinearGradientBrush BG = new LinearGradientBrush(Rect, Color.FromArgb(48, 48, 48), Color.FromArgb(64, 64, 64), 270))
                using (Pen P = new Pen(Color.FromArgb(22, 22, 22)))
                using (Pen P2 = new Pen(Color.FromArgb(30, Color.White)))
                using (SolidBrush TB = new SolidBrush(Color.FromArgb(212, 212, 212)))
                {
                    G.FillPath(BG, GP);
                    G.DrawPath(P2, GP2);
                    G.DrawPath(P, GP);
                    G.DrawString(Text, Font, TB, new Rectangle(0, 0, Width, Height), H.SetPosition());
                }

                break;
        }

        GP.Dispose();
    }

    #endregion

    #region  Events 

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        State = HelperMethods.MouseMode.Hovered;
        Invalidate();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        State = HelperMethods.MouseMode.Pushed;
        Invalidate();
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        State = HelperMethods.MouseMode.Hovered;
        Invalidate();
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseEnter(e);
        State = HelperMethods.MouseMode.Normal;
        Invalidate();
    }

    #endregion

}

#endregion

#region  Numerical UPDown 

public class DarkUINumericUpDown : Control
{

    #region  Variables 

    private static readonly HelperMethods H = new HelperMethods();
    private int X = 0;
    private int Y = 0;
    private int _Value = 0;
    private int _Maximum = 100;
    private int _Minimum = 0;

    #endregion

    #region  Properties 

    [Category("Custom Properties"), Description("Gets or sets the current number of the NumericUpDown.")]
    public int Value
    {
        get { return _Value; }
        set
        {
            if (value <= Maximum & value >= Minimum)
                _Value = value;
            if (value > Maximum)
                _Value = Maximum;
            Invalidate();
        }
    }

    [Category("Custom Properties"), Description("Gets or sets the maximum number of the NumericUpDown.")]
    public int Maximum
    {
        get { return _Maximum; }
        set
        {
            if (value > Minimum)
                _Maximum = value;
            if (value > _Maximum)
                value = _Maximum;
            Invalidate();
        }
    }

    [Category("Custom Properties"), Description("Gets or sets the minimum number of the NumericUpDown.")]
    public int Minimum
    {
        get { return _Minimum; }
        set
        {
            if (value < Maximum)
                _Minimum = value;
            if (value < _Minimum)
                value = _Minimum;
            Invalidate();
        }
    }

    #endregion

    #region  Constructors 

    public DarkUINumericUpDown()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
        DoubleBuffered = true;
        UpdateStyles();
        BackColor = Color.Transparent;
        Font = new Font("Helvetica Neue", 10);
    }

    #endregion

    #region  Draw Control 

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics G = e.Graphics;

        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        Rectangle Rect = new Rectangle(0, 0, Width - 1, Height - 1);

        using (SolidBrush B = new SolidBrush(Color.FromArgb(31, 31, 31)))
        using (LinearGradientBrush CR = new LinearGradientBrush(Rect, Color.FromArgb(48, 48, 48), Color.FromArgb(64, 64, 64), 270))
        using (Pen P = new Pen(Color.FromArgb(22, 22, 22)))
        using (Pen P2 = new Pen(Color.FromArgb(20, Color.White)))
        {
            G.FillRectangle(B, Rect);
            G.FillPath(CR, H.RoundRec(new Rectangle(Width - 25, 0, 24, Height - 1), 2));
            G.DrawLine(P, new Point(Width - 25, 1), new Point(Width - 25, Height - 2));
            G.DrawLine(P, new Point(Width - 25, 13), new Point(Width - 1, 13));
            G.DrawLine(P2, Width - 24, 1, Width - 24 + Width, 1);
        }
        G.SmoothingMode = SmoothingMode.AntiAlias;
        using (GraphicsPath AboveWardTriangle = new GraphicsPath())
        using (SolidBrush B = new SolidBrush(Value != Maximum ? Color.FromArgb(192, 192, 192) : Color.FromArgb(22, 22, 22)))
        {
            AboveWardTriangle.AddLine(Width - 17, 9, Width - 2, 9);
            AboveWardTriangle.AddLine(Width - 9, 9, Width - 13, 4);
            AboveWardTriangle.CloseFigure();
            G.FillPath(B, AboveWardTriangle);
        }

        using (GraphicsPath DownWardTriangle = new GraphicsPath())
        using (SolidBrush B = new SolidBrush(Value > Minimum ? Color.FromArgb(192, 192, 192) : Color.FromArgb(22, 22, 22)))
        {
            DownWardTriangle.AddLine(Width - 17, 17, Width - 2, 17);
            DownWardTriangle.AddLine(Width - 9, 17, Width - 13, 22);
            DownWardTriangle.CloseFigure();
            G.FillPath(B, DownWardTriangle);
        }
        G.SmoothingMode = SmoothingMode.Default;
        using (SolidBrush B = new SolidBrush(Color.FromArgb(207, 207, 207)))
        {
            G.DrawString(Value.ToString(), Font, B, new Rectangle(0, 0, Width - 18, Height), H.SetPosition());
        }

        using (Pen P = new Pen(Color.FromArgb(22, 22, 22)))
        {
            G.DrawRectangle(P, Rect);
        }

    }

    #endregion

    #region  Events
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        X = e.Location.X;
        Y = e.Location.Y;
        Invalidate();
        if (e.X < Width - 25)
            Cursor = Cursors.IBeam;
        else
            Cursor = Cursors.Hand;
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        Height = 26;
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        OnMouseClick(e);
        if (X > Width - 17 && X < Width - 3)
        {
            if (Y < 13)
            {
                if ((Value + 1) <= Maximum)
                    Value += 1;
            }
            else
            {
                if ((Value - 1) >= Minimum)
                    Value -= 1;
            }
        }
        Invalidate();
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.Down | keyData == Keys.Next)
        {
            if ((Value - 1) >= Minimum)
                Value -= 1;
            Invalidate();
            return true;
        }
        else if (keyData == Keys.Up | keyData == Keys.Back)
        {
            if ((Value + 1) <= Maximum)
                Value += 1;
            Invalidate();
            return true;
        }
        else
        {
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }

    #endregion

}


#endregion

#region ProgressBar

[DefaultEvent("ValueChanged"), DefaultProperty("Value")]
public class DarkUIProgressBar : Control
{

    #region  Declarations 

    private int _Maximum = 100;
    private int _Value = 0;
    private bool _ShowProgressLines = true;
    public event ValueChangedEventHandler ValueChanged;
    public delegate void ValueChangedEventHandler(object sender);
    private static readonly HelperMethods H = new HelperMethods();
    private bool _ShowProgressValue = true;

    #endregion

    #region  Constructors 


    public DarkUIProgressBar()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
        DoubleBuffered = true;
        BackColor = Color.Transparent;
        UpdateStyles();
        Font = new Font("Helvetica Neue", 9);
    }

    #endregion

    #region  Properties 

    [Category("Custom Properties"), Description("Gets or sets the current position of the progressbar.")]
    public int Value
    {
        get
        {
            if (_Value < 0)
            {
                return 0;
            }
            else
            {
                return _Value;
            }
        }
        set
        {
            if (value > Maximum)
            {
                value = Maximum;
            }
            _Value = value;
            ValueChanged?.Invoke(this);
            Invalidate();
        }
    }

    [Category("Custom Properties"), Description("Gets or sets the maximum value of the progressbar.")]
    public int Maximum
    {
        get { return _Maximum; }
        set
        {
            if (value < _Value)
            {
                _Value = Value;
            }
            _Maximum = value;
            Invalidate();
        }
    }

    [Category("Custom Properties"), Description("Gets or sets the whether the progressbar line be shown or not.")]
    public bool ShowProgressLines
    {
        get { return _ShowProgressLines; }
        set
        {
            _ShowProgressLines = value;
            Invalidate();
        }
    }

    [Category("Custom Properties"), Description("Gets or sets the whether the progressbar value be shown or not.")]
    public bool ShowProgressValue
    {
        get { return _ShowProgressValue; }
        set
        {
            _ShowProgressValue = value;
            Invalidate();
        }
    }

    #endregion

    #region  Draw Control 

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics G = e.Graphics;
        G.TextRenderingHint = TextRenderingHint.AntiAlias;

        GraphicsPath GP = new GraphicsPath();

        int CurrentValue = Convert.ToInt32(Value / (double)Maximum * Width);
        Rectangle Rect = new Rectangle(0, 0, Width - 1, Height - 1);

        using (LinearGradientBrush BG = new LinearGradientBrush(Rect, Color.FromArgb(29, 29, 29), Color.FromArgb(41, 41, 41), 90))
        {
            G.FillPath(BG, H.RoundRec(Rect, 2));
        }

        if (CurrentValue != 0)
        {
            using (LinearGradientBrush PS = new LinearGradientBrush(new Rectangle(Rect.X, Rect.Y, CurrentValue - 1, Rect.Height), Color.FromArgb(48, 48, 48), Color.FromArgb(64, 64, 64), 270))
            using (Pen P2 = new Pen(Color.FromArgb(20, Color.White)))
            {
                G.FillPath(PS, H.RoundRec(new Rectangle(Rect.X, Rect.Y, CurrentValue - 1, Rect.Height), 2));
                using (Pen PSL = new Pen(Color.FromArgb(50, 50, 50), 20))
                {
                    if (ShowProgressLines)
                    {
                        G.SmoothingMode = SmoothingMode.AntiAlias;
                        for (int i = 9; i <= Convert.ToInt32((Width - 20) * ((double)Value / Maximum)); i += 45)
                        {
                            G.DrawLine(PSL, new Point(i, Convert.ToInt32((Height / 2) - Height)), new Point(i - Height, Convert.ToInt32((Height / 2) + Height)));
                        }
                    }
                }
                G.DrawLine(P2, Rect.X, 1, CurrentValue - 2, 1);
            }
        }
        using (SolidBrush B = new SolidBrush(Color.FromArgb(207, 207, 207)))
        {
            if (ShowProgressValue)
            {
                G.DrawString(Value + "%", Font, B, new Rectangle(0, 1, Width, Height), H.SetPosition());
            }
        }

        using (Pen PSL = new Pen(Color.FromArgb(22, 22, 22)))
        {
            G.DrawPath(PSL, H.RoundRec(Rect, 2));
        }
        GP.Dispose();
    }

    #endregion

}

#endregion

#region TabControl

public class DarkUITabControl : TabControl
{

    #region Declarations

    private static readonly HelperMethods H = new HelperMethods();

    #endregion

    #region Constructors

    public DarkUITabControl()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor, true);
        DoubleBuffered = true;
        SizeMode = TabSizeMode.Fixed;
        Dock = DockStyle.None;
        ItemSize = new Size(80, 32);
        Font = new Font("Helvetica Neue", 8);
        Alignment = TabAlignment.Top;
        UpdateStyles();
    }

    #endregion

    #region Draw Control

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics G = e.Graphics;
        G.SmoothingMode = SmoothingMode.AntiAlias;
        G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        G.Clear(Color.FromArgb(39, 39, 39));

        Cursor = Cursors.Hand;

        int Rects = 0;

        for (int i = 0; i <= TabCount - 1; i++)
        {
            Rectangle R = GetTabRect(i);

            if (i == TabCount - 1)
            {
                using (
                    LinearGradientBrush B = new LinearGradientBrush(R, Color.FromArgb(29, 29, 29),
                        Color.FromArgb(41, 41, 41), 270))
                {
                    H.FillRoundedPath(G, B, R, 2, false, true, false);
                }
            }
            else if (i == 0)
            {
                using (
                    LinearGradientBrush B = new LinearGradientBrush(R, Color.FromArgb(29, 29, 29),
                        Color.FromArgb(41, 41, 41), 270))
                {
                    H.FillRoundedPath(G, B, new Rectangle(R.X, R.Y, R.Width, R.Height), 2, true, false, true, false);
                }
            }
            else
            {
                using (
                    LinearGradientBrush B = new LinearGradientBrush(R, Color.FromArgb(29, 29, 29),
                        Color.FromArgb(41, 41, 41), 270))
                {
                    G.FillRectangle(B, R);
                }
            }
        }

        for (int i = 0; i <= TabCount - 1; i++)
        {
            Rectangle R = GetTabRect(i);
            Rects += R.Width;
            if (SelectedIndex == i)
            {
                using (
                    LinearGradientBrush PS = new LinearGradientBrush(R, Color.FromArgb(48, 48, 48),
                        Color.FromArgb(64, 64, 64), 270))
                {
                    if (i == TabCount - 1)
                    {
                        G.FillPath(PS, H.RoundRec(new Rectangle(R.X + 1, R.Y, R.Width - 1, R.Height - 1), 2, false, true, false));
                    }
                    else if (i == 0)
                    {
                        G.FillPath(PS, H.RoundRec(new Rectangle(R.X, R.Y, R.Width, R.Height), 2, true, false, true, false));
                    }
                    else
                    {
                        G.FillRectangle(PS, new Rectangle(R.X + 1, R.Y, R.Width - 1, R.Height));
                    }
                }

                using (SolidBrush B = new SolidBrush(Color.White))
                {
                    G.DrawString(TabPages[i].Text, Font, B, R, H.SetPosition());
                }
            }
            else
            {
                using (SolidBrush B = new SolidBrush(Color.FromArgb(168, 168, 168)))
                {
                    G.DrawString(TabPages[i].Text, Font, B, R, H.SetPosition());
                }

            }
            try
            {
                using (Pen P = new Pen(Color.FromArgb(22, 22, 22)))
                    G.DrawPath(P, H.RoundRec(new Rectangle(GetTabRect(0).X, GetTabRect(0).Y, Rects, GetTabRect(TabCount - 1).Height), 1));
            }
            catch
            {
            }
        }
    }

    #endregion

    #region Events

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        for (int i = 0; i <= TabCount - 1; i++)
        {
            Rectangle R = GetTabRect(i);
            if (R.Contains(e.Location))
            {
                Cursor = Cursors.Hand;
                Invalidate();
            }
            else
            {
                Cursor = Cursors.Arrow;
                Invalidate();
            }
        }
    }

    protected override void OnCreateControl()
    {
        base.OnCreateControl();
        foreach (TabPage Tab in base.TabPages)
        {
            Tab.BackColor = Color.FromArgb(39, 39, 39);
        }
    }

    #endregion

}

#endregion

#region ControlButton

public class DarkUIControlButton : Control
{

    #region Variables

    private HelperMethods.MouseMode State;
    private Style _ControlStyle = Style.Close;

    #endregion

    #region Enumenators

    public enum Style
    {
        Close,
        Minimize,
        Maximize
    }

    #endregion

    #region Constructors

    public DarkUIControlButton()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer |
       ControlStyles.SupportsTransparentBackColor, true);
        DoubleBuffered = true;
        BackColor = Color.Transparent;
        UpdateStyles();
        Anchor = AnchorStyles.Top | AnchorStyles.Right;
        Size = new Size(18, 18);
    }

    #endregion

    #region Draw Control

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics G = e.Graphics;
        {

            G.SmoothingMode = SmoothingMode.HighQuality;

            switch (State)
            {

                case HelperMethods.MouseMode.Normal:
                    using (LinearGradientBrush PS = new LinearGradientBrush(new Rectangle(1, 1, 15, 15), Color.FromArgb(48, 48, 48), Color.FromArgb(64, 64, 64), 270))
                    using (Pen PSL = new Pen(Color.FromArgb(22, 22, 22)))
                    {
                        G.FillEllipse(PS, new Rectangle(1, 1, 15, 15));
                        G.DrawEllipse(PSL, new Rectangle(1, 1, 15, 15));
                    }
                    break;
                case HelperMethods.MouseMode.Hovered:
                    Cursor = Cursors.Hand;
                    using (LinearGradientBrush BG = new LinearGradientBrush(new Rectangle(1, 1, 15, 15), Color.FromArgb(29, 29, 29), Color.FromArgb(41, 41, 41), 90))
                    using (Pen PSL = new Pen(Color.FromArgb(22, 22, 22)))
                    {
                        G.FillEllipse(BG, new Rectangle(1, 1, 15, 15));
                        G.DrawEllipse(PSL, new Rectangle(1, 1, 15, 15));
                    }
                    break;
                case HelperMethods.MouseMode.Pushed:
                    using (LinearGradientBrush PS = new LinearGradientBrush(new Rectangle(1, 1, 15, 15), Color.FromArgb(48, 48, 48), Color.FromArgb(64, 64, 64), 270))
                    using (Pen PSL = new Pen(Color.FromArgb(22, 22, 22)))
                    {
                        G.FillEllipse(PS, new Rectangle(1, 1, 15, 15));
                        G.DrawEllipse(PSL, new Rectangle(1, 1, 15, 15));
                    }
                    break;
            }
        }
    }

    #endregion

    #region Properties

    [Category("Custom"), Description("Gets or sets the type of control button.")]
    public Style ControlStyle
    {
        get
        {
            return _ControlStyle;
        }
        set
        {
            _ControlStyle = value;
            Invalidate();
        }
    }

    #endregion

    #region Events

    protected override void OnClick(EventArgs e)
    {
        base.OnClick(e);
        if (ControlStyle == Style.Close)
        {
            Environment.Exit(0);
            Application.Exit();
        }
        else if (ControlStyle == Style.Minimize)
        {
            if (FindForm().WindowState == FormWindowState.Normal)
            {
                FindForm().WindowState = FormWindowState.Minimized;
            }
        }
        else if (ControlStyle == Style.Maximize)
        {
            if (FindForm().WindowState == FormWindowState.Normal)
            {
                FindForm().WindowState = FormWindowState.Maximized;
            }
            else if (FindForm().WindowState == FormWindowState.Maximized)
            {
                FindForm().WindowState = FormWindowState.Normal;
            }
        }
    }


    protected override void OnMouseEnter(EventArgs e)
    {

        base.OnMouseEnter(e);
        State = HelperMethods.MouseMode.Hovered;
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {

        base.OnMouseUp(e);
        State = HelperMethods.MouseMode.Hovered;
        Invalidate();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {

        base.OnMouseDown(e);
        State = HelperMethods.MouseMode.Pushed;
        Invalidate();
    }

    protected override void OnMouseLeave(EventArgs e)
    {

        base.OnMouseLeave(e);
        State = HelperMethods.MouseMode.Normal;
        Invalidate();
    }


    #endregion
}

#endregion