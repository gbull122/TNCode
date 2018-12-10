//-----------------------------------------------------------------------
// <copyright file="DockingManagerRegionAdapter.cs" company="Development In Progress Ltd">
//     Copyright © 2012. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------


using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.AvalonDock;

namespace TNCodeApp.Docking
{
    public class DockingRegionAdapter : RegionAdapterBase<DockingManager>
    {
        public DockingRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
           : base(regionBehaviorFactory)
        {
        }

        protected override void Adapt(IRegion region, DockingManager regionTarget)
        {
            
        }

        protected override IRegion CreateRegion()
        {
            return new Region();
        }

        protected override void AttachBehaviors(IRegion region, DockingManager regionTarget)
        {
            if (region == null)
            {
                throw new System.ArgumentNullException("region");
            }

            region.Behaviors.Add(DockingBehavior.BehaviorKey,
                new DockingBehavior()
                {
                    HostControl = regionTarget
                });

            base.AttachBehaviors(region, regionTarget);
        }
    }
}
