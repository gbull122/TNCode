using Prism.Regions;
using Prism.Regions.Behaviors;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace TNCodeApp.Docking
{
    /// <summary>
    /// Encapsulates behaviours related to the docking manager, keeping
    /// the documents source in sync with the active views of the <see cref="IRegion"/>. 
    /// </summary>
    public class DockingBehavior : RegionBehavior, IHostAwareRegionBehavior
    {
        private DockingManager dockingManager;

        /// <summary>
        /// The target control for the <see cref="IRegion"/>.
        /// </summary>
        public DependencyObject HostControl
        {
            get
            {
                return dockingManager;
            }

            set
            {
                dockingManager = value as DockingManager;

            }
        }

        public static readonly string BehaviorKey = "DockingManagerBehavior";


        protected override void OnAttach()
        {
            Region.ActiveViews.CollectionChanged += ActiveViewsCollectionChanged;
            Region.Views.CollectionChanged += ViewsCollectionChanged;
        }

        private void ViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (object newItem in e.NewItems)
                {
                    var tnPanel = (ITnPanel)GetDataContext(newItem) as ITnPanel;

                    var layoutDocumentPaneControl
                           = dockingManager
                           .FindVisualChildren<LayoutDocumentPaneControl>()
                           .FirstOrDefault();


                    if (layoutDocumentPaneControl != null && tnPanel.Docking == DockingMethod.Document)
                    {
                        var document = new LayoutAnchorable
                        {
                            CanClose = true,
                            CanAutoHide = false
                        };
                        document.Closed += DocumentAnchorableClosed;

                        document.Content = newItem;

                        var layoutDocumentPane = (LayoutDocumentPane)layoutDocumentPaneControl.Model;

                        layoutDocumentPane.Children.Add(document);
                        return;
                    }

                    var anchorablePanel = new LayoutAnchorable();
                    anchorablePanel.Content = newItem;

                    if (tnPanel.Docking == DockingMethod.ControlPanel)
                    {
                        var controlPanel = dockingManager.FindName("ControlPanel") as LayoutAnchorablePane;
                        controlPanel.Children.Add(anchorablePanel);
                        return;
                    }

                    var statusPanel = dockingManager.FindName("StatusPanel") as LayoutAnchorablePane;
                    statusPanel.Children.Add(anchorablePanel);
                }
            }
        }

        private void DocumentAnchorableClosed(object sender, EventArgs e)
        {
            var layoutAnchorable = sender as LayoutAnchorable;
            if (layoutAnchorable == null)
                return;

            var view = layoutAnchorable.Content;
            if (Region.Views.Contains(view))
            {
                Region.RegionManager.Regions["MainRegion"].Remove(view);
            }
        }

        /// <summary>
        /// Keeps the active content of the <see cref="IRegion"/> in sync with the <see cref="HostControl"/>.
        /// </summary>
        /// <param name="sender">The <see cref="IRegion"/> views collection.</param>
        /// <param name="e">Event arguments.</param>
        private void ActiveViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (object newItem in e.NewItems)
                {
                    var tnPanel = (ITnPanel)GetDataContext(newItem) as ITnPanel;

                    var anchorablePanel = new LayoutAnchorable();
                    anchorablePanel.Content = newItem;

                    if (tnPanel.Docking == DockingMethod.ControlPanel)
                    {
                        var controlPanel = dockingManager.FindName("ControlPanel") as LayoutAnchorablePane;
                        controlPanel.Children.Add(anchorablePanel);
                        return;
                    }

                    var statusPanel = dockingManager.FindName("StatusPanel") as LayoutAnchorablePane;
                    statusPanel.Children.Add(anchorablePanel);
                }
            }
        }

        private object GetDataContext(object item)
        {
            var frameworkElement = item as FrameworkElement;
            return frameworkElement == null ? item : frameworkElement.DataContext;
        }
    }
}
