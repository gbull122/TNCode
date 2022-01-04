namespace TnCode.TnCodeApp.Ribbon
{
	using Prism.Regions;
	using Fluent;

	public class RibbonRegionAdapter : RegionAdapterBase<Ribbon>
	{
		public RibbonRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
			: base(regionBehaviorFactory)
		{
		}

		protected override void Adapt(IRegion region, Ribbon regionTarget)
		{
			region.Behaviors.Add(RibbonBehavoir.BehaviorKey,
				new RibbonBehavoir()
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
