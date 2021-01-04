using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ActivityMonitor.ApplicationMonitor;
using ActMon.Properties;

namespace ActMon.Forms
{

    public partial class FormActivity : Form
    {
        private string _appPath;
        private AppMonitor _appMon;
        private ApplicationView _idleCtl;
        private ComboBox _sortMethod;
        private CheckBox _chkIgnoreWindowsApps;

        public FormActivity(AppMonitor AppMonitor)
        {
            InitializeComponent();

            _appMon = AppMonitor;
            _appPath = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).AbsolutePath.ToLower();
            _appPath = _appPath.Replace("/", "\\");

            // idle time always at the top
            _idleCtl = new ApplicationView(_appMon);
            _idleCtl.ApplicationText = ResFiles.GlobalRes.caption_IdleTime;
            _idleCtl.Icon = Resources.IdleIcon.ToBitmap();
            flApplicationsUsage.Controls.Add(_idleCtl);
            flApplicationsUsage.SetFlowBreak(_idleCtl, true);

            // sorting
            var sortLabel = new Label();
            sortLabel.Text = "Sort by";
            sortLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            flApplicationsUsage.Controls.Add(sortLabel);

            _sortMethod = new ComboBox();
            List<ComboItem> items = new List<ComboItem>();
            items.Add(new ComboItem() { Key = 0, Text = "Time" }); // descending
            items.Add(new ComboItem() { Key = 1, Text = "Name"  });  // ascending
            items.Add(new ComboItem() { Key = 2, Text = "Events" });  // descending

            _sortMethod.DataSource = items;
            _sortMethod.DisplayMember = "Text";
            _sortMethod.ValueMember = "Key";

            flApplicationsUsage.Controls.Add(_sortMethod);

            _chkIgnoreWindowsApps = new CheckBox();
            _chkIgnoreWindowsApps.Width = 170;
            _chkIgnoreWindowsApps.Text = "Hide Windows/System apps";
            _chkIgnoreWindowsApps.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            _chkIgnoreWindowsApps.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft;
            flApplicationsUsage.Controls.Add(_chkIgnoreWindowsApps);

            flApplicationsUsage.SetFlowBreak(_chkIgnoreWindowsApps, true);
            
            var refreshButton = new Button();
            refreshButton.Text = "Refresh";
            refreshButton.Margin = new Padding(65, 0, 0, 10);
            refreshButton.MouseClick += RefreshButton_MouseClick;
            flApplicationsUsage.Controls.Add(refreshButton);
            flApplicationsUsage.SetFlowBreak(refreshButton, true);

            _appMon.Applications.Sort("Time");  // initial sort

            ResizeControls();
            updateView();
        }

        private void RefreshButton_MouseClick(object sender, MouseEventArgs e)
        {
            RemoveApplicationViewControls();
            _appMon.Applications.Sort(_sortMethod.GetItemText(_sortMethod.SelectedItem)); 
            updateView();
        }

        private void FormActivity_Load(object sender, EventArgs e)
        {

        }

        private void gBoxSessionInfo_Enter(object sender, EventArgs e)
        {

        }

        private void updateView()
        {
            
            foreach (ActivityMonitor.Application.Application lApp in _appMon.Applications)
            {
                if (!_chkIgnoreWindowsApps.Checked)
                {
                    addFlControlIfNotExists(lApp);
                } 
                else if (!lApp.Path.ToLower().Contains("c:\\windows") && lApp.Path.ToLower() != _appPath)
                {
                    addFlControlIfNotExists(lApp);
                }
            }

            int pbvalue;
            double totaltime;

            totaltime = _appMon.TotalTimeSpentInApplications.TotalSeconds + _appMon.Session.IdleTime.TotalSeconds;
            if (totaltime == 0)
                pbvalue = 0;
            else
                pbvalue = (int)(_appMon.Session.IdleTime.TotalSeconds * 100 / totaltime);

            _idleCtl.AppUsageText = _appMon.Session.IdleTime.ToString(@"hh\:mm\:ss") + " (" + pbvalue.ToString() + "%)";

            _idleCtl.PctValue = pbvalue;
        }

        private void addFlControlIfNotExists(ActivityMonitor.Application.Application lApp)
        {
            ApplicationView ctl;

            foreach (var Av in flApplicationsUsage.Controls)
            {
                if (Av.GetType() == typeof(ApplicationView) && ((ApplicationView)Av).Key == lApp.ExeName) return;
            }

            ctl = new ApplicationView(lApp, _appMon);            
            flApplicationsUsage.Controls.Add(ctl);
            flApplicationsUsage.SetFlowBreak(ctl, true);
            ctl.TriggerResize();           

        }

        private void RemoveApplicationViewControls()
        {
            
            for (int i = flApplicationsUsage.Controls.Count - 1; i >= 0; i--) 
            {
                var ctl = flApplicationsUsage.Controls[i];
                if (ctl.GetType() == typeof(ApplicationView) && ((ApplicationView)ctl).ApplicationText != ResFiles.GlobalRes.caption_IdleTime)
                {
                    flApplicationsUsage.Controls.Remove(ctl);
                }
            }

        }

        private void tmrRefresh_Tick(object sender, EventArgs e)
        {
            updateView();
        }

        private void flApplicationsUsage_OnResize(object sender, EventArgs e)
        {
            ResizeControls();
        }

        private void ResizeControls()
        {
            if (flApplicationsUsage.HorizontalScroll.Visible)
            {
                flApplicationsUsage.AutoScroll = false;
                flApplicationsUsage.HorizontalScroll.Visible = false;
                flApplicationsUsage.Width -= (SystemInformation.VerticalScrollBarWidth + 10);
                flApplicationsUsage.AutoScroll = true;
                flApplicationsUsage.Width += (SystemInformation.VerticalScrollBarWidth + 10);
            }
            foreach (var c in flApplicationsUsage.Controls)
            {
                if (c.GetType() == typeof(ApplicationView))
                {
                    ((ApplicationView)c).TriggerResize();
                }
            }
        }

        private class ComboItem
        {
            public int Key { get; set; }
            public string Text { get; set; }
        }
    }
}