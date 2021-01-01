using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TnCode.TnCodeApp.R.ViewModels;

namespace TnCodeApp_Tests
{
    [TestClass]
    public class TitlesViewModel_Tests
    {

        [TestMethod]
        public void WhenNoTitlesThenParamZero()
        {
            TitlesViewModel titlesViewModel = new TitlesViewModel();

            var actualParams = titlesViewModel.GetParameters();

            int paramCount = actualParams.Count;
            paramCount.Should().Be(0);
        }

        [TestMethod]
        public void WhenOneTitleSetThenOneParam()
        {
            TitlesViewModel titlesViewModel = new TitlesViewModel();

            titlesViewModel.XAxisTitle = "xAxis";

            var actualParams = titlesViewModel.GetParameters();

            int paramCount = actualParams.Count;
            paramCount.Should().Be(1);
        }

        [TestMethod]
        public void WhenAllTitlesSetThenFiveParams()
        {
            TitlesViewModel titlesViewModel = new TitlesViewModel();

            titlesViewModel.XAxisTitle = "xAxis";
            titlesViewModel.YAxisTitle = "yAxis";
            titlesViewModel.MainTitle = "main";
            titlesViewModel.SubTitle = "sub";
            titlesViewModel.Caption = "Caption";

            var actualParams = titlesViewModel.GetParameters();

            int paramCount = actualParams.Count;
            paramCount.Should().Be(5);
        }
    }
}
