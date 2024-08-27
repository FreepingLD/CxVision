using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace FunctionBlock
{
    public class ViewConfigParamManager
    {
        private static string ParaPath = @"ConfigParam"; // 传感器、光源、控制器等配置参数都可以统一放到这个文件夹内
        private static object sycnObj = new object();
        private static ViewConfigParamManager _Instance;
        public static ViewConfigParamManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        _Instance = new ViewConfigParamManager();
                    }
                }
                return _Instance;
            }
        }

        private BindingList<ViewConfigParam> _ViewParamList = new BindingList<ViewConfigParam>();
        public BindingList<ViewConfigParam> ViewParamList { get => _ViewParamList; set => _ViewParamList = value; }

        public Form DataForm { get; set; }

        //public ViewConfigParam GetViewConfigParam(string camName)
        //{
        //    foreach (var item in _ViewParamList)
        //    {
        //        if (item.CamName == camName)
        //            return item;
        //    }
        //    return null;
        //}

        public void Init(Control marstControl)
        {
            BindingList<ViewConfigParam> emptyList = new BindingList<ViewConfigParam>();
            if (this._ViewParamList == null) return;
            // 先将页面添加完
            foreach (var item in this._ViewParamList) 
            {
                if (item.FormName == "TabPage")
                {
                    ////////////////////////////
                    //TabPage tabPage = new TabPage(item.ViewName.Substring(7));
                    TabPage tabPage = new TabPage(item.Text);
                    tabPage.Name = item.ViewName;
                    tabPage.BackColor = System.Drawing.SystemColors.Control;
                    tabPage.Margin = new Padding(1);
                    tabPage.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    marstControl.Controls.Add(tabPage);
                    marstControl.Controls.SetChildIndex(tabPage, item.ImageNum);
                    /// 重排序 TabPage
                    TabPage[] array = new TabPage[marstControl.Controls.Count];
                    marstControl.Controls.CopyTo(array, 0);
                    marstControl.Controls.Clear();
                    marstControl.Controls.AddRange(array);
                }
            }
             /// 在页面中添加窗体控件 
            foreach (var item in this._ViewParamList)
            {
                if (item == null) continue;
                ControlCollection controlCollection = marstControl.Controls; // 只是获取容器控件第一层所包含的控件，并不能获取嵌套的控件
                switch (item.FormName)
                {
                    case nameof(ViewForm):
                        //if(controlCollection[item.ContainerName] == null)
                        //    emptyList.Add(item);
                        AddForm(controlCollection[item.ContainerName], new ViewForm(item));
                        break;
                    case nameof(ImageViewForm):
                        //if (controlCollection[item.ContainerName] == null)
                        //    emptyList.Add(item);
                        AddForm(controlCollection[item.ContainerName], new ImageViewForm(item));
                        break;
                    case nameof(GraphicViewForm):
                        //if (controlCollection[item.ContainerName] == null)
                        //    emptyList.Add(item);
                        AddForm(controlCollection[item.ContainerName], new GraphicViewForm(item));
                        break;
                    case nameof(DisplayPositionForm):
                        //if (controlCollection[item.ContainerName] == null)
                        //    emptyList.Add(item);
                        AddForm(controlCollection[item.ContainerName], new DisplayPositionForm(item));
                        break;
                    case nameof(JogMotionForm):
                        //if (controlCollection[item.ContainerName] == null)
                        //    emptyList.Add(item);
                        AddForm(controlCollection[item.ContainerName], new JogMotionForm(item));
                        break;
                    case nameof(LightControlForm):
                        //if (controlCollection[item.ContainerName] == null)
                        //    emptyList.Add(item);
                        AddForm(controlCollection[item.ContainerName], new LightControlForm(item));
                        break;
                    case nameof(ElementViewForm):
                        //if (controlCollection[item.ContainerName] == null)
                        //    emptyList.Add(item);
                        AddForm(controlCollection[item.ContainerName], new ElementViewForm(item));
                        break;
                    case nameof(ProgramForm):
                        //if (controlCollection[item.ContainerName] == null)
                        //    emptyList.Add(item);
                        ProgramForm.Instance.ViewConfigParam = item;
                        AddForm(controlCollection[item.ContainerName], ProgramForm.Instance);  //   
                        break;
                    case nameof(DataDisplayForm):
                        //if (controlCollection[item.ContainerName] == null)
                        //    emptyList.Add(item);
                        DataForm = new DataDisplayForm(item);
                        break;
                    case nameof(ReportForm):
                        //if (controlCollection[item.ContainerName] == null)
                        //    emptyList.Add(item);
                        AddForm(controlCollection[item.ContainerName], new ReportForm(item));
                        break;
                    case nameof(RobotJawParaManagerForm):
                        //if (controlCollection[item.ContainerName] == null)
                        //    emptyList.Add(item);
                        AddForm(controlCollection[item.ContainerName], new RobotJawParaManagerForm(item));
                        break;
                    case nameof(FlawDetecteViewForm):
                        //if (controlCollection[item.ContainerName] == null)
                        //    emptyList.Add(item);
                        AddForm(controlCollection[item.ContainerName], new FlawDetecteViewForm(item));
                        break;
                    case nameof(ThicknessViewForm):
                        //if (controlCollection[item.ContainerName] == null)
                        //    emptyList.Add(item);
                        AddForm(controlCollection[item.ContainerName], new ThicknessViewForm(item));
                        break;
                    case nameof(ProgramViewForm):
                        //if (controlCollection[item.ContainerName] == null)
                        //    emptyList.Add(item);
                        AddForm(controlCollection[item.ContainerName], new ProgramViewForm(item));
                        break; //
                    case nameof(MeasureViewForm):
                        //if (controlCollection[item.ContainerName] == null)
                        //    emptyList.Add(item);
                        AddForm(controlCollection[item.ContainerName], new MeasureViewForm(item));
                        break;
                    case nameof(OkNgViewForm):
                        //if (controlCollection[item.ContainerName] == null)
                        //    emptyList.Add(item);
                        AddForm(controlCollection[item.ContainerName], new OkNgViewForm(item));
                        break;
                    case nameof(OperateViewForm):
                        //if (controlCollection[item.ContainerName] == null)
                        //    emptyList.Add(item);
                        AddForm(controlCollection[item.ContainerName], new OperateViewForm(item));
                        break;
                    case nameof(ShowDataForm):
                        //if (controlCollection[item.ContainerName] == null)
                        //    emptyList.Add(item);
                        AddForm(controlCollection[item.ContainerName], new ShowDataForm(item));
                        break;
                    case nameof(AlignDataForm):
                        //if (controlCollection[item.ContainerName] == null)
                        //    emptyList.Add(item);
                        AddForm(controlCollection[item.ContainerName], new AlignDataForm(item));
                        break;
                    case nameof(LoadViewForm):
                        //if (controlCollection[item.ContainerName] == null)
                        //    emptyList.Add(item);
                        AddForm(controlCollection[item.ContainerName], new LoadViewForm(item));
                        break;
                    case nameof(TryPlateParamViewForm):
                        //if (controlCollection[item.ContainerName] == null)
                        //    emptyList.Add(item);
                        AddForm(controlCollection[item.ContainerName], new TryPlateParamViewForm(item));
                        break;
                    case "TabPage":
                        break;
                    default:
                        emptyList.Add(item);
                        break;
                }
            }
            foreach (var item in emptyList)
            {
                this._ViewParamList.Remove(item);
            }          
        }

        public void AddView(Control control, Form form)
        {
            AddForm(control, form);
        }

        public ViewConfigParam GetViewConfigParam(string viewConfigName)
        {
            ViewConfigParam viewConfig = null;
            foreach (var item in this.ViewParamList)
            {
                if (item.ViewName == viewConfigName)
                {
                    viewConfig = item;
                }
            }
            return viewConfig;
        }
        public void AddTabPage(TabControl control, AddViewForm addForm)
        {
            ViewConfigParam viewConfigParam = new ViewConfigParam();
            viewConfigParam.ContainerName = control.Name;
            viewConfigParam.FormName = addForm.FormName; // 表示添加的窗体类型
            viewConfigParam.ViewName = "tabPage" + addForm.ViewName;  // 窗体的名称
            //// 检索是否具有相同名称的视图
            foreach (var item in this.ViewParamList)
            {
                if (item.ViewName == viewConfigParam.ViewName)
                {
                    MessageBox.Show("已包含有相同名称的 TabPage，请使用其他名称!", "添加TabPage");
                    return;
                }
            }
            this.ViewParamList.Add(viewConfigParam);
            /////////////////////////////
            switch (addForm.FormName)
            {
                case nameof(TabPage):
                    TabPage tabPage = new TabPage(addForm.ViewName);
                    viewConfigParam.Text = addForm.ViewName; // 视图的显示名称
                    tabPage.Name = viewConfigParam.ViewName;
                    tabPage.BackColor = System.Drawing.SystemColors.Control;
                    tabPage.Margin = new Padding(1);
                    tabPage.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    control.Controls.Add(tabPage);
                    int curIndex = control.SelectedIndex;
                    control.Controls.SetChildIndex(tabPage, curIndex ); //+ 1
                    viewConfigParam.ImageNum = (curIndex ); //+ 1
                    /// 重排序 TabPage
                    TabPage[] array = new TabPage[control.Controls.Count];
                    control.Controls.CopyTo(array, 0);
                    control.Controls.Clear();
                    control.Controls.AddRange(array);
                    break;
            }
        }
        public void RemoveTabPage(TabControl control, TabPage tabPage)
        {
            foreach (var item in this.ViewParamList)
            {
                if (item.ViewName == tabPage.Name)
                {
                    this.ViewParamList.Remove(item);
                    break;
                }
            }
            if (control.Controls.Contains(tabPage))
            {
                control.Controls.Remove(tabPage);
                /// 重排序 TabPage
                TabPage[] array = new TabPage[control.Controls.Count];
                control.Controls.CopyTo(array, 0);
                control.Controls.Clear();
                control.Controls.AddRange(array);
            }
        }
        public void LeftShiftTabPage(TabControl control, TabPage tabPage)
        {
            ViewConfigParam viewConfigParam = null;
            foreach (var item in this.ViewParamList)
            {
                if (item.ViewName == tabPage.Name)
                {
                    viewConfigParam = item;
                    break;
                }
            }
            if (viewConfigParam == null) return;
            /////////////////////////////
            int curIndex = control.SelectedIndex;
            if (curIndex - 1 < 0)
            {
                control.Controls.SetChildIndex(tabPage, 0);
                viewConfigParam.ImageNum = 0;
            }
            else
            {
                control.Controls.SetChildIndex(tabPage, curIndex - 1);
                viewConfigParam.ImageNum = curIndex - 1;
            }
            /// 重排序 TabPage
            TabPage[] array = new TabPage[control.Controls.Count];
            control.Controls.CopyTo(array, 0);
            control.Controls.Clear();
            control.Controls.AddRange(array);
        }
        public void RightShiftTabPage(TabControl control, TabPage tabPage)
        {
            ViewConfigParam viewConfigParam = null;
            foreach (var item in this.ViewParamList)
            {
                if (item.ViewName == tabPage.Name)
                {
                    viewConfigParam = item;
                    break;
                }
            }
            if (viewConfigParam == null) return;
            /////////////////////////////
            int curIndex = control.SelectedIndex;
            if (curIndex + 1 >= control.Controls.Count)
            {
                control.Controls.SetChildIndex(tabPage, control.Controls.Count - 1);
                viewConfigParam.ImageNum = control.Controls.Count - 1;
            }
            else
            {
                control.Controls.SetChildIndex(tabPage, curIndex + 1);
                viewConfigParam.ImageNum = curIndex + 1;
            }
            /// 重排序 TabPage
            TabPage[] array = new TabPage[control.Controls.Count];
            control.Controls.CopyTo(array, 0);
            control.Controls.Clear();
            control.Controls.AddRange(array);
        }
        public Form AddFormView(TabPage control, AddViewForm addForm)
        {
            Form form = null;
            ViewConfigParam viewConfigParam = new ViewConfigParam();
            viewConfigParam.ContainerName = control.Name;
            viewConfigParam.FormName = addForm.FormName;
            viewConfigParam.ViewName = addForm.ViewName;
            viewConfigParam.Text = addForm.ViewName;
            this.ViewParamList.Add(viewConfigParam);
            /////////////////////////////
            switch (addForm.FormName)
            {
                case nameof(ViewForm):
                    form = new ViewForm(viewConfigParam);
                    break;
                case nameof(ImageViewForm):
                    form = new ImageViewForm(viewConfigParam);
                    break;
                case nameof(DataDisplayForm):
                    form = new DataDisplayForm(viewConfigParam);
                    break;
                case nameof(GraphicViewForm):
                    form = new GraphicViewForm(viewConfigParam);
                    break;
                case nameof(DisplayPositionForm):
                    form = new DisplayPositionForm(viewConfigParam);
                    break;
                case nameof(JogMotionForm):
                    form = new JogMotionForm(viewConfigParam);
                    break;
                case nameof(LightControlForm):
                    form = new LightControlForm(viewConfigParam);
                    break;
                case nameof(ElementViewForm):
                    form = new ElementViewForm(viewConfigParam);
                    break;
                case nameof(ProgramForm):
                    form = ProgramForm.Instance;
                    break;
                case nameof(ReportForm):
                    form = new ReportForm(viewConfigParam);
                    break;
                case nameof(RobotJawParaManagerForm):
                    form = new RobotJawParaManagerForm(viewConfigParam);
                    break;
                case nameof(FlawDetecteViewForm):
                    form = new FlawDetecteViewForm(viewConfigParam);
                    break;
                case nameof(ThicknessViewForm):
                    form = new ThicknessViewForm(viewConfigParam);
                    break;
                case nameof(ProgramViewForm):
                    form = new ProgramViewForm(viewConfigParam);
                    break;
                case nameof(MeasureViewForm):
                    form = new MeasureViewForm(viewConfigParam);
                    break;
                case nameof(OkNgViewForm):
                    form = new OkNgViewForm(viewConfigParam);
                    break;
                case nameof(OperateViewForm):
                    form = new OperateViewForm(viewConfigParam);
                    break;
                case nameof(ShowDataForm):
                    form = new ShowDataForm(viewConfigParam);
                    break;
                case nameof(AlignDataForm):
                    form = new AlignDataForm(viewConfigParam);
                    break;
                case nameof(LoadViewForm):
                    form = new LoadViewForm(viewConfigParam);
                    break;
                case nameof(TryPlateParamViewForm):
                    form = new TryPlateParamViewForm(viewConfigParam);
                    break;
            }
            AddForm(control, form);
            return form;
        }

        public void Clear()
        {
            this._ViewParamList.Clear();
        }

        private void AddForm(Control MastPanel, Form form)
        {
            if (MastPanel == null || form == null) return;
            form.ControlBox = false;
            form.Text = "";
            form.TopLevel = false; // 顶级控制是不能添加的
            form.Padding = new Padding(0);
            form.Dock = DockStyle.None;
            MastPanel.Margin = new Padding(1);
            MastPanel.Controls.Add(form);
            form.Show();
        }
        public bool Save()
        {
            bool IsOk = true;
            if (!DirectoryEx.Exist(ParaPath)) DirectoryEx.Create(ParaPath);
            IsOk = IsOk && XML<BindingList<ViewConfigParam>>.Save(_ViewParamList, ParaPath + "\\" + "ViewConfigParam.xml"); // 以类名作为文件名
            return IsOk;
        }
        public void Read()
        {
            if (File.Exists(ParaPath + "\\" + "ViewConfigParam.xml"))
                this._ViewParamList = XML<BindingList<ViewConfigParam>>.Read(ParaPath + "\\" + "ViewConfigParam.xml");
            else
                this._ViewParamList = new BindingList<ViewConfigParam>();
            if (this._ViewParamList == null)
                this._ViewParamList = new BindingList<ViewConfigParam>();
        }


    }
}
