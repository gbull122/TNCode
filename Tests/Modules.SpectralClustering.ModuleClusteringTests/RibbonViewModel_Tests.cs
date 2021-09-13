using Clustering.ViewModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Events;
using TnCode.TnCodeApp.Data;
using TnCode.TnCodeApp.Progress;

namespace Modules.SpectralClustering.ModuleClusteringTests
{
    [TestClass]
    public class RibbonViewModel_Tests
    {
        [TestMethod]
        public void SortResults_Test()
        {

            int[] group1 = new int[] { 4, 2 };
            int[] group2 = new int[] { 5, 3, 1 };

            int[][] input = new int[][] { group1, group2 };

            var expectedLength = 5;

            var eventAgg = A.Fake<IEventAggregator>();
            var dataMgr = A.Fake<IDataSetsManager>();
            var progService = A.Fake<IProgressService>();

            var ribbonViewModel = new RibbonViewModel(eventAgg, dataMgr, progService);

            //var actualVariable = ribbonViewModel.SortResults(input);

            //var actualLength = actualVariable.Length;

            //actualLength.Should().Be(expectedLength);
        }
    }
}
