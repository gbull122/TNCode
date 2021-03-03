using Fluent;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TnCode.TnCodeApp.Ribbon
{
    public class RibbonBehavoir : RegionBehavior
    {
        public static readonly string BehaviorKey = "RibbonBehavior";
        public const double DefaultMergeOrder = 10000d;
        private const string HOME_TAB = "Home";

        public Fluent.Ribbon MainRibbon { get; set; }

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
                    var view = newItem as UserControl;
                    var ribbonFromView = GetRibbonFromView(view);

                    MergeRibbon(newItem, ribbonFromView);
                }
            }
        }

        private void ActiveViews_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }

        public Fluent.Ribbon GetRibbonFromView(UserControl thing)
        {
            var foundControl = thing.FindName("MainRibbon") as Fluent.Ribbon;
            return foundControl;
        }

        protected void MergeRibbon(object sourceView, Fluent.Ribbon moduleRibbon)
        {
            //MergeApplicationMenu(sourceView, moduleRibbon, ribbon);
            //MergeQuickAccessToolbar(sourceView, moduleRibbon, ribbon);
            //MergeContextualTabGroups(sourceView, moduleRibbon, ribbon);
            MergeTabs(sourceView, moduleRibbon);
            
            //var ribbonTabs = ribbon.Tabs.Cast<UIElement>().ToArray();
            //var moduleTabs = moduleRibbon.Tabs.Cast<UIElement>().ToArray();

            //ItemsControl ribbontemsControl = new ItemsControl();
            //ribbontemsControl.ItemsSource = ribbonTabs;

            //ItemsControl moduleItemsControl = new ItemsControl();
            //moduleItemsControl.ItemsSource=(moduleTabs);

            //MergeItemsControl(sourceView, moduleItemsControl, ribbontemsControl);
        }

        protected void MergeTabs(object sourceView, Fluent.Ribbon moduleRibbon)
        {
            var tabs = moduleRibbon.Tabs;
            foreach (var tab in tabs)
            {
                if (tab.Header.Equals(HOME_TAB))
                {
                    var groups = tab.Groups;
                    var homeTab = GetHomeTab();

                    foreach (var group in groups)
                    {
                        DisconnectFromParent(sourceView, group);
                        try
                        {
                            homeTab.Groups.Add(group);
                        }
                        catch (Exception w)
                        {
                            var e = w.Message;
                        }

                    }
                }
                else
                {
                    DisconnectFromParent(sourceView, tab);
                    MainRibbon.Tabs.Add(tab);
                }
            }
            //if (moduleRibbon.QuickAccessToolBar != null)
            //{
            //    if (ribbon.QuickAccessToolBar == null)
            //        ribbon.QuickAccessToolBar = new RibbonQuickAccessToolBar();
            //    MergeItemsControl(sourceView, moduleRibbon.QuickAccessToolBar, ribbon.QuickAccessToolBar);
            //}
        }

        

        protected Fluent.RibbonTabItem GetHomeTab()
        {
            var tabs = MainRibbon.Tabs;
            foreach (var tab in tabs)
            {
                if (tab.Header.Equals(HOME_TAB))
                    return tab;
            }
            return null;
        }
        protected void MergeQuickAccessToolbar(object sourceView, Fluent.Ribbon moduleRibbon, Fluent.Ribbon ribbon)
        {
            //if (moduleRibbon.QuickAccessToolBar != null)
            //{
            //    if (ribbon.QuickAccessToolBar == null)
            //        ribbon.QuickAccessToolBar = new RibbonQuickAccessToolBar();
            //    MergeItemsControl(sourceView, moduleRibbon.QuickAccessToolBar, ribbon.QuickAccessToolBar);
            //}
        }

        protected void MergeApplicationMenu(object sourceView, Fluent.Ribbon moduleRibbon, Fluent.Ribbon ribbon)
        {
            //if (moduleRibbon.ApplicationMenu != null)
            //{
            //    if (ribbon.ApplicationMenu == null)
            //        ribbon.ApplicationMenu = new RibbonApplicationMenu();
            //    MergeItemsControl(sourceView, moduleRibbon.ApplicationMenu, ribbon.ApplicationMenu);
            //}
        }

        protected void MergeContextualTabGroups(object sourceView, Fluent.Ribbon moduleRibbon, Fluent.Ribbon ribbon)
        {
            //foreach (RibbonContextualTabGroup group in moduleRibbon.ContextualTabGroups)
            //{
            //    if (!ribbon.ContextualTabGroups.Any(t => UiElementsHaveSameId(t, group)))
            //    {
            //        DisconnectFromParent(group);
            //        if (group.DataContext == null)
            //            group.DataContext = moduleRibbon.DataContext;
            //        InsertItem(sourceView, group, ribbon.ContextualTabGroups);
            //    }
            //}
        }

        //protected void MergeItems(IList list, ItemsControl target)
        //{
        //    if (list == null)
        //        return;

        //    foreach (object item in list)
        //    {
        //        if (item is ItemsControl)
        //            MergeItemsControl(item, (ItemsControl)item, target);
        //        else
        //            MergeNonItemsControl(item as UIElement, target);
        //    }
        //}

        //protected void MergeNonItemsControl(UIElement item, ItemsControl target)
        //{
        //    if (item == null)
        //        return;
        //    InsertItem(item, item, target.Items);
        //}

        //protected void MergeItemsControl(object sourceView, ItemsControl source, ItemsControl target)
        //{
        //    var items = source.ItemsSource;
        //    foreach (UIElement item in items)
        //    {
        //        //if (item is ItemsControl)
        //        //{
        //           //var existingItem = target.ItemsSource
        //            //    .OfType<ItemsControl>()
        //             //   .FirstOrDefault(t => UiElementsHaveSameId(t, item));
        //           // if (existingItem == null)
        //                InsertItem(sourceView, item, target.ItemsSource);
        //            //else
        //               // MergeItemsControl(sourceView, (ItemsControl)item, existingItem);
        //        //}
        //        //else
        //        //{
        //            //InsertItem(sourceView, item, target.ItemsSource);
        //        //}
        //    }
        //}

        //protected void InsertItem(object sourceView, UIElement item, IEnumerable target)
        //{
        //    var targetList = target.Cast<UIElement>().ToList();
        //    DisconnectFromParent(item);
        //    InsertSorted(item, targetList);
        //}

        protected internal bool UiElementsHaveSameId(UIElement item1, UIElement item2)
        {
            var item1Id = GetUiElementHeaderOrName(item1);
            var item2Id = GetUiElementHeaderOrName(item2);

            return item1Id.Equals(item2Id);
        }

        protected string GetUiElementHeaderOrName(UIElement item)
        {
            var key = string.Empty;

            if (item is HeaderedItemsControl)
                key = ((HeaderedItemsControl)item).Header as string;
            if (string.IsNullOrEmpty(key) && item is FrameworkElement)
                key = ((FrameworkElement)item).Name;

            return key;
        }

        //public void InsertSorted(UIElement item, IList collection)
        //{
        //    var order = UIElementExtension.GetMergeOrder(item);
        //    if (Math.Abs(order - DefaultMergeOrder) < 0.001)
        //    {
        //        order = DefaultMergeOrder;
        //        UIElementExtension.SetMergeOrder(item, order);
        //    }

        //    int insertPosition = 0;
        //    foreach (UIElement t in collection)
        //    {
        //        var curOrder = UIElementExtension.GetMergeOrder(t);
        //        if (curOrder > order)
        //            break;
        //        insertPosition++;
        //    }
        //    DisconnectFromParent(item);
        //    collection.Insert(insertPosition, item);
        //}

        public T GetChildOfType<T>(DependencyObject depObj)
                where T : DependencyObject
        {
            if (depObj == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = (child as T) ?? GetChildOfType<T>(child);
                if (result != null) return result;
            }
            return null;
        }

        public DependencyObject GetParent(UIElement child)
        {
            DependencyObject parent = null;
            if (child is FrameworkElement)
                parent = ((FrameworkElement)child).Parent;

            if (parent == null)
                parent = VisualTreeHelper.GetParent(child);

            return parent;
        }

        public object GetDataContext(UIElement child, DependencyObject parent)
        {
            object dataContext = null;
            if (child is FrameworkElement)
            {
                dataContext = ((FrameworkElement)child).DataContext;
                if (dataContext == null && parent is FrameworkElement)
                    dataContext = ((FrameworkElement)parent).DataContext;
            }

            return dataContext;
        }

        private void DisconnectFromParent(object sourceView, RibbonTabItem tab)
        {
            //var parent = GetParent(tab) as Panel;

            var s = (FrameworkElement)sourceView;

            var dataContext = s.DataContext;

            //parent.Children.Remove(tab);
            //var parentAsItemsControl = parent as System.Windows.Controls.ItemsControl;
            //if (parentAsItemsControl != null)
            //{
            //    parentAsItemsControl.Items.Remove(child);
            //}

            if (tab is FrameworkElement && dataContext != null)
                ((FrameworkElement)tab).DataContext = dataContext;
        }

        public void DisconnectFromParent(object sourceView, RibbonGroupBox child)
        {
            var parent = GetParent(child) as Panel;

            var s = (FrameworkElement)sourceView;

            var dataContext = s.DataContext;

            parent.Children.Remove(child);
            //var parentAsItemsControl = parent as System.Windows.Controls.ItemsControl;
            //if (parentAsItemsControl != null)
            //{
            //    parentAsItemsControl.Items.Remove(child);
            //}

            if (child is FrameworkElement && dataContext != null)
                ((FrameworkElement)child).DataContext = dataContext;
        }

    }
}
