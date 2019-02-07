using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModuleR.Charts.Ggplot.Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleR_Tests
{
    [TestClass]
    public class Aesthetic_Tests
    {
        [TestMethod]
        public void Empty_Test()
        {
            var aesthetic = new Aesthetic();

            var command = aesthetic.Command();

            command.Should().Be("aes()");
        }


        [TestMethod]
        public void Simple_Test()
        {
            var aesthetic = new Aesthetic();
            //aesthetic.XVariable.Entry = "Xvar";
            var command = aesthetic.Command();

            command.Should().Be("aes(x=Xvar)");
        }

        [TestMethod]
        public void Factor_Test()
        {
            var aesthetic = new Aesthetic();
            //aesthetic.XVariable.Entry = "Xvar";
            //aesthetic.Fill.Entry = "fillVar";
            //aesthetic.Fill.IsFactor = true;
            var command = aesthetic.Command();

            command.Should().Be("aes(x=Xvar,fill=as.factor(fillVar))");
        }

        [TestMethod]
        public void LowerCase_Test()
        {
            var aesthetic = new Aesthetic();
            //aesthetic.XVariable.Entry = "Xvar";
            //aesthetic.Fill.Entry = "fillVar";
            //aesthetic.Fill.IsFactor = true;
            //aesthetic.Fill.UseLowerCase = true;
            var command = aesthetic.Command();

            command.Should().Be("aes(x=Xvar,fill=as.factor(fillvar))");
        }
    }
}
