using Prism.Regions;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using TNCodeApp.Menu.Views;

namespace TNCodeApp.Menu
{
    public class MenuBehavior : RegionBehavior
    {
        public static readonly string BehaviorKey = "MenuManagerBehavior";

        private Ribbon menuRibbon;

        public Ribbon MenuRibbon
        {
            get { return menuRibbon; }
            set { menuRibbon = value; }
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
                    var menuView =  newItem as MenuView;

                    if (menuView!=null)
                    {
                        RemoveFromParent(menuView.MainApplicationMenu);
                        menuRibbon.ApplicationMenu = menuView.MainApplicationMenu;

                        RemoveFromParent(menuView.HomeTab);
                        menuRibbon.Items.Add(menuView.HomeTab);
                        return;
                    }
                    //menuRibbon.Items.Add(menuView);
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
