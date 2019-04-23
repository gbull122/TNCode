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

        /// <summary>
        /// The DockingManagerBehavior's key used to 
        /// identify it in the region's behavior collection.
        /// </summary>
        public static readonly string BehaviorKey = "DockingManagerBehavior";

        /// <summary>
        /// Register to handle region events.
        /// </summary>
        protected override void OnAttach()
        {
            Region.ActiveViews.CollectionChanged += ActiveViewsCollectionChanged;
            Region.Views.CollectionChanged += ViewsCollectionChanged;
        }

        /// <summary>
        /// Handles changes to the <see cref="IRegion"/> views collection, adding  
        /// or removing items to the <see cref="HostControl"/> document source.
        /// </summary>
        /// <param name="sender">The <see cref="IRegion"/> views collection.</param>
        /// <param name="e">Event arguments.</param>
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


                    if (layoutDocumentPaneControl != null && tnPanel.Docking==DockingMethod.Document)
                    {
                        var document = new LayoutAnchorable();
                        document.CanClose = true;
                        document.CanAutoHide = false;
                        document.Closed += LayoutAnchorableClosed;
                        
                        document.Content = newItem;

                        var layoutDocumentPane = (LayoutDocumentPane)layoutDocumentPaneControl.Model;

                        layoutDocumentPane.Children.Add(document);
                        return;
                    }
                   

                    var anchorablePanel = new LayoutAnchorable();
                    anchorablePanel.Closed += LayoutAnchorableClosed;
                    //layoutAnchorable1.CanClose = true;
                    //layoutAnchorable1.CanAutoHide = false;
                    anchorablePanel.Content = newItem;

                    if(tnPanel.Docking == DockingMethod.ControlPanel)
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

        /// <summary>
        /// Removes the document from the <see cref="IRegion"/> after it has been closed.
        /// If you don't do this the document will not be removed but just hidden and you
        /// will not be able to open it again as prism will already think it exists.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">LayoutAnchorable closed event arguments.</param>
        private void LayoutAnchorableClosed(object sender, EventArgs e)
        {
            var layoutAnchorable = sender as LayoutAnchorable;
            if (layoutAnchorable == null)
            {
                return;
            }

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
            //if (e.Action == NotifyCollectionChangedAction.Add)
            //{
            //    if (dockingManager.ActiveContent != null
            //        && dockingManager.ActiveContent != e.NewItems[0]
            //        && Region.ActiveViews.Contains(dockingManager.ActiveContent))
            //    {
            //        Region.Deactivate(dockingManager.ActiveContent);
            //    }

            //    dockingManager.ActiveContent = e.NewItems[0];
            //}
            //else if (e.Action == NotifyCollectionChangedAction.Remove
            //    && e.OldItems.Contains(dockingManager.ActiveContent))
            //{
            //    dockingManager.ActiveContent = null;
            //}
        }

        private object GetDataContext(object item)
        {
            var frameworkElement = item as FrameworkElement;
            return frameworkElement == null ? item : frameworkElement.DataContext;
        }
    }
}
