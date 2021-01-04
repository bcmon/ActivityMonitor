using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ActivityMonitor.ApplicationMonitor;
using ActMon.Properties;

namespace ActMon.Forms
{

    public partial class FormActivity : Form
    {
        private AppMonitor _appMon;
        private ApplicationView _idleCtl;
        private ComboBox _sortMethod;

        public FormActivity(AppMonitor AppMonitor)
        {
            InitializeComponent();

            _appMon = AppMonitor;

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
            items.Add(new ComboItem() { Key = 2, Text = "Usage" });  // descending

            _sortMethod.DataSource = items;
            _sortMethod.DisplayMember = "Text";
            _sortMethod.ValueMember = "Key";

            flApplicationsUsage.Controls.Add(_sortMethod);

            var sortButton = new Button();
            sortButton.Text = "Refresh";
            sortButton.MouseClick += Sb_MouseClick;
            flApplicationsUsage.Controls.Add(sortButton);

            flApplicationsUsage.SetFlowBreak(sortButton, true);

            _appMon.Applications.Sort("Time");  // initial sort

            ResizeControls();
            updateView();
        }

        private void Sb_MouseClick(object sender, MouseEventArgs e)
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
                addFlControlIfNotExists(lApp);
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