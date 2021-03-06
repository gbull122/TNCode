﻿using Prism.Regions;

namespace TnCode.TnCodeApp.Ribbon
{
	public class RibbonRegionAdapter : RegionAdapterBase<Fluent.Ribbon>
	{

		/// <summary>
		/// Initializes a new instance of <see cref="RibbonRegionAdapter"/>.
		/// </summary>
		/// <param name="regionBehaviorFactory">The factory used to create the region behaviors to attach to the created regions.</param>
		public RibbonRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
			: base(regionBehaviorFactory)
		{
		}

		/// <summary>
		/// Adapts a <see cref="ContentControl"/> to an <see cref="IRegion"/>.
		/// </summary>
		/// <param name="region">The new region being used.</param>
		/// <param name="regionTarget">The object to adapt.</param>
		protected override void Adapt(IRegion region, Fluent.Ribbon regionTarget)
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
