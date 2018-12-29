using Prism.Regions;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using TNCodeApp.Menu.Views;

namespace TNCodeApp.Menu
{
    public class RibbonBehavior : RegionBehavior
    {
        public static readonly string BehaviorKey = "MenuManagerBehavior";

        private Ribbon mainRibbon;

        public Ribbon MainRibbon
        {
            get { return mainRibbon; }
            set { mainRibbon = value; }
        }

        protected override void OnAttach()
        {
            Region.ActiveViews.CollectionChanged += ActiveViews_CollectionChanged;
            Region.Views.CollectionChanged += Views_CollectionChanged;
        }

        private void Views_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (object newItem in e.NewItems)
                {
                    var ribbonView =  newItem as RibbonView;

                    if (ribbonView!=null)
                    {
                        RemoveFromParent(ribbonView.MainApplicationMenu);
                        mainRibbon.ApplicationMenu = ribbonView.MainApplicationMenu;

                        RemoveFromParent(ribbonView.HomeTab);
                        mainRibbon.Items.Add(ribbonView.HomeTab);
                        return;
                    }
                    var newTab = newItem as RibbonTab;

                    mainRibbon.Items.Add(newTab);
                    //if (newTab.Header.Equals("Home"))
                    //{
                    //    var ribbonGroups = newTab.Items;
                    //    //foreach(FrameworkElement group in ribbonGroups)
                    //    //{
                    //    var thing = (FrameworkElement)ribbonGroups[0];
                    //        RemoveFromParent(thing);
                    //        mainRibbon.Items.Add(thing);
                    //    //}
                    //    return;
                    //}
                    
                }
            }
        }
        public void RemoveFromParent(FrameworkElement item)
        {
            if (item != null)
            {
                var parentItemsControl = (ItemsControl)item.Parent;
                if (parentItemsControl != null)
                {
                    parentItemsControl.Items.Remove(item as UIElement);
                }
            }
        }


        private void ActiveViews_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            
        }
    }
}
