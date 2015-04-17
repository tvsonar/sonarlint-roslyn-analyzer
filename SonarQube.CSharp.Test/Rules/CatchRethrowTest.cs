﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SonarQube.CSharp.CodeAnalysis.Rules;

namespace SonarQube.CSharp.Test.Rules
{
    [TestClass]
    public class CatchRethrowTest
    {
        [TestMethod]
        public void CatchRethrow()
        {
            Verifier.Verify(@"TestCases\CatchRethrow.cs", new CatchRethrow());
        }
    }
}