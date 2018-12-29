using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Ribbon;

namespace TNCodeApp.Menu
{
    public class RibbonRegionAdapter : RegionAdapterBase<Ribbon>
    {

        public RibbonRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
          : base(regionBehaviorFactory)
        {
        }

        protected override void Adapt(IRegion region, Ribbon regionTarget)
        {
            region.Behaviors.Add(RibbonBehavior.BehaviorKey,
                 new RibbonBehavior()
                 {
                     MainRibbon = regionTarget
                 });

            base.AttachBehaviors(region, regionTarget);
        }

        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }
    }
}
