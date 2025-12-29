using Common;
using FunctionBlock;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class DrawParamForm : Form
    {
        private IFunction _function;
        public DrawingBaseMeasure drawObject, drawBaseObject;
        private Form _form;
        public DrawParamForm()
        {
            InitializeComponent();
        }
        public DrawParamForm(DrawingBaseMeasure drawingBaseClass)
        {
            InitializeComponent();
            this.drawObject = drawingBaseClass;
            this._form = null;
        }
        public DrawParamForm(IFunction function, DrawingBaseMeasure drawingClass)
        {
            InitializeComponent();
            this._function = function;
            this.drawObject = drawingClass;
            this._form = null;
        }
        public DrawParamForm(IFunction function, Form form)
        {
            InitializeComponent();
            this._function = function;
            this._form = form;
        }
        private void DrawParamForm_Load(object sender, EventArgs e)
        {
            switch (this.drawObject?.GetType().Name)
            {
                case nameof(userDrawCircleMeasure):

                    break;
                case nameof(userDrawCircleSectorMeasure):

                    break;

                case nameof(userDrawEllipseMeasure):

                    break;
                case nameof(userDrawEllipseSectorMeasure):

                    break;
                case nameof(userDrawLineMeasure):

                    break;
                case nameof(userDrawPointMeasure):

                    break;
                case nameof(userDrawRect2Measure):

                    break;
                case nameof(userDrawWidthMeasure):

                    break;
                case nameof(userDrawCrossMeasure):

                    break;
                case nameof(userDrawPolyLineMeasure):

                    break;
                case nameof(userDrawPolygonMeasure):

                    break;
                case nameof(userDrawManualPointMeasure):

                    break;
                case nameof(userDrawManualPolyLineMeasure):

                    break;
                case nameof(userDrawManualPolygonMeasure):

                    break;

                case nameof(userDrawManualCircleSectorMeasure):
                    this._form = new DrawManualCircleParamForm(this.drawObject);
                    this.AddForm(this.panel1, this._form);
                    break;

                default:

                    break;
            }
        }

        public Form AddForm(Panel MastPanel, Form form)
        {
            if (MastPanel.Controls.Count > 0)
                MastPanel.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            form.Padding = new Padding(0);
            MastPanel.Margin = new Padding(0);
            MastPanel.Controls.Add(form);
            form.Show();
            return form;
        }
        private void DrawParamForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._form?.Close();
        }

        private void 示教button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._form != null)
                {
                    switch (this._form.GetType().Name)
                    {
                        case nameof(PointMeasureForm):
                            ((PointMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(PolyLineMeasureForm):
                            ((PolyLineMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(PolygonMeasureForm):
                            ((PolygonMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(Rect2MeasureForm):
                            ((Rect2MeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(WidthMeasureForm):
                            ((WidthMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(CrossPointMeasureForm):
                            ((CrossPointMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(EllipseMeasureForm):
                            ((EllipseMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(EllipseSectorMeasureForm):
                            ((EllipseSectorMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(CircleMeasureForm):
                            ((CircleMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(CircleSectorMeasureForm):
                            ((CircleSectorMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(LineMeasureForm):
                            ((LineMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(ManualPointMeasureForm):
                            ((ManualPointMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(ManualCircleSectorMeasureForm):
                            ((ManualCircleSectorMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(ManualPolyLineMeasureForm):
                            ((ManualPolyLineMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                        case nameof(ManualPolygonMeasureForm):
                            ((ManualPolygonMeasureForm)this._form).示教点位button_Click(null, null);
                            break;
                    }
                }
                else
                    this.drawObject?.AttachDrawingObjectToWindow();
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
                //new UserMessageForm().ShowDialog(ex.ToString());
            }
        }

        private void 确认button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._function == null) return;
                switch (this._function.GetType().Name)
                {
                    case nameof(CircleMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawCircleMeasure)this.drawObject).GetPixCircleParam());
                        else
                            ((CircleMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(CircleSectorMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawCircleSectorMeasure)this.drawObject).GetPixCircleSectorParam());
                        else
                            ((CircleSectorMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(EllipseMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawEllipseMeasure)this.drawObject).GetPixEllipseParam());
                        else
                            ((EllipseMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(EllipseSectorMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawEllipseSectorMeasure)this.drawObject).GetPixEllipseSectorParam());
                        else
                            ((EllipseSectorMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(LineMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawLineMeasure)this.drawObject).GetPixLineParam());
                        else
                            ((LineMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(PointMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawPointMeasure)this.drawObject).GetPixLineParam());
                        else
                            ((PointMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(Rectangle2Measure):
                        if (this._form == null)
                            this._function.Execute(((userDrawRect2Measure)this.drawObject).GetPixRectangle2Param());
                        else
                            ((Rect2MeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(WidthMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawWidthMeasure)this.drawObject).GetPixRectangle2Param());
                        else
                            ((WidthMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(CrossPointMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawCrossMeasure)this.drawObject).GetPixLineParam());
                        else
                            ((CrossPointMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(PolyLineMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawPolyLineMeasure)this.drawObject).GetPixPolyLineParam());
                        else
                            ((PolyLineMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(PolygonMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawPolygonMeasure)this.drawObject).GetPixPolygonParam());
                        else
                            ((PolygonMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(ManualPointMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawManualPointMeasure)this.drawObject).GetPixLineParam());
                        else
                            ((ManualPointMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(ManualCircleSectorMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawManualCircleSectorMeasure)this.drawObject).GetPixCircleSectorParam());
                        else
                            ((ManualCircleSectorMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(ManualPolyLineMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawManualPolyLineMeasure)this.drawObject).GetPixPolyLineParam());
                        else
                            ((ManualPolyLineMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    case nameof(ManualPolygonMeasure):
                        if (this._form == null)
                            this._function.Execute(((userDrawManualPolygonMeasure)this.drawObject).GetPixPolygonParam());
                        else
                            ((ManualPolygonMeasureForm)this._form).运行toolStrip_ItemClicked(this._form, new ToolStripItemClickedEventArgs(new ToolStripButton("运行", null, null, "toolStripButton_Run")));
                        break;
                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
                //new UserMessageForm().ShowDialog(ex.ToString());
            }
        }




    }
}
