using System;
using System.Collections.ObjectModel;
using System.Linq;
using ActivityMonitor.Collections;

namespace ActivityMonitor.Application
{
    public class Applications : ObservableCollection<Application>
    {
        public bool Contains(string application)
        {
            return (from app in this
                    where app.Name == application
                    select app).FirstOrDefault() != null;
        }

        public bool Contains(string application, string path)
        {
            return (from app in this
                    where app.Name == application && app.Path == path
                    select app).FirstOrDefault() != null;
        }

        public void Sort(string sortMethod = null)
        {
            System.Collections.Generic.List<Application> sorted = null;
            if (sortMethod == "Name")
            {
                sorted = this.OrderBy(x => x.Name).ThenByDescending(x => x.TotalTimeInMinutes).ToList();
                
            }
            else if (sortMethod == "Usage")
            {
                sorted = this.OrderByDescending(x => x.Usage.Count).ThenBy(x => x.Name).ToList();
            }
            else  // time, default, or anything else
            {
                sorted = this.OrderByDescending(x => x.TotalTimeInMinutes).ToList();               
            }

            // sort in place. see NeilW's answer to https://stackoverflow.com/questions/1945461/how-do-i-sort-an-observable-collection
            for (int i = 0; i < sorted.Count(); i++)
            {
                this.Move(this.IndexOf(sorted[i]), i);
            }

        }

        public IApplication this[string applicationName]
        {
            get
            {
                return (from app in this
                        where app.Name == applicationName
                        select app).FirstOrDefault();


            }

        }

        public TimeSpan TotalTime
        {
            get
            {
                return (from app in this
                        select app.TotalUsageTime).Aggregate(TimeSpan.Zero, (subtotal,
                                                                             t) => subtotal.Add(t));
            }
        }

        public void Refresh()
        {

            foreach (var application in this)
            {
                application.Refresh();
            }

            
        }
        
    }
}