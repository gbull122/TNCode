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

        private System.Windows.Controls.Ribbon.Ribbon mainRibbon;

        public System.Windows.Controls.Ribbon.Ribbon MainRibbon
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
                    var ribbonView = newItem as RibbonView;

                    if (ribbonView != null)
                    {
                        RemoveFromParent(ribbonView.ApplicationMenu);
                        mainRibbon.ApplicationMenu = ribbonView.ApplicationMenu;
                        mainRibbon.ApplicationMenu.DataContext = ribbonView.DataContext;

                        RemoveFromParent(ribbonView.HomeTab);
                        mainRibbon.Items.Add(ribbonView.HomeTab);
                        
                        return;
                    }
                    var newTab = newItem as RibbonTab;

                   
                    if (newTab.Header.Equals("Home"))
                    {
                        RibbonGroup group = (RibbonGroup)newTab.Items[0];

                        RemoveFromParent(group);
                        group.DataContext = newTab.DataContext;

                        var homeTab = (RibbonTab)mainRibbon.Items.GetItemAt(0);

                        homeTab.Items.Add(group);

                        return;
                    }

                    mainRibbon.Items.Add(newTab);
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
